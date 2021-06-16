using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DataLibrary.Models.JoinLists;
using DataLibrary.Models.Tables;

namespace DataLibrary.Models
{
    /// <summary>
    ///     Provides special commands for the PizzaDatabase class that go beyond simple CRUD operations
    /// </summary>
    public class PizzaDatabaseCommands
    {
        private readonly PizzaDatabase _pizzaDb;

        public PizzaDatabaseCommands(PizzaDatabase pizzaDb)
        {
            _pizzaDb = pizzaDb;
        }

        public async Task<int> AddItemToCart(SiteUser siteUser, CartItem cartItem, CartItemCategory cartItemCategory)
        {
            using (IDbTransaction transaction = _pizzaDb.Connection.BeginTransaction())
            {
                await InsertAsync(cartItem, cartItemCategory, transaction);
                transaction.Commit();
            }

            return cartItem.GetId();
        }

        private async Task<int> InsertAsync(CartItem cartItem, CartItemCategory cartItemCategory,
            IDbTransaction transaction)
        {
            await cartItem.InsertAsync(_pizzaDb, transaction);
            cartItemCategory.CartItemId = cartItem.Id;
            await cartItemCategory.InsertAsync(_pizzaDb, transaction);

            return cartItem.GetId();
        }

        public async Task<int> UpdateCartItemAsync(CartItem cartItem, CartItemCategory cartItemCategory)
        {
            int rowsUpdated = 0;

            using (IDbTransaction transaction = _pizzaDb.Connection.BeginTransaction())
            {
                rowsUpdated += await cartItem.UpdateAsync(_pizzaDb, transaction);
                rowsUpdated += await cartItemCategory.UpdateAsync(_pizzaDb, transaction);
                transaction.Commit();
            }

            return rowsUpdated;
        }

        public async Task<bool> IsEmployedAtLocationAsync(Employee employee, StoreLocation storeLocation,
            IDbTransaction transaction = null)
        {
            return await _pizzaDb.GetEmployeeLocationAsync(employee, storeLocation, transaction) != null;
        }

        public async Task AddNewEmployeeAsync(string employeeId, bool isManager, SiteUser user)
        {
            SiteRole employeeRole = await _pizzaDb.GetAsync<SiteRole>("Employee");
            SiteRole managerRole = await _pizzaDb.GetAsync<SiteRole>("Manager");

            using (IDbTransaction transaction = _pizzaDb.Connection.BeginTransaction())
            {
                Employee employee = new Employee
                {
                    Id = employeeId,
                    UserId = user.Id
                };

                await employee.InsertAsync(_pizzaDb, transaction);
                await AddUserToRoleAsync(user, employeeRole, transaction);

                if (isManager)
                {
                    await AddUserToRoleAsync(user, managerRole, transaction);
                }

                transaction.Commit();
            }
        }

        public async Task AddUserToRoleAsync(SiteUser siteUser, SiteRole siteRole, IDbTransaction transaction = null)
        {
            UserRole userRole = new UserRole
            {
                UserId = siteUser.Id,
                RoleName = siteRole.Name
            };
            await userRole.InsertAsync(_pizzaDb, transaction);
        }

        public async Task<int> RemoveUserFromRoleAsync(SiteUser user, SiteRole siteRole,
            IDbTransaction transaction = null)
        {
            UserRole userRole = await _pizzaDb.GetUserRoleAsync(user, siteRole, transaction);
            return await _pizzaDb.DeleteAsync(userRole, transaction);
        }

        public async Task ReorderPreviousOrder(SiteUser siteUser, CustomerOrder previousOrder)
        {
            CartItemJoinList cartItemJoinList = new CartItemJoinList();
            await cartItemJoinList.LoadListByCartIdAsync(previousOrder.CartId, _pizzaDb);

            using (IDbTransaction transaction = _pizzaDb.Connection.BeginTransaction())
            {
                await CloneCart(siteUser.CurrentCartId, true, cartItemJoinList.Items, transaction);
                transaction.Commit();
            }
        }

