﻿using Dapper;
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
            string insertCartItemSql = @"insert into dbo.CartItem (CartId, PricePerItem, Quantity) output Inserted.Id
                                         values(@CartId, @PricePerItem, @Quantity);";

            object cartItemQueryParameters = new
            {
                CartId = cartPizza.CartId,
                PricePerItem = cartPizza.PricePerItem,
                Quantity = cartPizza.Quantity
            };

            cartPizza.Id = SqlDataAccess.SaveNewRecord(insertCartItemSql, cartItemQueryParameters, connection, transaction);

            // Save cart pizza record
            string insertCartPizzaSql = @"insert into dbo.CartPizza (CartItemId, PizzaId) output Inserted.Id
                                          values(@CartItemId, @PizzaId);";

            object cartPizzaQueryParameters = new
            {
                CartItemId = cartPizza.Id,
                PizzaId = cartPizza.Pizza.Id
            };

            int cartPizzaId = SqlDataAccess.SaveNewRecord(insertCartPizzaSql, cartPizzaQueryParameters, connection, transaction);

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

        public static List<CartPizzaModel> LoadCartPizzas()
        {
            List<CartPizzaModel> cartPizzas = new List<CartPizzaModel>();
            List<PizzaModel> pizzas = DatabasePizzaProcessor.LoadPizzas();

            string selectQuerySql = @"select CartItem.Id, CartItem.CartId, CartItem.PricePerItem, CartItem.Quantity, CartPizza.CartItemId, CartPizza.PizzaId
                                      from dbo.CartItem inner join CartPizza on CartItem.Id=CartPizza.CartItemId;";
            List<dynamic> queryList = SqlDataAccess.LoadData<dynamic>(selectQuerySql).ToList();

            foreach (var item in queryList)
            {
                cartPizzas.Add(new CartPizzaModel()
                {
                    Id = item.Id,
                    CartId = item.CartId,
                    CartItemId = item.CartItemId,
                    PricePerItem = item.PricePerItem,
                    Quantity = item.Quantity,
                    Pizza = pizzas.Where(p => p.Id == item.PizzaId).First()
                });
            }

            return cartPizzas;
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

                        // Update cart item record
                        string updateSql = @"update dbo.CartItem set PricePerItem = @PricePerItem, Quantity = @Quantity where Id = @Id;";

                        object queryParameters = new
                        {
                            Id = cartPizza.CartItemId,
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

        /*internal static int DeleteCartPizza(CartPizzaModel cartPizza, IDbConnection connection, IDbTransaction transaction)
        {
            // Delete cart pizza record
            string deleteCartPizzaSql = @"delete from dbo.CartPizza where Id = @Id;";
            int cartPizzaRowsDeleted = SqlDataAccess.DeleteRecord(deleteCartPizzaSql, cartPizza, connection, transaction);

            // Delete pizza record
            int pizzaRecordRowsDeleted = DatabasePizzaProcessor.DeletePizza(cartPizza.Pizza, connection, transaction);

            return cartPizzaRowsDeleted;
        }*/

        /*


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
        }*/
    }
}
