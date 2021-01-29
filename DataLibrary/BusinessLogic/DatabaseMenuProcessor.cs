using DataLibrary.DataAccess;
using DataLibrary.Models.Menu;
using System;
using System.Collections.Generic;
using Dapper;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DataLibrary.BusinessLogic
{
    public static class DatabaseMenuProcessor
    {
        public static int CreateMenuDessert(
            bool availableForPurchase,
            string name,
            bool hasMenuIcon,
            string menuIconFile,
            decimal price,
            string description,
            string itemDetails)
        {
            using (IDbConnection cnn = new SqlConnection(SqlDataAccess.GetConnectiongString()))
            {
                // Return id of new menu dessert.
                MenuDessertModel data = new MenuDessertModel
                {
                    AvailableForPurchase = availableForPurchase,
                    Name = name,
                    HasMenuIcon = hasMenuIcon,
                    MenuIconFile = menuIconFile,
                    Price = price,
                    Description = description,
                    ItemDetails = itemDetails
                };

                string sql = @"insert into dbo.MenuDessert (AvailableForPurchase, Name, HasMenuIcon, MenuIconFile, Price, Description, ItemDetails) output Inserted.Id values (@AvailableForPurchase, @Name, @HasMenuIcon, @MenuIconFile, @Price, @Description, @ItemDetails);";

                return SqlDataAccess.SaveData(sql, data);
            }
        }
    }
}
