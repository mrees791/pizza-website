using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using DataLibrary.Models;
using DataLibrary.Models.Tables;
using PizzaWebsite.Models.Carts;
using PizzaWebsite.Models.Shop;

namespace PizzaWebsite.Models.ViewModelServices
{
    public class CustomerOrderServices
    {
        public async Task<CustomerOrderViewModel> CreateViewModelAsync(bool loadCartItems, CustomerOrder customerOrder,
            DeliveryInfo deliveryInfo, PizzaDatabase pizzaDb, IEnumerable<int> quantityList)
        {
            CartViewModel cartVm = new CartViewModel();
            if (loadCartItems)
            {
                CartServices cartServices = new CartServices();
                cartVm = await cartServices.CreateViewModelAsync(customerOrder.CartId, pizzaDb, quantityList);
            }

            DeliveryInfoViewModel deliveryInfoVm = null;

            if (deliveryInfo != null)
            {
                deliveryInfoVm = new DeliveryInfoViewModel()
                {
                    AddressName = deliveryInfo.DeliveryAddressName,
                    AddressType = deliveryInfo.DeliveryAddressType,
                    City = deliveryInfo.DeliveryCity,
                    PhoneNumber = deliveryInfo.DeliveryPhoneNumber,
                    State = deliveryInfo.DeliveryState,
                    StreetAddress = deliveryInfo.DeliveryStreetAddress,
                    ZipCode = deliveryInfo.DeliveryZipCode
                };
            }

            return new CustomerOrderViewModel
            {
                Id = customerOrder.Id,
                DateOrderPlaced =
                    $"{customerOrder.DateOrderPlaced.ToShortDateString()} {customerOrder.DateOrderPlaced.ToShortTimeString()}",
                OrderTotal = customerOrder.OrderTotal.ToString("C", CultureInfo.CurrentCulture),
                OrderType = customerOrder.GetOrderType(),
                CartVm = cartVm,
                DeliveryInfoVm = deliveryInfoVm,
                StoreId = customerOrder.StoreId,
                UserId = customerOrder.UserId
            };
        }
    }
}