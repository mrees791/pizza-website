using DataLibrary.Models.Tables;
using DataLibrary.Models.Utility;
using PizzaWebsite.Models;
using PizzaWebsite.Models.Identity.Stores;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PizzaWebsite.Controllers
{
    public class ShopController : BaseController
    {
        public async Task<ActionResult> Cart()
        {
            // IF USER IS NOT SIGNED IN
            // todo: Implement

            // IF USER IS SIGNED IN
            List<SiteUser> users = await PizzaDb.GetListAsync<SiteUser>(new { UserName = User.Identity.Name });
            SiteUser user = users.First();

            List<CartItem> cartItemList = await PizzaDb.GetListAsync<CartItem>(new { CartId = user.CurrentCartId });

            CartViewModel cartVm = new CartViewModel();

            foreach (CartItem cartItem in cartItemList)
            {
                CartItemViewModel cartItemVm = new CartItemViewModel()
                {
                    CartItemId = cartItem.Id,
                    ProductCategory = CartItemUtility.FindProductCategory(cartItem.ProductCategory),
                    Name = cartItem.Name,
                    Price = cartItem.PricePerItem.ToString("C", CultureInfo.CurrentCulture),
                    Quantity = cartItem.Quantity
                };

                cartVm.CartItemList.Add(cartItemVm);
            }

            return View(cartVm);
        }
    }
}