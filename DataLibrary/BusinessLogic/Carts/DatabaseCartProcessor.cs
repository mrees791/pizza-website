using Dapper;
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
    public static class DatabaseCartProcessor
    {
        internal static int AddNewCart(IDbConnection connection, IDbTransaction transaction)
        {
            string insertCartSql = @"insert into dbo.Cart output Inserted.Id default values;";

            return SqlDataAccess.SaveNewRecord(insertCartSql, new { }, connection, transaction);
        }

        public static List<CartModel> LoadCarts()
        {
            List<CartModel> carts = new List<CartModel>();
            List<CartPizzaModel> cartPizzas = DatabaseCartPizzaProcessor.LoadCartPizzas();

            using (IDbConnection connection = new SqlConnection(SqlDataAccess.GetConnectiongString()))
            {
                string selectCartQuerySql = @"select Id from dbo.Cart;";
                carts = SqlDataAccess.LoadData<CartModel>(selectCartQuerySql);

                foreach (var cart in carts)
                {
                    cart.CartPizzas.AddRange(cartPizzas.Where(c => c.Id == cart.Id));
                }
            }

            return carts;
        }
    }
}
