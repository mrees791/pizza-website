using DataLibrary.DataAccess;
using DataLibrary.Models.Menu.Pizzas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic
{
    public static class DatabaseMenuPizzaProcessor
    {
        public static List<MenuPizzaToppingModel> LoadMenuPizzaToppings()
        {
            string sql = @"select Id, AvailableForPurchase, Name, PriceLight, PriceRegular, PriceExtra,
                           PizzaToppingType, Description, HasMenuIcon, MenuIconFile, HasPizzaBuilderImage, PizzaBuilderImageFile from dbo.MenuPizzaTopping;";

            return SqlDataAccess.LoadData<MenuPizzaToppingModel>(sql);
        }

        public static int UpdateMenuPizzaTopping(MenuPizzaToppingModel databaseModel)
        {
            string sql = @"update dbo.MenuPizzaTopping set AvailableForPurchase = @AvailableForPurchase, Name = @Name, PriceLight = @PriceLight,
                           PriceRegular = @PriceRegular, PriceExtra = @PriceExtra, PizzaToppingType = @PizzaToppingType, Description = @Description,
                           HasMenuIcon = @HasMenuIcon, MenuIconFile = @MenuIconFile, HasPizzaBuilderImage = @HasPizzaBuilderImage, PizzaBuilderImageFile = @PizzaBuilderImageFile where Id = @Id;";

            return SqlDataAccess.UpdateRecord(sql, databaseModel);
        }

        public static int AddMenuPizzaTopping(MenuPizzaToppingModel databaseModel)
        {
            string sql = @"insert into dbo.MenuPizzaTopping (AvailableForPurchase, Name, PriceLight, PriceRegular, PriceExtra,
                           PizzaToppingType, Description, HasMenuIcon, MenuIconFile, HasPizzaBuilderImage, PizzaBuilderImageFile)
                           output Inserted.Id values (@AvailableForPurchase, @Name, @PriceLight, @PriceRegular, @PriceExtra,
                           @PizzaToppingType, @Description, @HasMenuIcon, @MenuIconFile, @HasPizzaBuilderImage, @PizzaBuilderImageFile);";

            return SqlDataAccess.SaveNewRecord(sql, databaseModel);
        }

        public static List<MenuPizzaSauceModel> LoadMenuPizzaSauces()
        {
            string sql = @"select Id, AvailableForPurchase, Name, PriceLight, PriceRegular, PriceExtra,
                           Description, HasMenuIcon, MenuIconFile, HasPizzaBuilderImage, PizzaBuilderImageFile from dbo.MenuPizzaSauce;";

            return SqlDataAccess.LoadData<MenuPizzaSauceModel>(sql);
        }

        public static int UpdateMenuPizzaSauce(MenuPizzaSauceModel databaseModel)
        {
            string sql = @"update dbo.MenuPizzaSauce set AvailableForPurchase = @AvailableForPurchase, Name = @Name, PriceLight = @PriceLight,
                           PriceRegular = @PriceRegular, PriceExtra = @PriceExtra, Description = @Description, HasMenuIcon = @HasMenuIcon,
                           MenuIconFile = @MenuIconFile, HasPizzaBuilderImage = @HasPizzaBuilderImage, PizzaBuilderImageFile = @PizzaBuilderImageFile where Id = @Id;";

            return SqlDataAccess.UpdateRecord(sql, databaseModel);
        }

        public static int AddMenuPizzaSauce(MenuPizzaSauceModel databaseModel)
        {
            string sql = @"insert into dbo.MenuPizzaSauce (AvailableForPurchase, Name, PriceLight, PriceRegular, PriceExtra,
                           Description, HasMenuIcon, MenuIconFile, HasPizzaBuilderImage, PizzaBuilderImageFile)
                           output Inserted.Id values (@AvailableForPurchase, @Name, @PriceLight, @PriceRegular, @PriceExtra,
                           @Description, @HasMenuIcon, @MenuIconFile, @HasPizzaBuilderImage, @PizzaBuilderImageFile);";

            return SqlDataAccess.SaveNewRecord(sql, databaseModel);
        }

        public static List<MenuPizzaCrustFlavorModel> LoadMenuPizzaCrustFlavors()
        {
            string sql = @"select Id, AvailableForPurchase, Name, Description, HasMenuIcon, MenuIconFile, HasPizzaBuilderImage, PizzaBuilderImageFile from dbo.MenuPizzaCrustFlavor;";

            return SqlDataAccess.LoadData<MenuPizzaCrustFlavorModel>(sql);
        }

        public static int UpdateMenuPizzaCrustFlavor(MenuPizzaCrustFlavorModel databaseModel)
        {
            string sql = @"update dbo.MenuPizzaCrustFlavor set AvailableForPurchase = @AvailableForPurchase, Name = @Name, HasMenuIcon = @HasMenuIcon,
                           MenuIconFile = @MenuIconFile, HasPizzaBuilderImage = @HasPizzaBuilderImage, PizzaBuilderImageFile = @PizzaBuilderImageFile, Description = @Description where Id = @Id;";

            return SqlDataAccess.UpdateRecord(sql, databaseModel);
        }

        public static int AddMenuPizzaCrustFlavor(MenuPizzaCrustFlavorModel databaseModel)
        {
            string sql = @"insert into dbo.MenuPizzaCrustFlavor (AvailableForPurchase, Name, HasMenuIcon, MenuIconFile, HasPizzaBuilderImage, PizzaBuilderImageFile, Description)
                           output Inserted.Id values (@AvailableForPurchase, @Name, @HasMenuIcon, @MenuIconFile, @HasPizzaBuilderImage, @PizzaBuilderImageFile, @Description);";

            return SqlDataAccess.SaveNewRecord(sql, databaseModel);
        }

        public static List<MenuPizzaCrustModel> LoadMenuPizzaCrusts()
        {
            string sql = @"select Id, AvailableForPurchase, Name, PriceSmall, PriceMedium, PriceLarge, Description, HasMenuIcon, MenuIconFile, HasPizzaBuilderImage, PizzaBuilderImageFile from dbo.MenuPizzaCrust;";

            return SqlDataAccess.LoadData<MenuPizzaCrustModel>(sql);
        }

        public static int UpdateMenuPizzaCrust(MenuPizzaCrustModel databaseModel)
        {
            string sql = @"update dbo.MenuPizzaCrust set AvailableForPurchase = @AvailableForPurchase, Name = @Name, PriceSmall = @PriceSmall, PriceMedium = @PriceMedium, PriceLarge = @PriceLarge,
                           Description = @Description, HasMenuIcon = @HasMenuIcon, MenuIconFile = @MenuIconFile, HasPizzaBuilderImage = @HasPizzaBuilderImage, PizzaBuilderImageFile = @PizzaBuilderImageFile where Id = @Id;";

            return SqlDataAccess.UpdateRecord(sql, databaseModel);
        }

        public static int AddMenuPizzaCrust(MenuPizzaCrustModel databaseModel)
        {
            string sql = @"insert into dbo.MenuPizzaCrust (AvailableForPurchase, Name, PriceSmall, PriceMedium, PriceLarge, Description, HasMenuIcon, MenuIconFile, HasPizzaBuilderImage, PizzaBuilderImageFile)
                           output Inserted.Id values (@AvailableForPurchase, @Name, @PriceSmall, @PriceMedium, @PriceLarge, @Description, @HasMenuIcon, @MenuIconFile, @HasPizzaBuilderImage, @PizzaBuilderImageFile);";

            return SqlDataAccess.SaveNewRecord(sql, databaseModel);
        }

        public static List<MenuPizzaCheeseModel> LoadMenuPizzaCheeses()
        {
            string sql = @"select Id, AvailableForPurchase, Name, PriceLight, PriceRegular, PriceExtra, Description, HasMenuIcon, MenuIconFile, HasPizzaBuilderImage, PizzaBuilderImageFile from dbo.MenuPizzaCheese;";

            return SqlDataAccess.LoadData<MenuPizzaCheeseModel>(sql);
        }

        public static int UpdateMenuPizzaCheese(MenuPizzaCheeseModel databaseModel)
        {
            string sql = @"update dbo.MenuPizzaCheese set AvailableForPurchase = @AvailableForPurchase, Name = @Name, PriceLight = @PriceLight, PriceRegular = @PriceRegular, PriceExtra = @PriceExtra,
                           Description = @Description, HasMenuIcon = @HasMenuIcon, MenuIconFile = @MenuIconFile, HasPizzaBuilderImage = @HasPizzaBuilderImage, PizzaBuilderImageFile = @PizzaBuilderImageFile where Id = @Id;";

            return SqlDataAccess.UpdateRecord(sql, databaseModel);
        }

        public static int AddMenuPizzaCheese(MenuPizzaCheeseModel databaseModel)
        {
            string sql = @"insert into dbo.MenuPizzaCheese (AvailableForPurchase, Name, PriceLight, PriceRegular, PriceExtra, Description, HasMenuIcon, MenuIconFile, HasPizzaBuilderImage, PizzaBuilderImageFile)
                           output Inserted.Id values (@AvailableForPurchase, @Name, @PriceLight, @PriceRegular, @PriceExtra, @Description, @HasMenuIcon, @MenuIconFile, @HasPizzaBuilderImage, @PizzaBuilderImageFile);";

            return SqlDataAccess.SaveNewRecord(sql, databaseModel);
        }
    }
}