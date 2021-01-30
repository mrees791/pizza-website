using DataLibrary.Models.Menu.Pizzas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Factories
{
    public static class MenuPizzaFactory
    {
        public static MenuPizzaToppingModel CreateMenuPizzaTopping(
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
            MenuPizzaToppingModel model = new MenuPizzaToppingModel();
            model.Id = id;
            model.AvailableForPurchase = availableForPurchase;
            model.Name = name;
            model.PriceLight = priceLight;
            model.PriceRegular = priceRegular;
            model.PriceExtra = priceExtra;
            model.PizzaToppingType = pizzaToppingType;
            model.Description = description;
            model.HasMenuIcon = hasMenuIcon;
            model.MenuIconFile = menuIconFile;
            model.HasPizzaBuilderImage = hasPizzaBuilderImage;
            model.PizzaBuilderImageFile = pizzaBuilderImageFile;

            return model;
        }

        public static MenuPizzaSauceModel CreateMenuPizzaSauce(
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
            MenuPizzaSauceModel model = new MenuPizzaSauceModel();
            model.Id = id;
            model.AvailableForPurchase = availableForPurchase;
            model.Name = name;
            model.PriceLight = priceLight;
            model.PriceRegular = priceRegular;
            model.PriceExtra = priceExtra;
            model.Description = description;
            model.HasMenuIcon = hasMenuIcon;
            model.MenuIconFile = menuIconFile;
            model.HasPizzaBuilderImage = hasPizzaBuilderImage;
            model.PizzaBuilderImageFile = pizzaBuilderImageFile;

            return model;
        }

        public static MenuPizzaCrustFlavorModel CreateMenuPizzaCrustFlavor(
            int id,
            bool availableForPurchase,
            string name,
            string description,
            bool hasMenuIcon,
            string menuIconFile,
            bool hasPizzaBuilderImage,
            string pizzaBuilderImageFile)
        {
            MenuPizzaCrustFlavorModel model = new MenuPizzaCrustFlavorModel();
            model.Id = id;
            model.AvailableForPurchase = availableForPurchase;
            model.Name = name;
            model.Description = description;
            model.HasMenuIcon = hasMenuIcon;
            model.MenuIconFile = menuIconFile;
            model.HasPizzaBuilderImage = hasPizzaBuilderImage;
            model.PizzaBuilderImageFile = pizzaBuilderImageFile;

            return model;
        }

        public static MenuPizzaCrustModel CreateMenuPizzaCrust(
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
            MenuPizzaCrustModel model = new MenuPizzaCrustModel();
            model.Id = id;
            model.AvailableForPurchase = availableForPurchase;
            model.Name = name;
            model.PriceSmall = priceSmall;
            model.PriceMedium = priceMedium;
            model.PriceLarge = priceLarge;
            model.Description = description;
            model.HasMenuIcon = hasMenuIcon;
            model.MenuIconFile = menuIconFile;
            model.HasPizzaBuilderImage = hasPizzaBuilderImage;
            model.PizzaBuilderImageFile = pizzaBuilderImageFile;

            return model;
        }

        public static MenuPizzaCheeseModel CreateMenuPizzaCheese(
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
            MenuPizzaCheeseModel model = new MenuPizzaCheeseModel();
            model.Id = id;
            model.AvailableForPurchase = availableForPurchase;
            model.Name = name;
            model.PriceLight = priceLight;
            model.PriceRegular = priceRegular;
            model.PriceExtra = priceExtra;
            model.Description = description;
            model.HasMenuIcon = hasMenuIcon;
            model.MenuIconFile = menuIconFile;
            model.HasPizzaBuilderImage = hasPizzaBuilderImage;
            model.PizzaBuilderImageFile = pizzaBuilderImageFile;

            return model;
        }
    }
}
