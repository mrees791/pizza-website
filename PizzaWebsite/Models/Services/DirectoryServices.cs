using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using DataLibrary.Models;
using DataLibrary.Models.Tables;

namespace PizzaWebsite.Models.Services
{
    public class DirectoryServices
    {
        public readonly string PizzaMenuImageDir = "/Content/Images/Menu/Pizza/";
        public readonly string PizzaSauceMenuImageDir = "/Content/Images/Menu/PizzaSauce/";
        public readonly string PizzaCheeseMenuImageDir = "/Content/Images/Menu/PizzaCheese/";
        public readonly string PizzaCrustMenuImageDir = "/Content/Images/Menu/PizzaCrust/";
        public readonly string PizzaCrustFlavorMenuImageDir = "/Content/Images/Menu/PizzaCrustFlavor/";
        public readonly string PizzaToppingMenuImageDir = "/Content/Images/Menu/PizzaTopping/";

        public string GetMenuImageUrl(int id, MenuCategory menuCategory, MenuImageType imageType)
        {
            string dir = GetMenuCategoryImageDirectory(menuCategory);
            string fileName = CreateMenuImageFileName(id, imageType);
            return $"{dir}{fileName}";
        }

        private string GetMenuCategoryImageDirectory(MenuCategory menuCategory)
        {
            switch (menuCategory)
            {
                case MenuCategory.Pizza:
                    return PizzaMenuImageDir;
                case MenuCategory.PizzaCheese:
                    return PizzaCheeseMenuImageDir;
                case MenuCategory.PizzaCrust:
                    return PizzaCrustMenuImageDir;
                case MenuCategory.PizzaCrustFlavor:
                    return PizzaCrustFlavorMenuImageDir;
                case MenuCategory.PizzaSauce:
                    return PizzaSauceMenuImageDir;
                case MenuCategory.PizzaToppingType:
                    return PizzaToppingMenuImageDir;
            }

            throw new Exception($"Unable to get image directory for {menuCategory.ToString()}");
        }

        private string CreateMenuImageFileName(int id, MenuImageType imageType)
        {
            switch (imageType)
            {
                case MenuImageType.MenuIcon:
                    return $"{id}-mi.webp";
                case MenuImageType.PizzaBuilder:
                    return $"{id}-pb.webp";
                case MenuImageType.PizzaBuilderLeft:
                    return $"{id}-pbl.webp";
                case MenuImageType.PizzaBuilderRight:
                    return $"{id}-pbr.webp";
                case MenuImageType.PizzaBuilderExtra:
                    return $"{id}-pbe.webp";
                case MenuImageType.PizzaBuilderExtraLeft:
                    return $"{id}-pbel.webp";
                case MenuImageType.PizzaBuilderExtraRight:
                    return $"{id}-pber.webp";
            }
            throw new Exception($"Unable to create menu image file name for {id}. Image type: {imageType.ToString()}");
        }
    }
}