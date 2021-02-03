using DataLibrary.DataAccess;
using DataLibrary.Models.Carts;
using DataLibrary.Models.Menu;
using DataLibrary.Models.Pizzas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic
{
    public static class DatabaseCartProcessor
    {
        internal static int AddPizzaToCart(int cartId, PizzaModel pizzaModel, int quantity, IDbConnection connection, IDbTransaction transaction)
        {
            string insertSql = @"insert into dbo.CartPizza (CartId, PizzaId, PricePerItem, Quantity) output Inserted.Id values(@CartId, @MenuDessertId, @PricePerItem, @Quantity);";

            // Save pizza record
            int newPizzaId = DatabasePizzaProcessor.AddPizza(pizzaModel, connection, transaction);

            // Save cart pizza record
            return SqlDataAccess.SaveNewRecord(insertSql,
                new
                {
                    CartId = cartId,
                    PizzaId = newPizzaId,
                    PricePerItem = pizzaModel.GetPrice(),
                    Quantity = quantity
                }
                , connection, transaction);
        }

        internal static int AddDessertToCart(int cartId, int quantity, MenuDessertModel menuDessert, IDbConnection connection, IDbTransaction transaction)
        {
            string insertSql = @"insert into dbo.CartDessert (CartId, MenuDessertId, PricePerItem, Quantity)
                                 output Inserted.Id values(@CartId, @MenuDessertId, @PricePerItem, @Quantity);";

            // Save cart dessert record
            return SqlDataAccess.SaveNewRecord(insertSql,
                new
                {
                    CartId = cartId,
                    MenuDessertId = menuDessert.Id,
                    PricePerItem = menuDessert.Price,
                    Quantity = quantity
                }
                , connection, transaction);
        }

        internal static int AddNewCart(IDbConnection connection, IDbTransaction transaction)
        {
            string insertSql = @"insert into dbo.Cart output Inserted.Id default values;";

            // Save new cart record
            return SqlDataAccess.SaveNewRecord(insertSql, new { }, connection, transaction);
        }
    }
}
