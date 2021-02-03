using DataLibrary.DataAccess;
using DataLibrary.Models.Carts;
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
        internal static int AddNewCart(CartModel cart, IDbConnection connection, IDbTransaction transaction)
        {
            string insertSql = @"insert into dbo.Cart output Inserted.Id default values;";

            // Save user role record
            cart.Id = SqlDataAccess.SaveNewRecord(insertSql, cart, connection, transaction);

            return cart.Id;
        }
    }
}
