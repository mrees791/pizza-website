using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
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

        public string GetMenuImageUrl(MenuPizzaTopping record, MenuImageType imageType)
        {
            return $"{PizzaToppingMenuImageDir}{CreateMenuImageFileName(record.Id, imageType)}";
        }

        public string GetMenuImageUrl(MenuPizzaCrustFlavor record, MenuImageType imageType)
        {
            return $"{PizzaCrustFlavorMenuImageDir}{CreateMenuImageFileName(record.Id, imageType)}";
        }

        public string GetMenuImageUrl(MenuPizzaCheese record, MenuImageType imageType)
        {
            return $"{PizzaCheeseMenuImageDir}{CreateMenuImageFileName(record.Id, imageType)}";
        }

        public string GetMenuImageUrl(MenuPizzaSauce record, MenuImageType imageType)
        {
            return $"{PizzaSauceMenuImageDir}{CreateMenuImageFileName(record.Id, imageType)}";
        }

        public string GetMenuImageUrl(MenuPizzaCrust record, MenuImageType imageType)
        {
            return $"{PizzaCrustMenuImageDir}{CreateMenuImageFileName(record.Id, imageType)}";
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
            throw new Exception($"Unable to create menu image file name for {id}.");
        }
    }
}