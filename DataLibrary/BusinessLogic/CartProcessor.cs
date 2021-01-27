using DataLibrary.DataAccess;
using DataLibrary.Models.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic
{
    public static class CartProcessor
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
    }
}
