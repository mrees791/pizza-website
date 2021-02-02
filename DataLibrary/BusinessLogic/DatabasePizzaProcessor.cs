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
            string insertSql = $"insert into dbo.PizzaTopping (PizzaId, ToppingHalf, ToppingAmount, MenuPizzaToppingId) output Inserted.Id values (@PizzaId, @ToppingHalf, @ToppingAmount, @MenuPizzaToppingId);";

            toppingModel.Id = connection.Query<int>(
                insertSql,
                new
                {
                    PizzaId = pizzaModel.Id,
                    ToppingHalf = toppingModel.ToppingHalf,
                    ToppingAmount = toppingModel.ToppingAmount,
                    MenuPizzaToppingId = toppingModel.MenuPizzaTopping.Id
                },
                transaction).Single();

            return toppingModel.Id;
        }

        public static List<PizzaToppingModel> LoadPizzaToppings()
        {
            string sql = @"select Id, PizzaId, ToppingHalf, ToppingAmount, MenuPizzaToppingId from dbo.PizzaTopping;";
            List<PizzaToppingModel> pizzaToppingList = new List<PizzaToppingModel>();
            List<MenuPizzaToppingModel> menuPizzaToppings = DatabaseMenuPizzaProcessor.LoadMenuPizzaToppings();

            using (IDbConnection connection = new SqlConnection(SqlDataAccess.GetConnectiongString()))
            {
                List<dynamic> queryList = connection.Query<dynamic>(sql).ToList();

                foreach (var item in queryList)
                {
                    pizzaToppingList.Add(new PizzaToppingModel()
                    {
                        Id = item.Id,
                        ToppingHalf = item.ToppingHalf,
                        ToppingAmount = item.ToppingAmount,
                        MenuPizzaTopping = menuPizzaToppings.Where(t => t.Id == item.MenuPizzaToppingId).First(),
                        PizzaId = item.PizzaId
                    });
                }
            }

            return pizzaToppingList;
        }

        public static List<PizzaModel> LoadPizzas()
        {
            string sql = @"select Id, Size, MenuPizzaCrustId, MenuPizzaSauceId, SauceAmount, MenuPizzaCheeseId, CheeseAmount, MenuPizzaCrustFlavorId from dbo.Pizza;";

            var pizzaList = new List<PizzaModel>();
            List<PizzaToppingModel> pizzaToppingList = LoadPizzaToppings();
            List<MenuPizzaCheeseModel> menuPizzaCheeseList = DatabaseMenuPizzaProcessor.LoadMenuPizzaCheeses();
            List<MenuPizzaCrustModel> menuPizzaCrustList = DatabaseMenuPizzaProcessor.LoadMenuPizzaCrusts();
            List<MenuPizzaCrustFlavorModel> menuPizzaCrustFlavorList = DatabaseMenuPizzaProcessor.LoadMenuPizzaCrustFlavors();
            List<MenuPizzaSauceModel> menuPizzaSauceList = DatabaseMenuPizzaProcessor.LoadMenuPizzaSauces();

            using (IDbConnection connection = new SqlConnection(SqlDataAccess.GetConnectiongString()))
            {
                List<dynamic> queryList = connection.Query<dynamic>(sql).ToList();

                foreach (var item in queryList)
                {
                    pizzaList.Add(new PizzaModel()
                    {
                        Id = item.Id,
                        CheeseAmount = item.CheeseAmount,
                        SauceAmount = item.SauceAmount,
                        Size = item.Size,
                        PizzaToppings = pizzaToppingList.Where(t => t.PizzaId == item.Id).ToList(),
                        MenuPizzaCheese = menuPizzaCheeseList.Where(c => c.Id == item.MenuPizzaCheeseId).First(),
                        MenuPizzaCrust = menuPizzaCrustList.Where(c => c.Id == item.MenuPizzaCrustId).First(),
                        MenuPizzaCrustFlavor = menuPizzaCrustFlavorList.Where(c => c.Id == item.MenuPizzaCrustFlavorId).First(),
                        MenuPizzaSauce = menuPizzaSauceList.Where(s => s.Id == item.MenuPizzaSauceId).First()
                    });
                }
            }

            return pizzaList;
        }

        public static int UpdatePizza(PizzaModel pizzaModel)
        {
            // Executed in a transaction

            // Remove old pizza topping records
            // Update pizza record
            // Add new pizza topping records

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
