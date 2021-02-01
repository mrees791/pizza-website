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
        private static int DeletePizzaTopping(IDbConnection connection, IDbTransaction transaction, PizzaToppingModel pizzaToppingModel)
        {
            string sql = $"delete from dbo.PizzaTopping where Id = @Id";

            return SqlDataAccess.DeleteRecord(connection, transaction, sql, pizzaToppingModel);
        }

        private static int AddPizzaTopping(IDbConnection connection, IDbTransaction transaction, PizzaToppingModel toppingModel, PizzaModel pizzaModel)
        {
            string insertSql = $"insert into dbo.PizzaTopping (PizzaId, ToppingHalf, MenuPizzaToppingId) output Inserted.Id values (@PizzaId, @ToppingHalf, @MenuPizzaToppingId);";

            toppingModel.Id = connection.Query<int>(
                insertSql,
                new
                {
                    PizzaId = pizzaModel.Id,
                    ToppingHalf = toppingModel.ToppingHalf,
                    MenuPizzaToppingId = toppingModel.MenuPizzaTopping.Id
                },
                transaction).Single();

            return toppingModel.Id;
        }

        private static List<PizzaToppingModel> LoadPizzaToppings()
        {
            string sql = @"select Id, PizzaId, ToppingHalf, MenuPizzaToppingId from dbo.PizzaTopping;";

            return SqlDataAccess.LoadData<PizzaToppingModel>(sql);
        }

        public static List<PizzaModel> LoadPizzas()
        {
            string sql = @"select Id, Size, MenuPizzaCrustId, MenuPizzaSauceId, SauceAmount, MenuPizzaCheeseId, CheeseAmount, MenuPizzaCrustFlavorId from dbo.Pizza;";

            return SqlDataAccess.LoadData<PizzaModel>(sql);
        }

        public static int UpdatePizza(PizzaModel pizzaModel)
        {
            throw new NotImplementedException();
        }

        public static int AddPizza(PizzaModel pizzaModel)
        {
            string pizzaSql = @"insert into dbo.Pizza (Size, MenuPizzaCrustId, MenuPizzaSauceId, SauceAmount, MenuPizzaCheeseId, CheeseAmount, MenuPizzaCrustFlavorId) output Inserted.Id
                                values (@Size, @MenuPizzaCrustId, @MenuPizzaSauceId, @SauceAmount, @MenuPizzaCheeseId, @CheeseAmount, @MenuPizzaCrustFlavorId);";

            using (IDbConnection connection = new SqlConnection(SqlDataAccess.GetConnectiongString()))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Save pizza record
                        pizzaModel.Id = connection.Query<int>(
                            pizzaSql,
                            new
                            {
                                Size = pizzaModel.Size,
                                MenuPizzaCrustId = pizzaModel.MenuPizzaCrust.Id,
                                MenuPizzaSauceId = pizzaModel.MenuPizzaSauce.Id,
                                SauceAmount = pizzaModel.SauceAmount,
                                MenuPizzaCheeseId = pizzaModel.MenuPizzaCheese.Id,
                                CheeseAmount = pizzaModel.CheeseAmount,
                                MenuPizzaCrustFlavorId = pizzaModel.MenuPizzaCrust.Id
                            },
                            transaction).Single();

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
