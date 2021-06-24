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
        public readonly string PizzaSauceMenuImageDir = "~/Images/Menu/PizzaSauce/";
        public readonly string PizzaCheeseMenuImageDir = "~/Images/Menu/PizzaCheese/";
        public readonly string PizzaCrustMenuImageDir = "~/Images/Menu/PizzaCrust/";
        public readonly string PizzaCrustFlavorMenuImageDir = "~/Images/Menu/PizzaCrustFlavor/";
        public readonly string PizzaToppingMenuImageDir = "~/Images/Menu/PizzaTopping/";

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
                    return PizzaSauceMenuImageDir;
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
                    return $"{id}-mi.jpg";
                case MenuImageType.PizzaBuilderImage:
                    return $"{id}-pb.jpg";
            }
            throw new Exception($"Unable to create menu image file name for {id}. Image type: {imageType.ToString()}");
        }
    }
}