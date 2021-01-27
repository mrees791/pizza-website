using DataLibrary.DataAccess;
using DataLibrary.Models.Cart;
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
        // Needs tested
        public static List<CartModel> LoadCarts()
        {
            string sql = @"select cart_id as CartId from dbo.cart";

            return SqlDataAccess.LoadData<CartModel>(sql);
        }

        // Needs tested
        public static int AddNewCart()
        {
            CartModel data = new CartModel
            {

            };

            string sql = @"insert into dbo.cart;";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static List<MenuDipModel> LoadMenuDips()
        {
            string sql = @"select menu_dip_id as MenuDipId, available_for_purchase as AvailableForPurchase, name as Name, price as Price, item_details as ItemDetails,
                            has_menu_icon as HasMenuIcon, menu_icon_file as MenuIconFile from dbo.menu_dip;";

            return SqlDataAccess.LoadData<MenuDipModel>(sql);
        }

        /// <summary>
        /// Adds a new record to the menu_dip table.
        /// </summary>
        /// <param name="availableForPurchase"></param>
        /// <param name="name"></param>
        /// <param name="price"></param>
        /// <param name="itemDetails"></param>
        /// <param name="hasMenuIcon"></param>
        /// <param name="menuIconFile"></param>
        /// <returns>Number of rows affected.</returns>
        public static int AddNewMenuDip(bool availableForPurchase, string name,
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