        public async Task CheckoutCartAsync(SiteUser siteUser)
        {
            CartItemJoinList cartItemJoinList = new CartItemJoinList();
            await cartItemJoinList.LoadListByCartIdAsync(siteUser.CurrentCartId, _pizzaDb);

            using (IDbTransaction transaction = _pizzaDb.Connection.BeginTransaction())
            {
                siteUser.OrderConfirmationId++;
                await siteUser.UpdateAsync(_pizzaDb, transaction);
                await CloneCart(siteUser.ConfirmOrderCartId, true, cartItemJoinList.Items, transaction);

                transaction.Commit();
            }
        }

        public async Task<decimal> CalculateCartSubtotalAsync(int cartId)
        {
            IEnumerable<CartItem> cartItems = await _pizzaDb.GetListAsync<CartItem>(new {CartId = cartId});
            return cartItems.Sum(i => i.Price);
        }

        public async Task AddCustomerOrderAsync(SiteUser siteUser, CustomerOrder customerOrder,
            DeliveryInfo deliveryInfo = null)
        {
            using (IDbTransaction transaction = _pizzaDb.Connection.BeginTransaction())
            {
                Cart cart = new Cart();
                customerOrder.CartId = await cart.InsertAsync(_pizzaDb, transaction);
                await MoveCartItems(siteUser.ConfirmOrderCartId, cart.Id, transaction);

                if (deliveryInfo != null)
                {
                    customerOrder.IsDelivery = true;
                    customerOrder.DeliveryInfoId = await deliveryInfo.InsertAsync(_pizzaDb, transaction);
                }

                await customerOrder.InsertAsync(_pizzaDb, transaction);
                await DeleteAllCartItemsAsync(siteUser.CurrentCartId, transaction);
                await DeleteAllCartItemsAsync(siteUser.ConfirmOrderCartId, transaction);

                transaction.Commit();
            }
        }

        public async Task<bool> UserOwnsDeliveryAddressAsync(string userId, DeliveryAddress deliveryAddress)
        {
            return await Task.FromResult(deliveryAddress.UserId == userId);
        }

        public async Task<bool> UserOwnsCartAsync(SiteUser siteUser, Cart cart)
        {
            return await Task.FromResult(siteUser.CurrentCartId == cart.Id);
        }

        public async Task<bool> UserOwnsCustomerOrderAsync(SiteUser siteUser, CustomerOrder customerOrder)
        {
            return await Task.FromResult(customerOrder.UserId == siteUser.Id);
        }

        public async Task<bool> UserOwnsCartItemAsync(string userId, CartItem cartItem)
        {
            return await Task.FromResult(cartItem.UserId == userId);
        }

        public async Task<int> DeleteAllCartItemsAsync(int cartId, IDbTransaction transaction)
        {
            string deleteQuerySql = @"delete from dbo.CartItem where CartId = @CartId;";
            return await _pizzaDb.Connection.ExecuteAsync(deleteQuerySql, new {CartId = cartId}, transaction);
        }

        private async Task CloneCart(int destinationCartId, bool clearDestinationCart,
            IEnumerable<CartItemJoin> cartItems, IDbTransaction transaction)
        {
            if (clearDestinationCart)
            {
                await DeleteAllCartItemsAsync(destinationCartId, transaction);
            }

            foreach (CartItemJoin cartItemJoin in cartItems)
            {
                cartItemJoin.CartItem.Id = 0;
                cartItemJoin.CartItem.CartId = destinationCartId;

                await InsertAsync(cartItemJoin.CartItem, cartItemJoin.CartItemType, transaction);
            }
        }

        public async Task<int> MoveCartItems(int sourceCartId, int destinationCartId, IDbTransaction transaction = null)
        {
            string updateQuerySql = @"update dbo.CartItem set CartId = @DestinationCartId where CartId = @SourceCartId";

            object queryParameters = new
            {
                SourceCartId = sourceCartId,
                DestinationCartId = destinationCartId
            };

            return await _pizzaDb.Connection.ExecuteAsync(updateQuerySql, queryParameters, transaction);
        }

        public async Task<CartItem> UpdateCartItemQuantityAsync(CartItem cartItem, int quantity)
        {
            using (IDbTransaction transaction = _pizzaDb.Connection.BeginTransaction())
            {
                cartItem.Quantity = quantity;
                cartItem.Price = cartItem.Quantity * cartItem.PricePerItem;
                await cartItem.UpdateAsync(_pizzaDb, transaction);

                transaction.Commit();
            }

            return cartItem;
        }
    }
}