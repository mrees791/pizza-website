using Dapper;
using DataLibrary.DataAccess;
using DataLibrary.Models.Menu.Pizzas;
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
    public static class DatabasePizzaProcessor
    {
        private static int AddPizzaTopping(IDbConnection connection, IDbTransaction transaction, PizzaToppingModel toppingModel, PizzaModel pizzaModel)
        {
            string sql = $"insert into dbo.PizzaTopping (PizzaId, ToppingHalf, MenuPizzaToppingId) output Inserted.Id values ({pizzaModel.Id}, @ToppingHalf, {toppingModel.MenuPizzaTopping.Id});";

            return SqlDataAccess.SaveNewRecord(connection, transaction, sql, toppingModel);
        }

        public static int AddPizza(PizzaModel pizzaModel)
        {
            string pizzaSql = $"insert into dbo.Pizza (Size, MenuPizzaCrustId, MenuPizzaSauceId, SauceAmount, MenuPizzaCheeseId, CheeseAmount, MenuPizzaCrustFlavorId) output Inserted.Id values (@Size, {pizzaModel.MenuPizzaCrust.Id}, {pizzaModel.MenuPizzaSauce.Id}, @SauceAmount, {pizzaModel.MenuPizzaCheese.Id}, @CheeseAmount, {pizzaModel.MenuPizzaCrustFlavor.Id});";

            using (IDbConnection connection = new SqlConnection(SqlDataAccess.GetConnectiongString()))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Save pizza record
                        pizzaModel.Id = SqlDataAccess.SaveNewRecord(connection, transaction, pizzaSql, pizzaModel);

                        // Save pizza topping records
                        foreach (var pizzaTopping in pizzaModel.PizzaToppings)
                        {
                            AddPizzaTopping(connection, transaction, pizzaTopping, pizzaModel);
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

                return pizzaModel.Id;
            }
        }
    }
}
