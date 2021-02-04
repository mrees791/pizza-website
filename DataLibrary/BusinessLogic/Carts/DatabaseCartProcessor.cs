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

        public static List<CartModel> LoadCarts()
        {
            List<CartModel> carts = new List<CartModel>();
            List<CartPizzaModel> cartPizzas = DatabaseCartPizzaProcessor.LoadCartPizzas();

            // Load cart records
            string selectCartQuerySql = @"select Id from dbo.Cart;";
            carts = SqlDataAccess.LoadData<CartModel>(selectCartQuerySql);

            foreach (var cart in carts)
            {
                cart.CartPizzas.AddRange(cartPizzas.Where(c => c.CartId == cart.Id));
            }

            return carts;
        }

        public static int DeleteAllItemsInCart(int cartId)
        {
            int totalRowsDeleted = 0;

            using (IDbConnection connection = new SqlConnection(SqlDataAccess.GetConnectiongString()))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        totalRowsDeleted = DeleteAllItemsInCart(cartId, connection, transaction);
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

        internal static int DeleteAllItemsInCart(int cartId, IDbConnection connection, IDbTransaction transaction)
        {
            int totalRowsDeleted = 0;

            CartModel cart = LoadCarts().Where(c => c.Id == cartId).First();

            // Delete all cart pizza records
            foreach (var cartPizza in cart.CartPizzas)
            {
                totalRowsDeleted += DatabaseCartPizzaProcessor.DeleteCartPizza(cartPizza, connection, transaction);
            }

            return totalRowsDeleted;
        }

        internal static int CloneCart(int cartId, IDbConnection connection, IDbTransaction transaction)
        {
            CartModel originalCart = LoadCarts().Where(c => c.Id == cartId).First();
            int clonedCartId = AddNewCart(connection, transaction);

            // Clone all cart pizza records
            foreach (var cartPizza in originalCart.CartPizzas)
            {
                cartPizza.CartId = clonedCartId;
                int cartPizzaId = DatabaseCartPizzaProcessor.AddPizzaToCart(cartPizza, connection, transaction);
            }

            return clonedCartId;
        }

        public static int CloneCart(int cartId)
        {
            int clonedCartId = 0;

            using (IDbConnection connection = new SqlConnection(SqlDataAccess.GetConnectiongString()))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        clonedCartId = CloneCart(cartId, connection, transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            return clonedCartId;
        }
    }
}
