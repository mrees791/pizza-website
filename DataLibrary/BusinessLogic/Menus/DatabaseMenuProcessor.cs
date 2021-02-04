using DataLibrary.DataAccess;
using DataLibrary.Models.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic.Menus
{
    public static class DatabaseMenuProcessor
    {
        public static List<MenuWingsSauceModel> LoadMenuWingsSauces()
        {
            string sql = @"select Id, AvailableForPurchase, Name, Description from dbo.MenuWingsSauce;";

            return SqlDataAccess.LoadData<MenuWingsSauceModel>(sql);
        }

        public static int UpdateMenuWingsSauce(MenuWingsSauceModel menuWingsSauceModel)
        {
            string sql = @"update dbo.MenuWingsSauce set AvailableForPurchase = @AvailableForPurchase, Name = @Name, Description = @Description where Id = @Id;";

            return SqlDataAccess.UpdateRecord(sql, menuWingsSauceModel);
        }

        public static int AddMenuWingsSauce(MenuWingsSauceModel menuWingsSauceModel)
        {
            string sql = @"insert into dbo.MenuWingsSauce (AvailableForPurchase, Name, Description) output Inserted.Id
                           values (@AvailableForPurchase, @Name, @Description);";

            return SqlDataAccess.SaveNewRecord(sql, menuWingsSauceModel);
        }

        public static List<MenuWingsModel> LoadMenuWings()
        {
            string sql = @"select Id, AvailableForPurchase, Name, Price6Piece, Price12Piece, Price18Piece, Description, HasMenuIcon, MenuIconFile from dbo.MenuWings;";

            return SqlDataAccess.LoadData<MenuWingsModel>(sql);
        }

        public static int UpdateMenuWings(MenuWingsModel menuWingsModel)
        {
            string sql = @"update dbo.MenuWings set AvailableForPurchase = @AvailableForPurchase, Name = @Name, Price6Piece = @Price6Piece, Price12Piece = @Price12Piece,
                           Price18Piece = @Price18Piece, Description = @Description, HasMenuIcon = @HasMenuIcon, MenuIconFile = @MenuIconFile where Id = @Id;";

            return SqlDataAccess.UpdateRecord(sql, menuWingsModel);
        }

        public static int AddMenuWings(MenuWingsModel menuWingsModel)
        {
            string sql = @"insert into dbo.MenuWings (AvailableForPurchase, Name, Price6Piece, Price12Piece, Price18Piece, Description, HasMenuIcon, MenuIconFile) output Inserted.Id
                           values (@AvailableForPurchase, @Name, @Price6Piece, @Price12Piece, @Price18Piece, @Description, @HasMenuIcon, @MenuIconFile);";

            return SqlDataAccess.SaveNewRecord(sql, menuWingsModel);
        }

        public static List<MenuSideModel> LoadMenuSides()
        {
            string sql = @"select Id, AvailableForPurchase, Name, Price, Description, ItemDetails, HasMenuIcon, MenuIconFile from dbo.MenuSide;";

            return SqlDataAccess.LoadData<MenuSideModel>(sql);
        }

        public static int UpdateMenuSide(MenuSideModel menuSideModel)
        {
            string sql = @"update dbo.MenuSide set AvailableForPurchase = @AvailableForPurchase, Name = @Name, Price = @Price, Description = @Description, ItemDetails = @ItemDetails,
                           HasMenuIcon = @HasMenuIcon, MenuIconFile = @MenuIconFile where Id = @Id;";

            return SqlDataAccess.UpdateRecord(sql, menuSideModel);
        }

        public static int AddMenuSide(MenuSideModel menuSideModel)
        {
            string sql = @"insert into dbo.MenuSide (AvailableForPurchase, Name, Price, Description, ItemDetails, HasMenuIcon, MenuIconFile) output Inserted.Id
                           values (@AvailableForPurchase, @Name, @Price, @Description, @ItemDetails, @HasMenuIcon, @MenuIconFile);";

            return SqlDataAccess.SaveNewRecord(sql, menuSideModel);
        }

        public static List<MenuSauceModel> LoadMenuSauces()
        {
            string sql = @"select Id, AvailableForPurchase, Name, Price, Description, ItemDetails, HasMenuIcon, MenuIconFile from dbo.MenuSauce;";

            return SqlDataAccess.LoadData<MenuSauceModel>(sql);
        }

        public static int UpdateMenuSauce(MenuSauceModel menuSauceModel)
        {
            string sql = @"update dbo.MenuSauce set AvailableForPurchase = @AvailableForPurchase, Name = @Name, Price = @Price, Description = @Description, ItemDetails = @ItemDetails,
                           HasMenuIcon = @HasMenuIcon, MenuIconFile = @MenuIconFile where Id = @Id;";

            return SqlDataAccess.UpdateRecord(sql, menuSauceModel);
        }

        public static int AddMenuSauce(MenuSauceModel menuSauceModel)
        {
            string sql = @"insert into dbo.MenuSauce (AvailableForPurchase, Name, Price, Description, ItemDetails, HasMenuIcon, MenuIconFile) output Inserted.Id
                           values (@AvailableForPurchase, @Name, @Price, @Description, @ItemDetails, @HasMenuIcon, @MenuIconFile);";

            return SqlDataAccess.SaveNewRecord(sql, menuSauceModel);
        }

        public static List<MenuPastaModel> LoadMenuPastas()
        {
            string sql = @"select Id, AvailableForPurchase, Name, Price, Description, ItemDetails, HasMenuIcon, MenuIconFile from dbo.MenuPasta;";

            return SqlDataAccess.LoadData<MenuPastaModel>(sql);
        }

        public static int UpdateMenuPasta(MenuPastaModel menuPastaModel)
        {
            string sql = @"update dbo.MenuPasta set AvailableForPurchase = @AvailableForPurchase, Name = @Name, Price = @Price, Description = @Description, ItemDetails = @ItemDetails,
                           HasMenuIcon = @HasMenuIcon, MenuIconFile = @MenuIconFile where Id = @Id;";

            return SqlDataAccess.UpdateRecord(sql, menuPastaModel);
        }

        public static int AddMenuPasta(MenuPastaModel menuPastaModel)
        {
            string sql = @"insert into dbo.MenuPasta (AvailableForPurchase, Name, Price, Description, ItemDetails, HasMenuIcon, MenuIconFile) output Inserted.Id
                           values (@AvailableForPurchase, @Name, @Price, @Description, @ItemDetails, @HasMenuIcon, @MenuIconFile);";

            return SqlDataAccess.SaveNewRecord(sql, menuPastaModel);
        }

        public static List<MenuDrinkModel> LoadMenuDrinks()
        {
            string sql = @"select Id, AvailableForPurchase, Name, AvailableIn20Oz, AvailableIn2Liter, AvailableIn2Pack12Oz, AvailableIn6Pack12Oz, Price20Oz, Price2Liter, Price2Pack12Oz, Price6Pack12Oz, Description, HasMenuIcon, MenuIconFile from dbo.MenuDrink;";

            return SqlDataAccess.LoadData<MenuDrinkModel>(sql);
        }

        public static int UpdateMenuDrink(MenuDrinkModel menuDrinkModel)
        {
            string sql = @"update dbo.MenuDrink set AvailableForPurchase = @AvailableForPurchase, Name = @Name, AvailableIn20Oz = @AvailableIn20Oz, AvailableIn2Liter = @AvailableIn2Liter,
                           AvailableIn2Pack12Oz = @AvailableIn2Pack12Oz, AvailableIn6Pack12Oz = @AvailableIn6Pack12Oz,
                           Price20Oz = @Price20Oz, Price2Liter = @Price2Liter, Price2Pack12Oz = @Price2Pack12Oz, Price6Pack12Oz = @Price6Pack12Oz,
                           Description = @Description, HasMenuIcon = @HasMenuIcon, MenuIconFile = @MenuIconFile where Id = @Id;";

            return SqlDataAccess.UpdateRecord(sql, menuDrinkModel);
        }

        public static int AddMenuDrink(MenuDrinkModel menuDrinkModel)
        {
            string sql = @"insert into dbo.MenuDrink (AvailableForPurchase, Name, AvailableIn20Oz, AvailableIn2Liter, AvailableIn2Pack12Oz, AvailableIn6Pack12Oz,
                           Price20Oz, Price2Liter, Price2Pack12Oz, Price6Pack12Oz, Description, HasMenuIcon, MenuIconFile) output Inserted.Id
                           values (@AvailableForPurchase, @Name, @AvailableIn20Oz, @AvailableIn2Liter, @AvailableIn2Pack12Oz, @AvailableIn6Pack12Oz,
                           @Price20Oz, @Price2Liter, @Price2Pack12Oz, @Price6Pack12Oz, @Description, @HasMenuIcon, @MenuIconFile);";

            return SqlDataAccess.SaveNewRecord(sql, menuDrinkModel);
        }

        public static List<MenuDipModel> LoadMenuDips()
        {
            string sql = @"select Id, AvailableForPurchase, Name, HasMenuIcon, MenuIconFile, Price, ItemDetails from dbo.MenuDip";

            return SqlDataAccess.LoadData<MenuDipModel>(sql);
        }

        public static int UpdateMenuDip(MenuDipModel menuDipModel)
        {
            string sql = @"update dbo.MenuDip set AvailableForPurchase = @AvailableForPurchase, Name = @Name, HasMenuIcon = @HasMenuIcon, MenuIconFile = @MenuIconFile, Price = @Price, ItemDetails = @ItemDetails where Id = @Id;";

            return SqlDataAccess.UpdateRecord(sql, menuDipModel);
        }

        public static int AddMenuDip(MenuDipModel menuDipModel)
        {
            string sql = @"insert into dbo.MenuDip (AvailableForPurchase, Name, HasMenuIcon, MenuIconFile, Price, ItemDetails) output Inserted.Id values (@AvailableForPurchase, @Name, @HasMenuIcon, @MenuIconFile, @Price, @ItemDetails);";

            return SqlDataAccess.SaveNewRecord(sql, menuDipModel);
        }

        public static List<MenuDessertModel> LoadMenuDesserts()
        {
            string sql = @"select Id, AvailableForPurchase, Name, HasMenuIcon, MenuIconFile, Price, Description, ItemDetails from dbo.MenuDessert;";

            return SqlDataAccess.LoadData<MenuDessertModel>(sql);
        }

        public static int UpdateMenuDessert(MenuDessertModel menuDessertModel)
        {
            string sql = @"update dbo.MenuDessert set AvailableForPurchase = @AvailableForPurchase, Name = @Name, HasMenuIcon = @HasMenuIcon, MenuIconFile = @MenuIconFile, Price = @Price, Description = @Description, ItemDetails = @ItemDetails where Id = @Id;";

            return SqlDataAccess.UpdateRecord(sql, menuDessertModel);
        }

        public static int AddMenuDessert(MenuDessertModel menuDessertModel)
        {
            string sql = @"insert into dbo.MenuDessert (AvailableForPurchase, Name, HasMenuIcon, MenuIconFile, Price, Description, ItemDetails) output Inserted.Id values (@AvailableForPurchase, @Name, @HasMenuIcon, @MenuIconFile, @Price, @Description, @ItemDetails);";

            return SqlDataAccess.SaveNewRecord(sql, menuDessertModel);
        }
    }
}