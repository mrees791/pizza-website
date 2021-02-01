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
        private static int AddPizzaTopping(PizzaToppingModel databaseModel)
        {
            string sql = @"insert into dbo.PizzaTopping (PizzaId, ToppingHalf, MenuPizzaToppingId) output Inserted.Id
                           values (@PizzaId, @ToppingHalf, @MenuPizzaToppingId);";

            return SqlDataAccess.SaveNewRecord(sql, databaseModel);
        }

        /*public static int AddPizza(
            string size,
            MenuPizzaCrustModel menuPizzaCrust,
            MenuPizzaSauceModel menuPizzaSauce,
            string sauceAmount,
            MenuPizzaCheeseModel menuPizzaCheese,
            string cheeseAmount,
            MenuPizzaCrustFlavorModel menuPizzaCrustFlavor,
            List<PizzaToppingModel> pizzaToppings)*/
        /*public static int AddPizza(
            string size,
            int menuPizzaCrustId,
            int menuPizzaSauceId,
            string sauceAmount,
            int menuPizzaCheeseId,
            string cheeseAmount,
            int menuPizzaCrustFlavorId,
            List<PizzaToppingModel> pizzaToppings)
        {

            /*PizzaModel data = PizzaFactory.CreatePizza(0, size, menuPizzaCrust, menuPizzaSauce, sauceAmount, menuPizzaCheese, cheeseAmount, menuPizzaCrustFlavor);

            // WILL REQUIRE TRANSACTION

            // INSERT NEW PIZZA RECORD
            // INSERT PIZZA TOPPINGS RECORDS

            string sql = @"insert into dbo.Pizza (AvailableForPurchase, Name, Description) output Inserted.Id
                           values (@AvailableForPurchase, @Name, @Description);";


            using (IDbConnection cnn = new SqlConnection(SqlDataAccess.GetConnectiongString()))
            {
                cnn.Open();

                using (var transaction = cnn.BeginTransaction())
                {
                    try
                    {

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        throw;
                    }
                }

                //return cnn.Query<T>(sql).ToList();
            }

            // Insert 
            string sql = @"insert into dbo.MenuWingsSauce (AvailableForPurchase, Name, Description) output Inserted.Id
                           values (@AvailableForPurchase, @Name, @Description);";

            //return SqlDataAccess.SaveNewRecord(sql, data);

            throw new NotImplementedException();
    }*/

        public static int UpdatePizza()
        {
            // WILL REQUIRE MULTIPLE QUERIES (TRANSACTION NEEDED)

            // Remove previous pizza toppings.

            throw new NotImplementedException();
        }
    }
}
