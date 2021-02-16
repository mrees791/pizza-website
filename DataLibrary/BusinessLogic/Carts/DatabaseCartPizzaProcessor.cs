using Dapper;
using DataLibrary.BusinessLogic.Pizzas;
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
    public static class DatabaseCartPizzaProcessor
    {
        internal static int AddPizzaToCart(CartPizzaModel cartPizza, IDbConnection connection, IDbTransaction transaction)
        {
            // Save pizza record
            cartPizza.Pizza.Id = DatabasePizzaProcessor.AddPizza(cartPizza.Pizza, connection, transaction);

            // Save cart item record
            cartPizza.CartItemId = DatabaseCartProcessor.AddCartItem(cartPizza, connection, transaction);

            // Save cart pizza record
            string insertCartPizzaSql = @"insert into dbo.CartPizza (CartItemId, PizzaId) output Inserted.Id
                                          values(@CartItemId, @PizzaId);";

            object cartPizzaQueryParameters = new
            {
                CartItemId = cartPizza.CartItemId,
                PizzaId = cartPizza.Pizza.Id
            };

            cartPizza.CartPizzaId = SqlDataAccess.SaveNewRecord(insertCartPizzaSql, cartPizzaQueryParameters, connection, transaction);

            return cartPizza.CartItemId;
        }

        public static int AddPizzaToCart(CartPizzaModel cartPizza)
        {
            using (IDbConnection connection = new SqlConnection(SqlDataAccess.GetConnectiongString()))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        cartPizza.CartPizzaId = AddPizzaToCart(cartPizza, connection, transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            return cartPizza.CartPizzaId;
        }

        public static List<CartPizzaModel> LoadCartPizzas()
        {
            List<CartPizzaModel> cartPizzas = new List<CartPizzaModel>();
            List<PizzaModel> pizzas = DatabasePizzaProcessor.LoadPizzas();

            string selectQuerySql = @"select CartItem.Id as CartItemId, CartItem.CartId, CartItem.PricePerItem, CartItem.Quantity, CartPizza.Id as CartPizzaId, CartPizza.CartItemId, CartPizza.PizzaId
                                      from dbo.CartItem right join CartPizza on CartItem.Id=CartPizza.CartItemId;";
            List<dynamic> queryList = SqlDataAccess.LoadData<dynamic>(selectQuerySql).ToList();

            foreach (var item in queryList)
            {
                cartPizzas.Add(new CartPizzaModel()
                {
                    CartId = item.CartId,
                    CartItemId = item.CartItemId,
                    PricePerItem = item.PricePerItem,
                    Quantity = item.Quantity,
                    CartPizzaId = item.CartPizzaId,
                    Pizza = pizzas.Where(p => p.Id == item.PizzaId).First()
                });
            }

            return cartPizzas;
        }

        public static int UpdateCartPizza(CartPizzaModel cartPizza)
        {
            int cartItemRowsAffected = 0;

            using (IDbConnection connection = new SqlConnection(SqlDataAccess.GetConnectiongString()))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Update pizza records
                        int pizzaRowsAffected = DatabasePizzaProcessor.UpdatePizza(cartPizza.Pizza, connection, transaction);

                        // Update cart item record
                        cartItemRowsAffected = DatabaseCartProcessor.UpdateCartItem(cartPizza, connection, transaction);

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

        internal static int DeleteCartPizza(CartPizzaModel cartPizza, IDbConnection connection, IDbTransaction transaction)
        {
            // Delete cart pizza record
            string deleteCartPizzaSql = @"delete from dbo.CartPizza where Id = @CartPizzaId;";
            int cartPizzaRowsDeleted = SqlDataAccess.DeleteRecord(deleteCartPizzaSql, cartPizza, connection, transaction);

            // Delete cart item record
            int cartItemRowsDeleted = DatabaseCartProcessor.DeleteCartItem(cartPizza, connection, transaction);

            // Delete pizza record
            int pizzaRecordRowsDeleted = DatabasePizzaProcessor.DeletePizza(cartPizza.Pizza, connection, transaction);

            return cartPizzaRowsDeleted;
        }

        public static int DeleteCartPizza(CartPizzaModel cartPizza)
        {
            int cartPizzaRowsDeleted = 0;

            using (IDbConnection connection = new SqlConnection(SqlDataAccess.GetConnectiongString()))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        cartPizzaRowsDeleted = DeleteCartPizza(cartPizza, connection, transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            return cartPizzaRowsDeleted;
        }
    }
}
