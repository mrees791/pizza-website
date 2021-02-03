﻿using DataLibrary.DataAccess;
using DataLibrary.Models.Carts;
using DataLibrary.Models.Menu;
using DataLibrary.Models.Pizzas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic
{
    public static class DatabaseCartProcessor
    {
        public static int AddPizzaToCart(int cartId, PizzaModel pizzaModel, int quantity)
        {
            int cartPizzaId = 0;

            using (IDbConnection connection = new SqlConnection(SqlDataAccess.GetConnectiongString()))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        cartPizzaId = AddPizzaToCart(cartId, pizzaModel, quantity, connection, transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            return cartPizzaId;
        }

        internal static int AddPizzaToCart(int cartId, PizzaModel pizzaModel, int quantity, IDbConnection connection, IDbTransaction transaction)
        {
            string insertSql = @"insert into dbo.CartPizza (CartId, PizzaId, PricePerItem, Quantity) output Inserted.Id values(@CartId, @PizzaId, @PricePerItem, @Quantity);";

            // Save pizza record
            pizzaModel.Id = DatabasePizzaProcessor.AddPizza(pizzaModel, connection, transaction);

            object queryParameters = new 
            {
                CartId = cartId,
                PizzaId = pizzaModel.Id,
                PricePerItem = pizzaModel.GetPrice(),
                Quantity = quantity
            };

            // Save cart pizza record
            return SqlDataAccess.SaveNewRecord(insertSql, queryParameters, connection, transaction);
        }

        internal static int AddDessertToCart(int cartId, MenuDessertModel menuDessert, int quantity, IDbConnection connection, IDbTransaction transaction)
        {
            string insertSql = @"insert into dbo.CartDessert (CartId, MenuDessertId, PricePerItem, Quantity)
                                 output Inserted.Id values(@CartId, @MenuDessertId, @PricePerItem, @Quantity);";

            object queryParameters = new
            {
                CartId = cartId,
                MenuDessertId = menuDessert.Id,
                PricePerItem = menuDessert.Price,
                Quantity = quantity
            };

            // Save cart dessert record
            return SqlDataAccess.SaveNewRecord(insertSql, queryParameters, connection, transaction);
        }

        internal static int AddDrinkToCart(int cartId, MenuDrinkModel menuDrink, decimal pricePerItem, string size, int quantity, IDbConnection connection, IDbTransaction transaction)
        {
            string insertSql = @"insert into dbo.CartDrink (CartId, MenuDrinkId, PricePerItem, Size, Quantity)
                                 output Inserted.Id values(@CartId, @MenuDrinkId, @PricePerItem, @Size, @Quantity);";

            object queryParameters = new
            {
                CartId = cartId,
                MenuDrinkId = menuDrink.Id,
                PricePerItem = pricePerItem,
                Size = size,
                Quantity = quantity
            };

            // Save cart drink record
            return SqlDataAccess.SaveNewRecord(insertSql, queryParameters, connection, transaction);
        }

        internal static int AddNewCart(IDbConnection connection, IDbTransaction transaction)
        {
            string insertSql = @"insert into dbo.Cart output Inserted.Id default values;";

            // Save new cart record
            return SqlDataAccess.SaveNewRecord(insertSql, new { }, connection, transaction);
        }
    }
}
