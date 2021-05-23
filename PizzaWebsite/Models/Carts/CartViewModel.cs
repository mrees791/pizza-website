using DataLibrary.Models;
using DataLibrary.Models.Joins;
using DataLibrary.Models.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PizzaWebsite.Models.Carts
{

    public class CartViewModel
    {
        public List<CartItemViewModel> CartItemList { get; set; }
        public CostSummaryViewModel CostSummaryVm { get; set; }

        public async Task InitializeAsync(int cartId, PizzaDatabase pizzaDb)
        {
            CostSummaryVm = new CostSummaryViewModel();
            CartItemList = new List<CartItemViewModel>();

            List<int> quantityList = ListUtility.CreateQuantityList();
            IEnumerable<CartItemJoin> cartItemList = await pizzaDb.GetJoinedCartItemListAsync(cartId);
            CostSummaryVm.Initialize(new CostSummary(cartItemList));

            foreach (CartItemJoin cartItemJoin in cartItemList)
            {
                CartItemViewModel cartItemVm = new CartItemViewModel()
                {
                    CartItemJoin = cartItemJoin,
                    CartItemId = cartItemJoin.CartItem.Id,
                    ProductCategory = cartItemJoin.CartItem.ProductCategory,
                    Price = cartItemJoin.CartItem.Price.ToString("C", CultureInfo.CurrentCulture),
                    Quantity = cartItemJoin.CartItem.Quantity,
                    QuantityList = quantityList,
                    CartItemQuantitySelectId = $"cartItemQuantitySelect-{cartItemJoin.CartItem.Id}",
                    CartItemDeleteButtonId = $"cartItemDeleteButton-{cartItemJoin.CartItem.Id}",
                    CartItemPriceCellId = $"cartItemPriceCell-{cartItemJoin.CartItem.Id}",
                    CartItemRowId = $"cartItemRow-{cartItemJoin.CartItem.Id}"
                };

                await cartItemVm.UpdateAsync(pizzaDb);

                CartItemList.Add(cartItemVm);
            }
        }

        public bool IsEmpty()
        {
            return !CartItemList.Any();
        }
    }
}