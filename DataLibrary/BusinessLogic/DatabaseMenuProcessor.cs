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
        public static List<MenuDrinkModel> LoadMenuDrinks()
        {
            string sql = @"select Id, AvailableForPurchase, Name, AvailableIn20Oz, AvailableIn2Liter, AvailableIn2Pack12Oz, AvailableIn6Pack12Oz, Price20Oz, Price2Liter, Price2Pack12Oz, Price6Pack12Oz, Description, HasMenuIcon, MenuIconFile from dbo.MenuDrink;";

            return SqlDataAccess.LoadData<MenuDrinkModel>(sql);
        }

        public static int UpdateMenuDrink(
            int id,
            bool availableForPurchase,
            string name,
            bool availableIn20Oz,
            bool availableIn2Liter,
            bool availableIn2Pack12Oz,
            bool availableIn6Pack12Oz,
            decimal price20Oz,
            decimal price2Liter,
            decimal price2Pack12Oz,
            decimal price6Pack12Oz,
            string description,
            bool hasMenuIcon,
            string menuIconFile)
        {
            MenuDrinkModel data = LoadMenuDrinks().Where(i => i.Id == id).First();

            data.AvailableForPurchase = availableForPurchase;
            data.Name = name;
            data.AvailableIn20Oz = availableIn20Oz;
            data.AvailableIn2Liter = availableIn2Liter;
            data.AvailableIn2Pack12Oz = availableIn2Pack12Oz;
            data.AvailableIn6Pack12Oz = availableIn6Pack12Oz;
            data.Price20Oz = price20Oz;
            data.Price2Liter = price2Liter;
            data.Price2Pack12Oz = price2Pack12Oz;
            data.Price6Pack12Oz = price6Pack12Oz;
            data.Description = description;
            data.HasMenuIcon = hasMenuIcon;
            data.MenuIconFile = menuIconFile;

            string sql = @"update dbo.MenuDip set AvailableForPurchase = @AvailableForPurchase, Name = @Name, AvailableIn20Oz = @AvailableIn20Oz, AvailableIn2Liter = @AvailableIn2Liter,
                           AvailableIn2Pack12Oz = @AvailableIn2Pack12Oz, AvailableIn6Pack12Oz = @AvailableIn6Pack12Oz,
                           Price20Oz = @Price20Oz, Price2Liter = @Price2Liter, Price2Pack12Oz = @Price2Pack12Oz, Price6Pack12Oz = @Price6Pack12Oz
                           Description = @Description, HasMenuIcon = @HasMenuIcon, MenuIconFile = @MenuIconFile where Id = @Id;";

            return SqlDataAccess.UpdateRecord(sql, data);
        }

        public static int AddMenuDrink(
            bool availableForPurchase,
            string name,
            bool availableIn20Oz,
            bool availableIn2Liter,
            bool availableIn2Pack12Oz,
            bool availableIn6Pack12Oz,
            decimal price20Oz,
            decimal price2Liter,
            decimal price2Pack12Oz,
            decimal price6Pack12Oz,
            string description,
            bool hasMenuIcon,
            string menuIconFile)
        {
            MenuDrinkModel data = new MenuDrinkModel();

            data.AvailableForPurchase = availableForPurchase;
            data.Name = name;
            data.AvailableIn20Oz = availableIn20Oz;
            data.AvailableIn2Liter = availableIn2Liter;
            data.AvailableIn2Pack12Oz = availableIn2Pack12Oz;
            data.AvailableIn6Pack12Oz = availableIn6Pack12Oz;
            data.Price20Oz = price20Oz;
            data.Price2Liter = price2Liter;
            data.Price2Pack12Oz = price2Pack12Oz;
            data.Price6Pack12Oz = price6Pack12Oz;
            data.Description = description;
            data.HasMenuIcon = hasMenuIcon;
            data.MenuIconFile = menuIconFile;

            string sql = @"insert into dbo.MenuDrink (AvailableForPurchase, Name, AvailableIn20Oz, AvailableIn2Liter, AvailableIn2Pack12Oz, AvailableIn6Pack12Oz,
                           Price20Oz, Price2Liter, Price2Pack12Oz, Price6Pack12Oz, Description, HasMenuIcon, MenuIconFile) output Inserted.Id
                           values (@AvailableForPurchase, @Name, @AvailableIn20Oz, @AvailableIn2Liter, @AvailableIn2Pack12Oz, @AvailableIn6Pack12Oz,
                           @Price20Oz, @Price2Liter, @Price2Pack12Oz, @Price6Pack12Oz, @Description, @HasMenuIcon, @MenuIconFile);";

            return SqlDataAccess.SaveNewRecord(sql, data);
        }

        public static List<MenuDipModel> LoadMenuDips()
        {
            string sql = @"select Id, AvailableForPurchase, Name, HasMenuIcon, MenuIconFile, Price, ItemDetails from dbo.MenuDip";

            return SqlDataAccess.LoadData<MenuDipModel>(sql);
        }

        public static int UpdateMenuDip(
            int id,
            bool availableForPurchase,
            string name,
            decimal price,
            string itemDetails,
            bool hasMenuIcon,
            string menuIconFile)
        {
            MenuDipModel data = LoadMenuDips().Where(i => i.Id == id).First();

            data.AvailableForPurchase = availableForPurchase;
            data.Name = name;
            data.Price = price;
            data.ItemDetails = itemDetails;
            data.HasMenuIcon = hasMenuIcon;
            data.MenuIconFile = menuIconFile;

            string sql = @"update dbo.MenuDip set AvailableForPurchase = @AvailableForPurchase, Name = @Name, HasMenuIcon = @HasMenuIcon, MenuIconFile = @MenuIconFile, Price = @Price, ItemDetails = @ItemDetails where Id = @Id;";

            return SqlDataAccess.UpdateRecord(sql, data);
        }

        public static int AddMenuDip(
            bool availableForPurchase,
            string name,
            decimal price,
            string itemDetails,
            bool hasMenuIcon,
            string menuIconFile)
        {
            MenuDipModel data = new MenuDipModel();

            data.AvailableForPurchase = availableForPurchase;
            data.Name = name;
            data.Price = price;
            data.ItemDetails = itemDetails;
            data.HasMenuIcon = hasMenuIcon;
            data.MenuIconFile = menuIconFile;

            string sql = @"insert into dbo.MenuDip (AvailableForPurchase, Name, HasMenuIcon, MenuIconFile, Price, ItemDetails) output Inserted.Id values (@AvailableForPurchase, @Name, @HasMenuIcon, @MenuIconFile, @Price, @ItemDetails);";

            return SqlDataAccess.SaveNewRecord(sql, data);
        }

        /// <summary>
        /// Loads all menu dessert records.
        /// </summary>
        /// <returns></returns>
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
            decimal price,
            string description,
            string itemDetails,
            bool hasMenuIcon,
            string menuIconFile)
        {
            MenuDessertModel data = LoadMenuDesserts().Where(i => i.Id == id).First();

            data.AvailableForPurchase = availableForPurchase;
            data.Name = name;
            data.Price = price;
            data.Description = description;
            data.ItemDetails = itemDetails;
            data.HasMenuIcon = hasMenuIcon;
            data.MenuIconFile = menuIconFile;

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
            decimal price,
            string description,
            string itemDetails,
            bool hasMenuIcon,
            string menuIconFile)
        {
            MenuDessertModel data = new MenuDessertModel();

            data.AvailableForPurchase = availableForPurchase;
            data.Name = name;
            data.Price = price;
            data.Description = description;
            data.ItemDetails = itemDetails;
            data.HasMenuIcon = hasMenuIcon;
            data.MenuIconFile = menuIconFile;

            string sql = @"insert into dbo.MenuDessert (AvailableForPurchase, Name, HasMenuIcon, MenuIconFile, Price, Description, ItemDetails) output Inserted.Id values (@AvailableForPurchase, @Name, @HasMenuIcon, @MenuIconFile, @Price, @Description, @ItemDetails);";

            return SqlDataAccess.SaveNewRecord(sql, data);
        }
    }
}
