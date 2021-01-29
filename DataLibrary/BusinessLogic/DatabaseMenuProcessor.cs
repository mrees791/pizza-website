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
        public static List<MenuWingsModel> LoadMenuWings()
        {
            string sql = @"select Id, AvailableForPurchase, Name, Price6Piece, Price12Piece, Price18Piece, Description, HasMenuIcon, MenuIconFile from dbo.MenuWings;";

            return SqlDataAccess.LoadData<MenuWingsModel>(sql);
        }

        public static int UpdateMenuWings(
            int id,
            bool availableForPurchase,
            string name,
            decimal price6Piece,
            decimal price12Piece,
            decimal price18Piece,
            string description,
            bool hasMenuIcon,
            string menuIconFile)
        {
            MenuWingsModel data = LoadMenuWings().Where(i => i.Id == id).First();

            data.AvailableForPurchase = availableForPurchase;
            data.Name = name;
            data.Price6Piece = price6Piece;
            data.Price12Piece = price12Piece;
            data.Price18Piece = price18Piece;
            data.Description = description;
            data.HasMenuIcon = hasMenuIcon;
            data.MenuIconFile = menuIconFile;

            string sql = @"update dbo.MenuWings set AvailableForPurchase = @AvailableForPurchase, Name = @Name, Price6Piece = @Price6Piece, Price12Piece = @Price12Piece,
                           Price18Piece = @Price18Piece, Description = @Description, HasMenuIcon = @HasMenuIcon, MenuIconFile = @MenuIconFile where Id = @Id;";

            return SqlDataAccess.UpdateRecord(sql, data);
        }

        public static int AddMenuWings(
            int id,
            bool availableForPurchase,
            string name,
            decimal price6Piece,
            decimal price12Piece,
            decimal price18Piece,
            string description,
            bool hasMenuIcon,
            string menuIconFile)
        {
            MenuWingsModel data = new MenuWingsModel();

            data.AvailableForPurchase = availableForPurchase;
            data.Name = name;
            data.Price6Piece = price6Piece;
            data.Price12Piece = price12Piece;
            data.Price18Piece = price18Piece;
            data.Description = description;
            data.HasMenuIcon = hasMenuIcon;
            data.MenuIconFile = menuIconFile;

            string sql = @"insert into dbo.MenuWings (AvailableForPurchase, Name, Price6Piece, Price12Piece, Price18Piece, Description, HasMenuIcon, MenuIconFile) output Inserted.Id
                           values (@AvailableForPurchase, @Name, @Price6Piece, @Price12Piece, @Price18Piece, @Description, @HasMenuIcon, @MenuIconFile);";

            return SqlDataAccess.SaveNewRecord(sql, data);
        }

        public static List<MenuSideModel> LoadMenuSides()
        {
            string sql = @"select Id, AvailableForPurchase, Name, Price, Description, ItemDetails, HasMenuIcon, MenuIconFile from dbo.MenuSide;";

            return SqlDataAccess.LoadData<MenuSideModel>(sql);
        }

        public static int UpdateMenuSide(
            int id,
            bool availableForPurchase,
            string name,
            decimal price,
            string description,
            string itemDetails,
            bool hasMenuIcon,
            string menuIconFile)
        {
            MenuSideModel data = LoadMenuSides().Where(i => i.Id == id).First();

            data.AvailableForPurchase = availableForPurchase;
            data.Name = name;
            data.Price = price;
            data.Description = description;
            data.ItemDetails = itemDetails;
            data.HasMenuIcon = hasMenuIcon;
            data.MenuIconFile = menuIconFile;

            string sql = @"update dbo.MenuSide set AvailableForPurchase = @AvailableForPurchase, Name = @Name, Price = @Price, Description = @Description, ItemDetails = @ItemDetails,
                           HasMenuIcon = @HasMenuIcon, MenuIconFile = @MenuIconFile where Id = @Id;";

            return SqlDataAccess.UpdateRecord(sql, data);
        }

        public static int AddMenuSide(
            bool availableForPurchase,
            string name,
            decimal price,
            string description,
            string itemDetails,
            bool hasMenuIcon,
            string menuIconFile)
        {
            MenuSideModel data = new MenuSideModel();

            data.AvailableForPurchase = availableForPurchase;
            data.Name = name;
            data.Price = price;
            data.Description = description;
            data.ItemDetails = itemDetails;
            data.HasMenuIcon = hasMenuIcon;
            data.MenuIconFile = menuIconFile;

            string sql = @"insert into dbo.MenuSide (AvailableForPurchase, Name, Price, Description, ItemDetails, HasMenuIcon, MenuIconFile) output Inserted.Id
                           values (@AvailableForPurchase, @Name, @Price, @Description, @ItemDetails, @HasMenuIcon, @MenuIconFile);";

            return SqlDataAccess.SaveNewRecord(sql, data);
        }

        public static List<MenuSauceModel> LoadMenuSauces()
        {
            string sql = @"select Id, AvailableForPurchase, Name, Price, Description, ItemDetails, HasMenuIcon, MenuIconFile from dbo.MenuSauce;";

            return SqlDataAccess.LoadData<MenuSauceModel>(sql);
        }

        public static int UpdateMenuSauce(
            int id,
            bool availableForPurchase,
            string name,
            decimal price,
            string description,
            string itemDetails,
            bool hasMenuIcon,
            string menuIconFile)
        {
            MenuSauceModel data = LoadMenuSauces().Where(i => i.Id == id).First();

            data.AvailableForPurchase = availableForPurchase;
            data.Name = name;
            data.Price = price;
            data.Description = description;
            data.ItemDetails = itemDetails;
            data.HasMenuIcon = hasMenuIcon;
            data.MenuIconFile = menuIconFile;

            string sql = @"update dbo.MenuSauce set AvailableForPurchase = @AvailableForPurchase, Name = @Name, Price = @Price, Description = @Description, ItemDetails = @ItemDetails,
                           HasMenuIcon = @HasMenuIcon, MenuIconFile = @MenuIconFile where Id = @Id;";

            return SqlDataAccess.UpdateRecord(sql, data);
        }

        public static int AddMenuSauce(
            bool availableForPurchase,
            string name,
            decimal price,
            string description,
            string itemDetails,
            bool hasMenuIcon,
            string menuIconFile)
        {
            MenuSauceModel data = new MenuSauceModel();

            data.AvailableForPurchase = availableForPurchase;
            data.Name = name;
            data.Price = price;
            data.Description = description;
            data.ItemDetails = itemDetails;
            data.HasMenuIcon = hasMenuIcon;
            data.MenuIconFile = menuIconFile;

            string sql = @"insert into dbo.MenuSauce (AvailableForPurchase, Name, Price, Description, ItemDetails, HasMenuIcon, MenuIconFile) output Inserted.Id
                           values (@AvailableForPurchase, @Name, @Price, @Description, @ItemDetails, @HasMenuIcon, @MenuIconFile);";

            return SqlDataAccess.SaveNewRecord(sql, data);
        }

        public static List<MenuPastaModel> LoadMenuPastas()
        {
            string sql = @"select Id, AvailableForPurchase, Name, Price, Description, ItemDetails, HasMenuIcon, MenuIconFile from dbo.MenuPasta;";

            return SqlDataAccess.LoadData<MenuPastaModel>(sql);
        }

        public static int UpdateMenuPasta(
            int id,
            bool availableForPurchase,
            string name,
            decimal price,
            string description,
            string itemDetails,
            bool hasMenuIcon,
            string menuIconFile)
        {
            MenuPastaModel data = LoadMenuPastas().Where(i => i.Id == id).First();

            data.AvailableForPurchase = availableForPurchase;
            data.Name = name;
            data.Price = price;
            data.Description = description;
            data.ItemDetails = itemDetails;
            data.HasMenuIcon = hasMenuIcon;
            data.MenuIconFile = menuIconFile;

            string sql = @"update dbo.MenuPasta set AvailableForPurchase = @AvailableForPurchase, Name = @Name, Price = @Price, Description = @Description, ItemDetails = @ItemDetails,
                           HasMenuIcon = @HasMenuIcon, MenuIconFile = @MenuIconFile where Id = @Id;";

            return SqlDataAccess.UpdateRecord(sql, data);
        }

        public static int AddMenuPasta(
            bool availableForPurchase,
            string name,
            decimal price,
            string description,
            string itemDetails,
            bool hasMenuIcon,
            string menuIconFile)
        {
            MenuPastaModel data = new MenuPastaModel();

            data.AvailableForPurchase = availableForPurchase;
            data.Name = name;
            data.Price = price;
            data.Description = description;
            data.ItemDetails = itemDetails;
            data.HasMenuIcon = hasMenuIcon;
            data.MenuIconFile = menuIconFile;

            string sql = @"insert into dbo.MenuPasta (AvailableForPurchase, Name, Price, Description, ItemDetails, HasMenuIcon, MenuIconFile) output Inserted.Id
                           values (@AvailableForPurchase, @Name, @Price, @Description, @ItemDetails, @HasMenuIcon, @MenuIconFile);";

            return SqlDataAccess.SaveNewRecord(sql, data);
        }

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
