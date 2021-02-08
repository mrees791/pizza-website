using Dapper;
using DataLibrary.DataAccess;
using DataLibrary.Models.Carts;
using DataLibrary.Models.Pizzas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic.Carts
{
    public static class DatabaseCartProcessor
    {
        internal static int UpdateCartItem(CartItemModel cartItem, IDbConnection connection, IDbTransaction transaction)
        {
            string updateSql = @"update dbo.CartItem set PricePerItem = @PricePerItem, Quantity = @Quantity where Id = @Id;";

            object queryParameters = new
            {
                Id = cartItem.CartItemId,
                PricePerItem = cartItem.PricePerItem,
                Quantity = cartItem.Quantity
            };

            int cartItemRowsAffected = SqlDataAccess.UpdateRecord(updateSql, queryParameters, connection, transaction);

            return cartItemRowsAffected;
        }

        internal static int DeleteCartItem(CartItemModel cartItem, IDbConnection connection, IDbTransaction transaction)
        {
            string deleteCartItemSql = @"delete from dbo.CartItem where Id = @CartItemId;";
            int cartPizzaRowsDeleted = SqlDataAccess.DeleteRecord(deleteCartItemSql, cartItem, connection, transaction);

            return cartPizzaRowsDeleted;
        }

        internal static int AddCartItem(CartItemModel cartItem, IDbConnection connection, IDbTransaction transaction)
        {
            string insertCartItemSql = @"insert into dbo.CartItem (CartId, PricePerItem, Quantity) output Inserted.Id
                                         values(@CartId, @PricePerItem, @Quantity);";

            object cartItemQueryParameters = new
            {
                CartId = cartItem.CartId,
                PricePerItem = cartItem.PricePerItem,
                Quantity = cartItem.Quantity
            };

            cartItem.CartItemId = SqlDataAccess.SaveNewRecord(insertCartItemSql, cartItemQueryParameters, connection, transaction);

            return cartItem.CartItemId;
        }

        internal static int AddNewCart(IDbConnection connection, IDbTransaction transaction)
        {
            string insertCartSql = @"insert into dbo.Cart output Inserted.Id default values;";

            return SqlDataAccess.SaveNewRecord(insertCartSql, null, connection, transaction);
        }

        internal static List<CartItemModel> LoadAllCartItems()
        {
            List<CartItemModel> cartItems = new List<CartItemModel>();
            List<CartPizzaModel> cartPizzas = DatabaseCartPizzaProcessor.LoadCartPizzas().ToList();

            cartItems.AddRange(cartPizzas);
            cartItems.Sort();

            return cartItems;
        }

        public static List<CartModel> LoadAllCarts()
        {
            List<CartModel> carts = new List<CartModel>();
            List<CartItemModel> allCartItems = LoadAllCartItems();

            string selectCartQuerySql = @"select Id from dbo.Cart;";
            carts = SqlDataAccess.LoadData<CartModel>(selectCartQuerySql);

            foreach (CartModel cart in carts)
            {
                cart.CartItems.AddRange(allCartItems.Where(c => c.CartId == cart.Id));
            }

            return carts;
        }

        internal static int MoveCartItems(int originalCartId, int destinationCartId, IDbConnection connection, IDbTransaction transaction)
        {
            string updateSql = @"update dbo.CartItem set CartId = @DestinationCartId where CartId = @OriginalCartId;";

            object queryParameters = new
            {
                OriginalCartId = originalCartId,
                DestinationCartId = destinationCartId
            };

            return SqlDataAccess.UpdateRecord(updateSql, queryParameters, connection, transaction);
        }

        public static int MoveCartItems(int originalCartId, int destinationCartId)
        {
            int cartItemRowsAffected = 0;

            using (IDbConnection connection = new SqlConnection(SqlDataAccess.GetConnectiongString()))
            {
                connection.Open();

                cartItemRowsAffected = MoveCartItems(originalCartId, destinationCartId, connection, null);
            }

            return cartItemRowsAffected;
        }

        internal static int DeleteAllItemsInCart(CartModel cart, IDbConnection connection, IDbTransaction transaction)
        {
            int totalRowsDeleted = 0;

            // Delete all cart items
            foreach (CartItemModel cartItem in cart.CartItems)
            {
                if (cartItem is CartPizzaModel)
                {
                    totalRowsDeleted += DatabaseCartPizzaProcessor.DeleteCartPizza((CartPizzaModel)cartItem, connection, transaction);
                }
                else
                {
                    throw new Exception("Cart item type needs implemented.");
                }
            }

            return totalRowsDeleted;
        }

        public static int DeleteAllItemsInCart(int cartId)
        {
            int totalRowsDeleted = 0;
            CartModel cart = LoadAllCarts().Where(c => c.Id == cartId).First();

            using (IDbConnection connection = new SqlConnection(SqlDataAccess.GetConnectiongString()))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        totalRowsDeleted = DeleteAllItemsInCart(cart, connection, transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            return totalRowsDeleted;
        }

        /*


        internal static int CloneCart(CartModel originalCart, CartModel destinationCart, IDbConnection connection, IDbTransaction transaction)
        {
            int cartItemRowsAffected = 0;

            int itemsDeletedFromDestinationCart = DeleteAllItemsInCart(destinationCart, connection, transaction);
            cartItemRowsAffected += itemsDeletedFromDestinationCart;

            CartModel clonedCart = (CartModel)originalCart.Clone();

            foreach (CartItemModel cartItem in clonedCart.CartItems)
            {
                cartItem.CartId = destinationCart.Id;

                if (cartItem is CartPizzaModel)
                {
                    cartItem.Id = DatabaseCartPizzaProcessor.AddPizzaToCart((CartPizzaModel)cartItem, connection, transaction);
                }
                else
                {
                    throw new Exception("Cart item type needs implemented.");
                }
            }
            return cartItemRowsAffected;
        }

        public static int CloneCart(int originalCartId, int destinationCartId)
        {
            List<CartModel> carts = LoadCarts();
            CartModel originalCart = carts.Where(c => c.Id == originalCartId).First();
            CartModel destinationCart = carts.Where(c => c.Id == destinationCartId).First();

            int cartItemRowsAffected = 0;

            using (IDbConnection connection = new SqlConnection(SqlDataAccess.GetConnectiongString()))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        cartItemRowsAffected = CloneCart(originalCart, destinationCart, connection, transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            return cartItemRowsAffected;
        }*/
    }
}