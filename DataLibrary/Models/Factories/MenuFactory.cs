using DataLibrary.Models.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Factories
{
    public static class MenuFactory
    {
        public static MenuWingsSauceModel CreateMenuWingsSauce(
            int id,
            bool availableForPurchase,
            string name,
            string description)
        {
            MenuWingsSauceModel model = new MenuWingsSauceModel();
            model.Id = id;
            model.AvailableForPurchase = availableForPurchase;
            model.Name = name;
            model.Description = description;

            return model;
        }

        public static MenuWingsModel CreateMenuWings(
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
            MenuWingsModel model = new MenuWingsModel();
            model.Id = id;
            model.AvailableForPurchase = availableForPurchase;
            model.Name = name;
            model.Price6Piece = price6Piece;
            model.Price12Piece = price12Piece;
            model.Price18Piece = price18Piece;
            model.Description = description;
            model.HasMenuIcon = hasMenuIcon;
            model.MenuIconFile = menuIconFile;

            return model;
        }

        public static MenuSideModel CreateMenuSide(
            int id,
            bool availableForPurchase,
            string name,
            decimal price,
            string description,
            string itemDetails,
            bool hasMenuIcon,
            string menuIconFile)
        {
            MenuSideModel model = new MenuSideModel();
            model.Id = id;
            model.AvailableForPurchase = availableForPurchase;
            model.Name = name;
            model.Price = price;
            model.Description = description;
            model.ItemDetails = itemDetails;
            model.HasMenuIcon = hasMenuIcon;
            model.MenuIconFile = menuIconFile;

            return model;
        }

        public static MenuSauceModel CreateMenuSauce(
            int id,
            bool availableForPurchase,
            string name,
            decimal price,
            string description,
            string itemDetails,
            bool hasMenuIcon,
            string menuIconFile)
        {
            MenuSauceModel model = new MenuSauceModel();
            model.Id = id;
            model.AvailableForPurchase = availableForPurchase;
            model.Name = name;
            model.Price = price;
            model.Description = description;
            model.ItemDetails = itemDetails;
            model.HasMenuIcon = hasMenuIcon;
            model.MenuIconFile = menuIconFile;

            return model;
        }

        public static MenuPastaModel CreateMenuPasta(
            int id,
            bool availableForPurchase,
            string name,
            decimal price,
            string description,
            string itemDetails,
            bool hasMenuIcon,
            string menuIconFile)
        {
            MenuPastaModel model = new MenuPastaModel();
            model.Id = id;
            model.AvailableForPurchase = availableForPurchase;
            model.Name = name;
            model.Price = price;
            model.Description = description;
            model.ItemDetails = itemDetails;
            model.HasMenuIcon = hasMenuIcon;
            model.MenuIconFile = menuIconFile;

            return model;
        }

        public static MenuDrinkModel CreateMenuDrink(
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
            MenuDrinkModel model = new MenuDrinkModel();
            model.Id = id;
            model.AvailableForPurchase = availableForPurchase;
            model.Name = name;
            model.AvailableIn20Oz = availableIn20Oz;
            model.AvailableIn2Liter = availableIn2Liter;
            model.AvailableIn2Pack12Oz = availableIn2Pack12Oz;
            model.AvailableIn6Pack12Oz = availableIn6Pack12Oz;
            model.Price20Oz = price20Oz;
            model.Price2Liter = price2Liter;
            model.Price2Pack12Oz = price2Pack12Oz;
            model.Price6Pack12Oz = price6Pack12Oz;
            model.Description = description;
            model.HasMenuIcon = hasMenuIcon;
            model.MenuIconFile = menuIconFile;

            return model;
        }

        public static MenuDipModel CreateMenuDip(
            int id,
            bool availableForPurchase,
            string name,
            decimal price,
            string itemDetails,
            bool hasMenuIcon,
            string menuIconFile)
        {
            MenuDipModel model = new MenuDipModel();
            model.Id = id;
            model.AvailableForPurchase = availableForPurchase;
            model.Name = name;
            model.Price = price;
            model.ItemDetails = itemDetails;
            model.HasMenuIcon = hasMenuIcon;
            model.MenuIconFile = menuIconFile;

            return model;
        }

        public static MenuDessertModel CreateMenuDessert(
            int id,
            bool availableForPurchase,
            string name,
            decimal price,
            string description,
            string itemDetails,
            bool hasMenuIcon,
            string menuIconFile)
        {
            MenuDessertModel model = new MenuDessertModel();
            model.Id = id;
            model.AvailableForPurchase = availableForPurchase;
            model.Name = name;
            model.Price = price;
            model.Description = description;
            model.ItemDetails = itemDetails;
            model.HasMenuIcon = hasMenuIcon;
            model.MenuIconFile = menuIconFile;

            return model;
        }
    }
}
