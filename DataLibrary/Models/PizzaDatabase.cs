using Dapper;
using DataLibrary.Models.Tables;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    public class PizzaDatabase : IDisposable
    {
        private IDbConnection connection;

        public PizzaDatabase(string connectionName = "PizzaDatabase")
        {
            string connectionString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
            connection = new SqlConnection(connectionString);
            connection.Open();
        }

        // todo: Remove
        public void Test()
        {
            /*Cart myCart = new Cart();
            connection.Insert<int,Cart>(myCart);

            Cart cart1 = connection.Get<Cart>(1);
            Cart cart2 = connection.Get<Cart>(2);
            Cart cart3 = connection.Get<Cart>(3);

            var carts = connection.GetList<Cart>();*/

            // RAN INSIDE TRANSACTION
            using (var transaction = connection.BeginTransaction())
            {
                int currentCartId = CreateNewCart(connection, transaction);
                int confirmOrderCartId = CreateNewCart(connection, transaction);

                SiteUser user = new SiteUser()
                {
                    CurrentCartId = currentCartId,
                    ConfirmOrderCartId = confirmOrderCartId,
                    UserName = "TestUser",
                    LockoutEndDateUtc = DateTime.UtcNow
                };

                int? userId = connection.Insert(user, transaction);

                transaction.Commit();
            }
            /*var testCart = new Cart();
            int cartId = connection.Execute("INSERT INTO Cart DEFAULT VALUES;");*/

            var carts = connection.GetList<Cart>();
            var users = connection.GetList<SiteUser>();
        }

        private int CreateNewCart(IDbConnection connection, IDbTransaction transaction = null)
        {
            return connection.Query<int>("INSERT INTO Cart OUTPUT Inserted.Id DEFAULT VALUES;", null, transaction).Single();
        }

        public void Dispose()
        {
            connection.Close();
            connection.Dispose();
        }
    }
}
