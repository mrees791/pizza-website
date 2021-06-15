using System.Collections.Generic;
using System.Threading.Tasks;
using DataLibrary.Models;
using DataLibrary.Models.JoinLists;
using PizzaWebsite.Models.Carts;

namespace PizzaWebsite.Models.ViewModelServices
{
    public class CartServices
    {
        public async Task<CartViewModel> CreateViewModelAsync(int cartId, PizzaDatabase pizzaDb,
            IEnumerable<int> quantityList)
        {
            CartItemJoinList cartItemJoinList = new CartItemJoinList();
            await cartItemJoinList.LoadListByCartIdAsync(cartId, pizzaDb);
            CostSummaryServices costSummaryServices = new CostSummaryServices();
            CostSummaryViewModel costSummaryVm =
                costSummaryServices.CreateViewModel(new CostSummary(cartItemJoinList.Items));
            List<CartItemViewModel> cartItemVmList = new List<CartItemViewModel>();
            CartItemServices cartItemServices = new CartItemServices();
            foreach (CartItemJoin cartItemJoin in cartItemJoinList.Items)
            {
                CartItemViewModel cartItemVm =
                    await cartItemServices.CreateViewModelAsync(cartItemJoin, pizzaDb, quantityList);
                cartItemVmList.Add(cartItemVm);
            }

            return new CartViewModel
            {
                CartId = cartId,
                CostSummaryVm = costSummaryVm,
                CartItemVmList = cartItemVmList
            };
        }
    }
}