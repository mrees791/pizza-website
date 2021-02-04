using DataLibrary.BusinessLogic.Pizzas;
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
    public static class DatabaseCartPizzaProcessor
    {
        internal static int AddPizzaToCart(CartPizzaModel cartPizza, IDbConnection connection, IDbTransaction transaction)
        {
            // Save pizza record
            cartPizza.Pizza.Id = DatabasePizzaProcessor.AddPizza(cartPizza.Pizza, connection, transaction);

            // Save cart pizza record
            string insertCartPizzaSql = @"insert into dbo.CartPizza (CartId, PizzaId, PricePerItem, Quantity, DateAddedToCart) output Inserted.Id
                                          values(@CartId, @PizzaId, @PricePerItem, @Quantity, @DateAddedToCart);";

            object queryParameters = new
            {
                CartId = cartPizza.CartId,
                PizzaId = cartPizza.Pizza.Id,
                PricePerItem = cartPizza.Pizza.GetPrice(),
                Quantity = cartPizza.Quantity,
                DateAddedToCart = DateTime.Now
            };

            cartPizza.Id = SqlDataAccess.SaveNewRecord(insertCartPizzaSql, queryParameters, connection, transaction);

            return cartPizza.Id;
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
                        cartPizza.Id = AddPizzaToCart(cartPizza, connection, transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            return cartPizza.Id;
        }

        public static int UpdateCartPizza(CartPizzaModel cartPizza)
        {
            int cartPizzaRowsAffected = 0;

            using (IDbConnection connection = new SqlConnection(SqlDataAccess.GetConnectiongString()))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Update pizza records
                        int pizzaRowsAffected = DatabasePizzaProcessor.UpdatePizza(cartPizza.Pizza, connection, transaction);

                        // Update cart pizza record
                        string updateSql = @"update dbo.CartPizza set PricePerItem = @PricePerItem, Quantity = @Quantity where Id = @Id;";

                        object queryParameters = new
                        {
                            Id = cartPizza.Id,
                            PricePerItem = cartPizza.Pizza.GetPrice(),
                            Quantity = cartPizza.Quantity
                        };

                        cartPizzaRowsAffected = SqlDataAccess.UpdateRecord(updateSql, queryParameters, connection, transaction);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            return cartPizzaRowsAffected;
        }

        public static int UpdateCartPizzaQuantity(CartPizzaModel cartPizza)
        {
            string updateSql = @"update dbo.CartPizza set Quantity = @Quantity where Id = @Id;";

            object queryParameters = new
            {
                Id = cartPizza.Id,
                Quantity = cartPizza.Quantity
            };

            return SqlDataAccess.UpdateRecord(updateSql, queryParameters);
        }

        internal static int DeleteCartPizza(CartPizzaModel cartPizza, IDbConnection connection, IDbTransaction transaction)
        {
            // Delete cart pizza record
            string deleteCartPizzaSql = @"delete from dbo.CartPizza where Id = @Id;";
            int cartPizzaRowsDeleted = SqlDataAccess.DeleteRecord(deleteCartPizzaSql, cartPizza, connection, transaction);

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
