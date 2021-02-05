using Dapper;
using DataLibrary.DataAccess;
using DataLibrary.Models.Carts;
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
        internal static int AddNewCart(IDbConnection connection, IDbTransaction transaction)
        {
            string insertCartSql = @"insert into dbo.Cart output Inserted.Id default values;";

            return SqlDataAccess.SaveNewRecord(insertCartSql, new { }, connection, transaction);
        }

        public static List<CartItemModel> LoadAllCartItems()
        {
            List<CartItemModel> cartItems = new List<CartItemModel>();
            List<CartPizzaModel> cartPizzas = DatabaseCartPizzaProcessor.LoadCartPizzas().ToList();

            cartItems.AddRange(cartPizzas);
            cartItems.Sort();

            return cartItems;
        }

        public static List<CartModel> LoadCarts()
        {
            List<CartModel> carts = new List<CartModel>();
            List<CartItemModel> allCartItems = LoadAllCartItems();

            // Load cart records
            string selectCartQuerySql = @"select Id from dbo.Cart;";
            carts = SqlDataAccess.LoadData<CartModel>(selectCartQuerySql);

            foreach (CartModel cart in carts)
            {
                cart.CartItems.AddRange(allCartItems.Where(c => c.CartId == cart.Id));
            }

            return carts;
        }

        public static int DeleteAllItemsInCart(int cartId)
        {
            int totalRowsDeleted = 0;
            CartModel cart = LoadCarts().Where(c => c.Id == cartId).First();

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

        internal static int CloneCart(CartModel originalCart, CartModel destinationCart, IDbConnection connection, IDbTransaction transaction)
        {
            int cartItemRowsAffected = 0;

            int itemsDeletedFromDestinationCart = DeleteAllItemsInCart(destinationCart, connection, transaction);
            cartItemRowsAffected += itemsDeletedFromDestinationCart;

            // Clone all cart items
            foreach (CartItemModel cartItem in originalCart.CartItems)
            {
                if (cartItem is CartPizzaModel)
                {
                    CartPizzaModel clonedCartPizza = new CartPizzaModel()
                    {
                        CartId = destinationCart.Id,
                        DateAddedToCart = cartItem.DateAddedToCart,
                        Pizza = ((CartPizzaModel)cartItem).Pizza,
                        PricePerItem = cartItem.PricePerItem,
                        Quantity = cartItem.Quantity

                    };
                    int cartPizzaId = DatabaseCartPizzaProcessor.AddPizzaToCart(clonedCartPizza, connection, transaction);
                    cartItemRowsAffected++;
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
        }

        /*internal static int MoveCartItems(int originalCartId, int destinationCartId, IDbConnection connection, IDbTransaction transaction)
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
                        cartItemRowsAffected += CloneCart(originalCart, destinationCart, connection, transaction);
                        cartItemRowsAffected += DeleteAllItemsInCart(originalCart, connection, transaction);
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
