using DataLibrary.DataAccess;
using DataLibrary.Models.Menu.Pizza;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic
{
    public static class DatabaseMenuPizzaProcessor
    {
        public static List<MenuPizzaSauceModel> LoadMenuPizzaSaucees()
        {
            string sql = @"select Id, AvailableForPurchase, Name, PriceLight, PriceRegular, PriceExtra,
                           Description, HasMenuIcon, MenuIconFile, HasPizzaBuilderImage, PizzaBuilderImageFile from dbo.MenuPizzaSauceFlavor;";

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
            MenuPizzaSauceModel data = new MenuPizzaSauceModel(id, availableForPurchase, name, hasMenuIcon, menuIconFile,
                hasPizzaBuilderImage, pizzaBuilderImageFile, description, priceLight, priceRegular, priceExtra);

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
            MenuPizzaSauceModel data = new MenuPizzaSauceModel(0, availableForPurchase, name, hasMenuIcon, menuIconFile,
                hasPizzaBuilderImage, pizzaBuilderImageFile, description, priceLight, priceRegular, priceExtra);

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
            MenuPizzaCrustFlavorModel data = new MenuPizzaCrustFlavorModel(id, availableForPurchase, name, hasMenuIcon, menuIconFile, hasPizzaBuilderImage, pizzaBuilderImageFile, description);

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
            MenuPizzaCrustFlavorModel data = new MenuPizzaCrustFlavorModel(0, availableForPurchase, name, hasMenuIcon, menuIconFile, hasPizzaBuilderImage, pizzaBuilderImageFile, description);

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
            MenuPizzaCrustModel data = new MenuPizzaCrustModel(id, availableForPurchase, name, hasMenuIcon, menuIconFile, hasPizzaBuilderImage, pizzaBuilderImageFile, description,
                priceSmall, priceMedium, priceLarge);

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
            MenuPizzaCrustModel data = new MenuPizzaCrustModel(0, availableForPurchase, name, hasMenuIcon, menuIconFile, hasPizzaBuilderImage, pizzaBuilderImageFile, description,
                priceSmall, priceMedium, priceLarge);

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
            MenuPizzaCheeseModel data = new MenuPizzaCheeseModel(id, availableForPurchase, name, hasMenuIcon, menuIconFile, hasPizzaBuilderImage, pizzaBuilderImageFile, description,
                priceLight, priceRegular, priceExtra);

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
            MenuPizzaCheeseModel data = new MenuPizzaCheeseModel(0, availableForPurchase, name, hasMenuIcon, menuIconFile, hasPizzaBuilderImage, pizzaBuilderImageFile, description,
                priceLight, priceRegular, priceExtra);

            string sql = @"insert into dbo.MenuPizzaCheese (AvailableForPurchase, Name, PriceLight, PriceRegular, PriceExtra, Description, HasMenuIcon, MenuIconFile, HasPizzaBuilderImage, PizzaBuilderImageFile)
                           output Inserted.Id values (@AvailableForPurchase, @Name, @PriceLight, @PriceRegular, @PriceExtra, @Description, @HasMenuIcon, @MenuIconFile, @HasPizzaBuilderImage, @PizzaBuilderImageFile);";

            return SqlDataAccess.SaveNewRecord(sql, data);
        }
    }
}