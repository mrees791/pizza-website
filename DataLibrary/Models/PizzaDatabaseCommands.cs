using Dapper;
using DataLibrary.Models.Exceptions;
using DataLibrary.Models.Joins;
using DataLibrary.Models.Tables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    /// <summary>
    /// Provides special commands for the PizzaDatabase class that go beyond simple CRUD operations
    /// </summary>
    public class PizzaDatabaseCommands
    {
        private PizzaDatabase pizzaDb;

        public PizzaDatabaseCommands(PizzaDatabase pizzaDb)
        {
            this.pizzaDb = pizzaDb;
        }

        public async Task AddNewEmployee(string employeeId, string userName, bool isManager)
        {
            SiteUser user = await pizzaDb.GetSiteUserByNameAsync(userName);

            using (IDbTransaction transaction = pizzaDb.Connection.BeginTransaction())
            {
                Employee employee = new Employee()
                {
                    Id = employeeId,
                    UserId = user.Id,
                    CurrentlyEmployed = true
                };

                UserRole employeeRole = new UserRole()
                {
                    UserId = user.Id,
                    RoleName = "Employee"
                };

                await employee.InsertAsync(pizzaDb, transaction);
                await employeeRole.InsertAsync(pizzaDb, transaction);

                if (isManager)
                {
                    UserRole managerRole = new UserRole()
                    {
                        UserId = user.Id,
                        RoleName = "Manager"
                    };

                    await managerRole.InsertAsync(pizzaDb, transaction);
                }

                transaction.Commit();
            }
        }

        public async Task ReorderPreviousOrder(SiteUser siteUser, CustomerOrder previousOrder)
        {
            IEnumerable<CartItemJoin> cartItems = await pizzaDb.GetJoinedCartItemListAsync(previousOrder.CartId);

            using (var transaction = pizzaDb.Connection.BeginTransaction())
            {
                await CloneCart(siteUser.CurrentCartId, true, cartItems, transaction);

                transaction.Commit();
            }
        }

        public async Task CheckoutCartAsync(SiteUser siteUser)
        {
            IEnumerable<CartItemJoin> cartItems = await pizzaDb.GetJoinedCartItemListAsync(siteUser.CurrentCartId);

            using (var transaction = pizzaDb.Connection.BeginTransaction())
            {
                siteUser.OrderConfirmationId++;
                await siteUser.UpdateAsync(pizzaDb, transaction);
                await CloneCart(siteUser.ConfirmOrderCartId, true, cartItems, transaction);

                transaction.Commit();
            }
        }

        public async Task<decimal> CalculateCartSubtotalAsync(int cartId)
        {
            IEnumerable<CartItem> cartItems = await pizzaDb.GetListAsync<CartItem>(new { CartId = cartId });
            return cartItems.Sum(i => i.Price);
        }

        public async Task SubmitCustomerOrderAsync(SiteUser siteUser, CustomerOrder customerOrder, DeliveryInfo deliveryInfo = null)
        {
            using (var transaction = pizzaDb.Connection.BeginTransaction())
            {
                Cart cart = new Cart();
                customerOrder.CartId = await cart.InsertAsync(pizzaDb, transaction);
                await MoveCartItems(siteUser.ConfirmOrderCartId, cart.Id, transaction);

                if (deliveryInfo != null)
                {
                    customerOrder.IsDelivery = true;
                    customerOrder.DeliveryInfoId = await deliveryInfo.InsertAsync(pizzaDb, transaction);
                }

                await customerOrder.InsertAsync(pizzaDb, transaction);
                await DeleteAllCartItemsAsync(siteUser.CurrentCartId, transaction);
                await DeleteAllCartItemsAsync(siteUser.ConfirmOrderCartId, transaction);

                transaction.Commit();
            }
        }

        public async Task<bool> UserOwnsDeliveryAddressAsync(int userId, DeliveryAddress deliveryAddress)
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

        public async Task<bool> UserOwnsCartItemAsync(int userId, CartItem cartItem)
        {
            return await Task.FromResult(cartItem.UserId == userId);
        }

        public async Task<int> DeleteAllCartItemsAsync(int cartId, IDbTransaction transaction)
        {
            string deleteQuerySql = @"delete from dbo.CartItem where CartId = @CartId;";
            return await pizzaDb.Connection.ExecuteAsync(deleteQuerySql, new { CartId = cartId }, transaction);
        }

        private async Task CloneCart(int destinationCartId, bool clearDestinationCart, IEnumerable<CartItemJoin> cartItems, IDbTransaction transaction = null)
        {
            if (clearDestinationCart)
            {
                await DeleteAllCartItemsAsync(destinationCartId, transaction);
            }

            foreach (CartItemJoin cartItem in cartItems)
            {
                cartItem.CartItem.Id = 0;
                cartItem.CartItem.CartId = destinationCartId;
                await cartItem.InsertAsync(pizzaDb, transaction);
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

            return await pizzaDb.Connection.ExecuteAsync(updateQuerySql, queryParameters, transaction);
        }

        public async Task<CartItem> UpdateCartItemQuantityAsync(CartItem cartItem, int quantity)
        {
            using (IDbTransaction transaction = pizzaDb.Connection.BeginTransaction())
            {
                cartItem.Quantity = quantity;
                cartItem.Price = cartItem.Quantity * cartItem.PricePerItem;
                await cartItem.UpdateAsync(pizzaDb, transaction);

                transaction.Commit();
            }

            return cartItem;
        }
    }
}