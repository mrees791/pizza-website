using DataLibrary.Models;
using DataLibrary.Models.JoinLists;
using DataLibrary.Models.JoinLists.CartItems;
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
        public int CartId { get; set; }
        public List<CartItemViewModel> CartItemList { get; set; }
        public CostSummaryViewModel CostSummaryVm { get; set; }

        public async Task InitializeAsync(int cartId, PizzaDatabase pizzaDb)
        {
            CartId = cartId;
            CostSummaryVm = new CostSummaryViewModel();
            CartItemList = new List<CartItemViewModel>();

            List<int> quantityList = ListUtility.CreateQuantityList();
            CartItemOnCartItemTypeJoin cartItemJoinList = new CartItemOnCartItemTypeJoin();
            await cartItemJoinList.LoadListByCartIdAsync(cartId, pizzaDb);
            CostSummaryVm.Initialize(new CostSummary(cartItemJoinList.Items));

            foreach (CartItemJoin cartItemJoin in cartItemJoinList.Items)
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