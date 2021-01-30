using DataLibrary.DataAccess;
using DataLibrary.Models.Factories;
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

        public static int UpdateMenuPizzaTopping(
            int id,
            bool availableForPurchase,
            string name,
            decimal priceLight,
            decimal priceRegular,
            decimal priceExtra,
            string pizzaToppingType,
            string description,
            bool hasMenuIcon,
            string menuIconFile,
            bool hasPizzaBuilderImage,
            string pizzaBuilderImageFile)
        {
            MenuPizzaToppingModel data = MenuPizzaFactory.CreateMenuPizzaTopping(id, availableForPurchase, name, priceLight, priceRegular, priceExtra, pizzaToppingType, description,
                hasMenuIcon, menuIconFile, hasPizzaBuilderImage, pizzaBuilderImageFile);

            string sql = @"update dbo.MenuPizzaTopping set AvailableForPurchase = @AvailableForPurchase, Name = @Name, PriceLight = @PriceLight,
                           PriceRegular = @PriceRegular, PriceExtra = @PriceExtra, PizzaToppingType = @PizzaToppingType, Description = @Description,
                           HasMenuIcon = @HasMenuIcon, MenuIconFile = @MenuIconFile, HasPizzaBuilderImage = @HasPizzaBuilderImage, PizzaBuilderImageFile = @PizzaBuilderImageFile where Id = @Id;";

            return SqlDataAccess.UpdateRecord(sql, data);
        }

        public static int AddMenuPizzaTopping(
            bool availableForPurchase,
            string name,
            decimal priceLight,
            decimal priceRegular,
            decimal priceExtra,
            string pizzaToppingType,
            string description,
            bool hasMenuIcon,
            string menuIconFile,
            bool hasPizzaBuilderImage,
            string pizzaBuilderImageFile)
        {
            MenuPizzaToppingModel data = MenuPizzaFactory.CreateMenuPizzaTopping(0, availableForPurchase, name, priceLight, priceRegular, priceExtra, pizzaToppingType, description,
                hasMenuIcon, menuIconFile, hasPizzaBuilderImage, pizzaBuilderImageFile);

            string sql = @"insert into dbo.MenuPizzaTopping (AvailableForPurchase, Name, PriceLight, PriceRegular, PriceExtra,
                           PizzaToppingType, Description, HasMenuIcon, MenuIconFile, HasPizzaBuilderImage, PizzaBuilderImageFile)
                           output Inserted.Id values (@AvailableForPurchase, @Name, @PriceLight, @PriceRegular, @PriceExtra,
                           @PizzaToppingType, @Description, @HasMenuIcon, @MenuIconFile, @HasPizzaBuilderImage, @PizzaBuilderImageFile);";

            return SqlDataAccess.SaveNewRecord(sql, data);
        }

        public static List<MenuPizzaSauceModel> LoadMenuPizzaSauces()
        {
            string sql = @"select Id, AvailableForPurchase, Name, PriceLight, PriceRegular, PriceExtra,
                           Description, HasMenuIcon, MenuIconFile, HasPizzaBuilderImage, PizzaBuilderImageFile from dbo.MenuPizzaSauce;";

            return SqlDataAccess.LoadData<MenuPizzaSauceModel>(sql);
        }

        public static int UpdateMenuPizzaSauce(
            int id,
            bool availableForPurchase,
            string name,
            decimal priceLight,
            decimal priceRegular,
            decimal priceExtra,
            string description,
            bool hasMenuIcon,
            string menuIconFile,
            bool hasPizzaBuilderImage,
            string pizzaBuilderImageFile)
        {
            MenuPizzaSauceModel data = MenuPizzaFactory.CreateMenuPizzaSauce(id, availableForPurchase, name, priceLight, priceRegular, priceExtra, description, hasMenuIcon, menuIconFile,
                hasPizzaBuilderImage, pizzaBuilderImageFile);

            string sql = @"update dbo.MenuPizzaSauce set AvailableForPurchase = @AvailableForPurchase, Name = @Name, PriceLight = @PriceLight,
                           PriceRegular = @PriceRegular, PriceExtra = @PriceExtra, Description = @Description, HasMenuIcon = @HasMenuIcon,
                           MenuIconFile = @MenuIconFile, HasPizzaBuilderImage = @HasPizzaBuilderImage, PizzaBuilderImageFile = @PizzaBuilderImageFile where Id = @Id;";

            return SqlDataAccess.UpdateRecord(sql, data);
        }

        public static int AddMenuPizzaSauce(
            bool availableForPurchase,
            string name,
            decimal priceLight,
            decimal priceRegular,
            decimal priceExtra,
            string description,
            bool hasMenuIcon,
            string menuIconFile,
            bool hasPizzaBuilderImage,
            string pizzaBuilderImageFile)
        {
            MenuPizzaSauceModel data = MenuPizzaFactory.CreateMenuPizzaSauce(0, availableForPurchase, name, priceLight, priceRegular, priceExtra, description, hasMenuIcon, menuIconFile,
                hasPizzaBuilderImage, pizzaBuilderImageFile);

            string sql = @"insert into dbo.MenuPizzaSauce (AvailableForPurchase, Name, PriceLight, PriceRegular, PriceExtra,
                           Description, HasMenuIcon, MenuIconFile, HasPizzaBuilderImage, PizzaBuilderImageFile)
                           output Inserted.Id values (@AvailableForPurchase, @Name, @PriceLight, @PriceRegular, @PriceExtra,
                           @Description, @HasMenuIcon, @MenuIconFile, @HasPizzaBuilderImage, @PizzaBuilderImageFile);";

            return SqlDataAccess.SaveNewRecord(sql, data);
        }

        public static List<MenuPizzaCrustFlavorModel> LoadMenuPizzaCrustFlavors()
        {
            string sql = @"select Id, AvailableForPurchase, Name, Description, HasMenuIcon, MenuIconFile, HasPizzaBuilderImage, PizzaBuilderImageFile from dbo.MenuPizzaCrustFlavor;";

            return SqlDataAccess.LoadData<MenuPizzaCrustFlavorModel>(sql);
        }

        public static int UpdateMenuPizzaCrustFlavor(
            int id,
            bool availableForPurchase,
            string name,
            string description,
            bool hasMenuIcon,
            string menuIconFile,
            bool hasPizzaBuilderImage,
            string pizzaBuilderImageFile)
        {
            MenuPizzaCrustFlavorModel data = MenuPizzaFactory.CreateMenuPizzaCrustFlavor(id, availableForPurchase, name, description, hasMenuIcon, menuIconFile, hasPizzaBuilderImage, pizzaBuilderImageFile);

            string sql = @"update dbo.MenuPizzaCrustFlavor set AvailableForPurchase = @AvailableForPurchase, Name = @Name, HasMenuIcon = @HasMenuIcon,
                           MenuIconFile = @MenuIconFile, HasPizzaBuilderImage = @HasPizzaBuilderImage, PizzaBuilderImageFile = @PizzaBuilderImageFile, Description = @Description where Id = @Id;";

            return SqlDataAccess.UpdateRecord(sql, data);
        }

        public static int AddMenuPizzaCrustFlavor(
            bool availableForPurchase,
            string name,
            string description,
            bool hasMenuIcon,
            string menuIconFile,
            bool hasPizzaBuilderImage,
            string pizzaBuilderImageFile)
        {
            MenuPizzaCrustFlavorModel data = MenuPizzaFactory.CreateMenuPizzaCrustFlavor(0, availableForPurchase, name, description, hasMenuIcon, menuIconFile, hasPizzaBuilderImage, pizzaBuilderImageFile);

            string sql = @"insert into dbo.MenuPizzaCrustFlavor (AvailableForPurchase, Name, HasMenuIcon, MenuIconFile, HasPizzaBuilderImage, PizzaBuilderImageFile, Description)
                           output Inserted.Id values (@AvailableForPurchase, @Name, @HasMenuIcon, @MenuIconFile, @HasPizzaBuilderImage, @PizzaBuilderImageFile, @Description);";

            return SqlDataAccess.SaveNewRecord(sql, data);
        }

        public static List<MenuPizzaCrustModel> LoadMenuPizzaCrusts()
        {
            string sql = @"select Id, AvailableForPurchase, Name, PriceSmall, PriceMedium, PriceLarge, Description, HasMenuIcon, MenuIconFile, HasPizzaBuilderImage, PizzaBuilderImageFile from dbo.MenuPizzaCrust;";

            return SqlDataAccess.LoadData<MenuPizzaCrustModel>(sql);
        }

        public static int UpdateMenuPizzaCrust(
            int id,
            bool availableForPurchase,
            string name,
            decimal priceSmall,
            decimal priceMedium,
            decimal priceLarge,
            string description,
            bool hasMenuIcon,
            string menuIconFile,
            bool hasPizzaBuilderImage,
            string pizzaBuilderImageFile)
        {
            MenuPizzaCrustModel data = MenuPizzaFactory.CreateMenuPizzaCrust(id, availableForPurchase, name, priceSmall, priceMedium, priceLarge, description,
                hasMenuIcon, menuIconFile, hasPizzaBuilderImage, pizzaBuilderImageFile);

            string sql = @"update dbo.MenuPizzaCrust set AvailableForPurchase = @AvailableForPurchase, Name = @Name, PriceSmall = @PriceSmall, PriceMedium = @PriceMedium, PriceLarge = @PriceLarge,
                           Description = @Description, HasMenuIcon = @HasMenuIcon, MenuIconFile = @MenuIconFile, HasPizzaBuilderImage = @HasPizzaBuilderImage, PizzaBuilderImageFile = @PizzaBuilderImageFile where Id = @Id;";

            return SqlDataAccess.UpdateRecord(sql, data);
        }

        public static int AddMenuPizzaCrust(
            bool availableForPurchase,
            string name,
            decimal priceSmall,
            decimal priceMedium,
            decimal priceLarge,
            string description,
            bool hasMenuIcon,
            string menuIconFile,
            bool hasPizzaBuilderImage,
            string pizzaBuilderImageFile)
        {
            MenuPizzaCrustModel data = MenuPizzaFactory.CreateMenuPizzaCrust(0, availableForPurchase, name, priceSmall, priceMedium, priceLarge, description,
                hasMenuIcon, menuIconFile, hasPizzaBuilderImage, pizzaBuilderImageFile);

            string sql = @"insert into dbo.MenuPizzaCrust (AvailableForPurchase, Name, PriceSmall, PriceMedium, PriceLarge, Description, HasMenuIcon, MenuIconFile, HasPizzaBuilderImage, PizzaBuilderImageFile)
                           output Inserted.Id values (@AvailableForPurchase, @Name, @PriceSmall, @PriceMedium, @PriceLarge, @Description, @HasMenuIcon, @MenuIconFile, @HasPizzaBuilderImage, @PizzaBuilderImageFile);";

            return SqlDataAccess.SaveNewRecord(sql, data);
        }

        public static List<MenuPizzaCheeseModel> LoadMenuPizzaCheeses()
        {
            string sql = @"select Id, AvailableForPurchase, Name, PriceLight, PriceRegular, PriceExtra, Description, HasMenuIcon, MenuIconFile, HasPizzaBuilderImage, PizzaBuilderImageFile from dbo.MenuPizzaCheese;";

            return SqlDataAccess.LoadData<MenuPizzaCheeseModel>(sql);
        }

        public static int UpdateMenuPizzaCheese(
            int id,
            bool availableForPurchase,
            string name,
            decimal priceLight,
            decimal priceRegular,
            decimal priceExtra,
            string description,
            bool hasMenuIcon,
            string menuIconFile,
            bool hasPizzaBuilderImage,
            string pizzaBuilderImageFile)
        {
            MenuPizzaCheeseModel data = MenuPizzaFactory.CreateMenuPizzaCheese(id, availableForPurchase, name, priceLight, priceRegular, priceExtra, description, hasMenuIcon,
                menuIconFile, hasPizzaBuilderImage, pizzaBuilderImageFile);

            string sql = @"update dbo.MenuPizzaCheese set AvailableForPurchase = @AvailableForPurchase, Name = @Name, PriceLight = @PriceLight, PriceRegular = @PriceRegular, PriceExtra = @PriceExtra,
                           Description = @Description, HasMenuIcon = @HasMenuIcon, MenuIconFile = @MenuIconFile, HasPizzaBuilderImage = @HasPizzaBuilderImage, PizzaBuilderImageFile = @PizzaBuilderImageFile where Id = @Id;";

            return SqlDataAccess.UpdateRecord(sql, data);
        }

        public static int AddMenuPizzaCheese(
            bool availableForPurchase,
            string name,
            decimal priceLight,
            decimal priceRegular,
            decimal priceExtra,
            string description,
            bool hasMenuIcon,
            string menuIconFile,
            bool hasPizzaBuilderImage,
            string pizzaBuilderImageFile)
        {
            MenuPizzaCheeseModel data = MenuPizzaFactory.CreateMenuPizzaCheese(0, availableForPurchase, name, priceLight, priceRegular, priceExtra, description, hasMenuIcon,
                menuIconFile, hasPizzaBuilderImage, pizzaBuilderImageFile);

            string sql = @"insert into dbo.MenuPizzaCheese (AvailableForPurchase, Name, PriceLight, PriceRegular, PriceExtra, Description, HasMenuIcon, MenuIconFile, HasPizzaBuilderImage, PizzaBuilderImageFile)
                           output Inserted.Id values (@AvailableForPurchase, @Name, @PriceLight, @PriceRegular, @PriceExtra, @Description, @HasMenuIcon, @MenuIconFile, @HasPizzaBuilderImage, @PizzaBuilderImageFile);";

            return SqlDataAccess.SaveNewRecord(sql, data);
        }
    }
}