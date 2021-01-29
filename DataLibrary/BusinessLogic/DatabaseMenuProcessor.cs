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
        public static List<MenuDessertModel> LoadMenuDesserts()
        {
            string sql = @"select Id, AvailableForPurchase, Name, HasMenuIcon, MenuIconFile, Price, Description, ItemDetails from dbo.MenuDessert;";

            return SqlDataAccess.LoadData<MenuDessertModel>(sql);
        }

        /// <summary>
        /// Updates a menu dessert record.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="availableForPurchase"></param>
        /// <param name="name"></param>
        /// <param name="hasMenuIcon"></param>
        /// <param name="menuIconFile"></param>
        /// <param name="price"></param>
        /// <param name="description"></param>
        /// <param name="itemDetails"></param>
        /// <returns>Rows affected.</returns>
        public static int UpdateMenuDessert(
               int id,
               bool availableForPurchase,
               string name,
               bool hasMenuIcon,
               string menuIconFile,
               decimal price,
               string description,
               string itemDetails)
        {
            MenuDessertModel data = LoadMenuDesserts().Where(i => i.Id == id).First();

            data.AvailableForPurchase = availableForPurchase;
            data.Name = name;
            data.HasMenuIcon = hasMenuIcon;
            data.MenuIconFile = menuIconFile;
            data.Price = price;
            data.Description = description;
            data.ItemDetails = itemDetails;

            string sql = @"update dbo.MenuDessert set AvailableForPurchase = @AvailableForPurchase, Name = @Name, HasMenuIcon = @HasMenuIcon, MenuIconFile = @MenuIconFile, Price = @Price, Description = @Description, ItemDetails = @ItemDetails where Id = @Id;";

            return SqlDataAccess.UpdateRecord(sql, data);
        }

        /// <summary>
        /// Adds a new menu dessert record.
        /// </summary>
        /// <param name="availableForPurchase"></param>
        /// <param name="name"></param>
        /// <param name="hasMenuIcon"></param>
        /// <param name="menuIconFile"></param>
        /// <param name="price"></param>
        /// <param name="description"></param>
        /// <param name="itemDetails"></param>
        /// <returns>ID of the newly added menu dessert.</returns>
        public static int AddMenuDessert(
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

                return SqlDataAccess.SaveNewRecord(sql, data);
            }
        }
    }
}
