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
        internal static void DeletePizza(PizzaModel pizzaModel, IDbConnection connection, IDbTransaction transaction)
        {
            string deletePizzaSql = $"delete from dbo.Pizza where Id = @Id;";

            // Delete pizza topping records
            DeletePizzaToppings(pizzaModel, connection, transaction);

            // Delete pizza record
            int rowsDeletedPizza = SqlDataAccess.DeleteRecord(deletePizzaSql, pizzaModel, connection, transaction);

            if (rowsDeletedPizza == 0)
            {
                throw new Exception($"Unable to delete pizza with ID: {pizzaModel.Id}");
            }
        }

        public static void DeletePizza(PizzaModel pizzaModel)
        {
            using (IDbConnection connection = new SqlConnection(SqlDataAccess.GetConnectiongString()))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        DeletePizza(pizzaModel, connection, transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }


        private static int DeletePizzaToppings(PizzaModel pizzaModel, IDbConnection connection, IDbTransaction transaction)
        {
            string sql = $"delete from dbo.PizzaTopping where PizzaTopping.PizzaId = @Id;";

            return SqlDataAccess.DeleteRecord(sql, pizzaModel, connection, transaction);
        }

        private static int AddPizzaTopping(PizzaToppingModel toppingModel, PizzaModel pizzaModel, IDbConnection connection, IDbTransaction transaction)
        {
            string insertSql = $"insert into dbo.PizzaTopping (PizzaId, ToppingHalf, ToppingAmount, MenuPizzaToppingId) output Inserted.Id values (@PizzaId, @ToppingHalf, @ToppingAmount, @MenuPizzaToppingId);";

            toppingModel.Id = SqlDataAccess.SaveNewRecord(insertSql,
                new
                {
                    PizzaId = pizzaModel.Id,
                    ToppingHalf = toppingModel.ToppingHalf,
                    ToppingAmount = toppingModel.ToppingAmount,
                    MenuPizzaToppingId = toppingModel.MenuPizzaTopping.Id
                },
                connection, transaction);

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

        internal static int UpdatePizza(PizzaModel pizzaModel, IDbConnection connection, IDbTransaction transaction)
        {
            string pizzaSql = @"update dbo.Pizza set Size = @Size, MenuPizzaCrustId = @MenuPizzaCrustId, MenuPizzaSauceId = @MenuPizzaSauceId, 
                                SauceAmount = @SauceAmount, MenuPizzaCheeseId = @MenuPizzaCheeseId, CheeseAmount = @CheeseAmount, MenuPizzaCrustFlavorId = @MenuPizzaCrustFlavorId where Id = @Id;";

            // Delete previous pizza topping records
            DeletePizzaToppings(pizzaModel, connection, transaction);

            // Update pizza record
            int rowsAffectedPizza = SqlDataAccess.UpdateRecord(pizzaSql,
                new
                {
                    Id = pizzaModel.Id,
                    Size = pizzaModel.Size,
                    MenuPizzaCrustId = pizzaModel.MenuPizzaCrust.Id,
                    MenuPizzaSauceId = pizzaModel.MenuPizzaSauce.Id,
                    SauceAmount = pizzaModel.SauceAmount,
                    MenuPizzaCheeseId = pizzaModel.MenuPizzaCheese.Id,
                    CheeseAmount = pizzaModel.CheeseAmount,
                    MenuPizzaCrustFlavorId = pizzaModel.MenuPizzaCrustFlavor.Id
                },
                connection, transaction);

            if (rowsAffectedPizza == 0)
            {
                throw new Exception($"Unable to update pizza with ID: {pizzaModel.Id}");
            }

            // Save new pizza topping records
            foreach (var pizzaTopping in pizzaModel.PizzaToppings)
            {
                AddPizzaTopping(pizzaTopping, pizzaModel, connection, transaction);
            }

            return rowsAffectedPizza;
        }

        public static void UpdatePizza(PizzaModel pizzaModel)
        {
            using (IDbConnection connection = new SqlConnection(SqlDataAccess.GetConnectiongString()))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        UpdatePizza(pizzaModel, connection, transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        internal static int AddPizza(PizzaModel pizzaModel, IDbConnection connection, IDbTransaction transaction)
        {
            string pizzaSql = @"insert into dbo.Pizza (Size, MenuPizzaCrustId, MenuPizzaSauceId, SauceAmount, MenuPizzaCheeseId, CheeseAmount, MenuPizzaCrustFlavorId) output Inserted.Id
                                values (@Size, @MenuPizzaCrustId, @MenuPizzaSauceId, @SauceAmount, @MenuPizzaCheeseId, @CheeseAmount, @MenuPizzaCrustFlavorId);";

            // Save pizza record
            pizzaModel.Id = SqlDataAccess.SaveNewRecord(pizzaSql,
                new
                {
                    Size = pizzaModel.Size,
                    MenuPizzaCrustId = pizzaModel.MenuPizzaCrust.Id,
                    MenuPizzaSauceId = pizzaModel.MenuPizzaSauce.Id,
                    SauceAmount = pizzaModel.SauceAmount,
                    MenuPizzaCheeseId = pizzaModel.MenuPizzaCheese.Id,
                    CheeseAmount = pizzaModel.CheeseAmount,
                    MenuPizzaCrustFlavorId = pizzaModel.MenuPizzaCrustFlavor.Id
                },
                connection, transaction);

            // Save pizza topping records
            foreach (var pizzaTopping in pizzaModel.PizzaToppings)
            {
                AddPizzaTopping(pizzaTopping, pizzaModel, connection, transaction);
            }

            return pizzaModel.Id;
        }

        /// <summary>
        /// Adds a new pizza record and pizza topping records.
        /// </summary>
        /// <param name="pizzaModel"></param>
        /// <returns>ID of newly added pizza.</returns>
        public static int AddPizza(PizzaModel pizzaModel)
        {
            using (IDbConnection connection = new SqlConnection(SqlDataAccess.GetConnectiongString()))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        pizzaModel.Id = AddPizza(pizzaModel, connection, transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            return pizzaModel.Id;
        }
    }
}
