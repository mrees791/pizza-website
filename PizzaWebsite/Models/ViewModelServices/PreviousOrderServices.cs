using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using DataLibrary.Models;
using DataLibrary.Models.Tables;
using PizzaWebsite.Models.Carts;
using PizzaWebsite.Models.Shop;

namespace PizzaWebsite.Models.ViewModelServices
{
    public class PreviousOrderServices
    {
        public async Task<PreviousOrderViewModel> CreateViewModelAsync(bool loadCartItems, CustomerOrder customerOrder,
            DeliveryInfo deliveryInfo, PizzaDatabase pizzaDb, IEnumerable<int> quantityList)
        {
            CartViewModel cartVm = new CartViewModel();
            if (loadCartItems)
            {
                CartServices cartServices = new CartServices();
                cartVm = await cartServices.CreateViewModelAsync(customerOrder.CartId, pizzaDb, quantityList);
            }

            return new PreviousOrderViewModel
            {
                Id = customerOrder.Id,
                DateOfOrder =
                    $"{customerOrder.DateOfOrder.ToShortDateString()} {customerOrder.DateOfOrder.ToShortTimeString()}",
                OrderTotal = customerOrder.OrderTotal.ToString("C", CultureInfo.CurrentCulture),
                OrderType = customerOrder.GetOrderType(),
                CartVm = cartVm
            };
        }
    }
}