using Dapper;
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

        // todo: Add server side validation
        public async Task CheckoutCartAsync(SiteUser siteUser)
        {
            List<CartItemJoin> cartItems = new List<CartItemJoin>(await pizzaDb.GetJoinedCartItemListAsync(siteUser.CurrentCartId));

            using (var transaction = pizzaDb.Connection.BeginTransaction())
            {
                siteUser.OrderConfirmationId++;
                await siteUser.UpdateAsync(pizzaDb, transaction);
                await CloneCart(cartItems, siteUser.ConfirmOrderCartId, transaction);

                transaction.Commit();
            }
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

        public async Task<bool> UserOwnsDeliveryAddressAsync(int userId, int deliveryAddressId)
        {
            DeliveryAddress address = await pizzaDb.GetAsync<DeliveryAddress>(deliveryAddressId);

            if (address != null)
            {
                if (address.UserId == userId)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> UserOwnsCartItemAsync(SiteUser user, int cartItemId)
        {
            List<CartItem> userCartItems = new List<CartItem>(await pizzaDb.GetListAsync<CartItem>(new { CartId = user.CurrentCartId }));

            foreach (CartItem cartItem in userCartItems)
            {
                if (cartItem.Id == cartItemId)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<int> DeleteAllCartItemsAsync(int cartId, IDbTransaction transaction)
        {
            string deleteQuerySql = @"delete from dbo.CartItem where CartId = @CartId;";
            return await pizzaDb.Connection.ExecuteAsync(deleteQuerySql, new { CartId = cartId }, transaction);
        }

        private async Task CloneCart(List<CartItemJoin> cartItems, int destinationCartId, IDbTransaction transaction = null)
        {
            await DeleteAllCartItemsAsync(destinationCartId, transaction);

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

        public async Task<CartItem> UpdateCartItemQuantityAsync(int cartItemId, int quantity)
        {
            CartItem cartItem = await pizzaDb.GetAsync<CartItem>(cartItemId);

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