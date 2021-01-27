using DataLibrary.DataAccess;
using DataLibrary.Models.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic
{
    public static class MenuProcessor
    {
        public static int AddNewMenuDipRecord(bool availableForPurchase, string name,
            decimal price, string itemDetails, bool hasMenuIcon, string menuIconFile)
        {
            MenuDipModel data = new MenuDipModel
            {
                AvailableForPurchase = availableForPurchase,
                Name = name,
                Price = price,
                ItemDetails = itemDetails,
                HasMenuIcon = hasMenuIcon,
                MenuIconFile = menuIconFile
            };

            string sql = @"insert into dbo.menu_dip (available_for_purchase, name, price, item_details, has_menu_icon, menu_icon_file)
                        values (@AvailableForPurchase, @Name, @Price, @ItemDetails, @HasMenuIcon, @MenuIconFile);";

            return SqlDataAccess.SaveData(sql, data);
        }
    }
}
