using DataLibrary.Models;
using DataLibrary.Models.JoinLists;
using DataLibrary.Models.Utility;
using PizzaWebsite.Models.ViewModelServices;
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
            CartItemList = new List<CartItemViewModel>();
            CostSummaryServices costSummaryServices = new CostSummaryServices();
            List<int> quantityList = ListUtility.CreateQuantityList();
            CartItemJoinList cartItemJoinList = new CartItemJoinList();
            await cartItemJoinList.LoadListByCartIdAsync(cartId, pizzaDb);
            CostSummaryVm = costSummaryServices.CreateViewModel(new CostSummary(cartItemJoinList.Items));
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