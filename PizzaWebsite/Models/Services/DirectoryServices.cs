using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using DataLibrary.Models.Tables;

namespace PizzaWebsite.Models.Services
{
    public class DirectoryServices
    {
        public readonly string MenuImageDir = "~/Images/Menu/";

        public string GetMenuIconFile(MenuPizzaCrust record)
        {
            return $"{MenuImageDir}PizzaCrust/{record.Id}-mi.jpg";
        }

        // todo: Implement
        /*public string GetPizzaBuilderImageFile(MenuPizzaCrust record)
        {
            return $"{MenuImageDir}PizzaCrust/{record.Id}-pb.jpg";
        }*/
    }
}