using DataLibrary.DataAccess;
using DataLibrary.Models.Carts;
using System;
using System.Collections.Generic;
using System.Data;
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
    }
}
