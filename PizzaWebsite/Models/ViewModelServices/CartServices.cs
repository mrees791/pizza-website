using DataLibrary.Models;
using DataLibrary.Models.JoinLists;
using DataLibrary.Models.Utility;
using PizzaWebsite.Models.Carts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PizzaWebsite.Models.ViewModelServices
{
    public class CartServices
    {
        public async Task<CartViewModel> CreateViewModelAsync(int cartId, PizzaDatabase pizzaDb)
        {
            List<int> quantityList = ListUtility.CreateQuantityList();
            CartItemJoinList cartItemJoinList = new CartItemJoinList();
            await cartItemJoinList.LoadListByCartIdAsync(cartId, pizzaDb);
            CostSummaryServices costSummaryServices = new CostSummaryServices();
            CostSummaryViewModel costSummaryVm = costSummaryServices.CreateViewModel(new CostSummary(cartItemJoinList.Items));
            List<CartItemViewModel> cartItemVmList = new List<CartItemViewModel>();
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
                cartItemVmList.Add(cartItemVm);
            }
            return new CartViewModel()
            {
                CartId = cartId,
                CostSummaryVm = costSummaryVm,
                CartItemVmList = cartItemVmList
            };
        }
    }
}