using DataLibrary.Models.OldTables;
using System;
using System.Collections.Generic;
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

        public async Task CmdCheckoutCartAsync(SiteUser siteUser)
        {
            List<CartItemJoin> cartItems = await GetJoinedCartItemListAsync(siteUser.CurrentCartId);

            using (var transaction = connection.BeginTransaction())
            {
                siteUser.OrderConfirmationId++;
                siteUser.Update(this, transaction);
                CmdCloneCart(cartItems, siteUser.ConfirmOrderCartId, transaction);

                transaction.Commit();
            }
        }

        public async Task<bool> CmdUserOwnsDeliveryAddressAsync(int userId, int deliveryAddressId)
        {
            DeliveryAddress address = await GetAsync<DeliveryAddress>(deliveryAddressId);

            if (address != null)
            {
                if (address.UserId == userId)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> CmdUserOwnsCartItemAsync(SiteUser user, int cartItemId)
        {
            List<CartItem> userCartItems = await GetListAsync<CartItem>(new { CartId = user.CurrentCartId });

            foreach (CartItem cartItem in userCartItems)
            {
                if (cartItem.Id == cartItemId)
                {
                    return true;
                }
            }

            return false;
        }

        public int CmdDeleteAllCartItems(int cartId, IDbTransaction transaction)
        {
            string deleteQuerySql = @"delete from dbo.CartItem where CartId = @CartId;";
            return connection.Execute(deleteQuerySql, new { CartId = cartId }, transaction);
        }

        private void CmdCloneCart(List<CartItemJoin> cartItems, int destinationCartId, IDbTransaction transaction = null)
        {
            CmdDeleteAllCartItems(destinationCartId, transaction);

            foreach (CartItemJoin cartItem in cartItems)
            {
                cartItem.CartItem.Id = 0;
                cartItem.CartItem.CartId = destinationCartId;
                cartItem.Insert(this, transaction);
            }
        }

        public int CmdMoveCartItems(int sourceCartId, int destinationCartId, IDbTransaction transaction = null)
        {
            string updateQuerySql = @"update dbo.CartItem set CartId = @DestinationCartId where CartId = @SourceCartId";

            object queryParameters = new
            {
                SourceCartId = sourceCartId,
                DestinationCartId = destinationCartId
            };

            return connection.Execute(updateQuerySql, queryParameters, transaction);
        }

        public async Task<int> CmdUpdateCartItemQuantityAsync(int cartItemId, int quantity, IDbTransaction transaction = null)
        {
            string updateQuerySql = @"update dbo.CartItem set Quantity = @Quantity where Id = @Id;";

            object queryParameters = new
            {
                Id = cartItemId,
                Quantity = quantity
            };

            return await connection.ExecuteAsync(updateQuerySql, queryParameters, transaction);
        }
    }
}