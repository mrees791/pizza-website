using DataLibrary.Models;
using DataLibrary.Models.QuerySearches;
using DataLibrary.Models.Tables;
using PizzaWebsite.Models.Geography;
using PizzaWebsite.Models.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PizzaWebsite.Models.ViewModelServices
{
    public class CheckoutServices
    {
        public async Task<CheckoutViewModel> CreateViewModelAsync(SiteUser user, PizzaDatabase pizzaDb, IEnumerable<int> quantityList, IEnumerable<State> stateList,
            IEnumerable<string> customerOrderTypeList, IEnumerable<string> deliveryAddressTypeList)
        {
            SiteUser updatedUser = await pizzaDb.GetAsync<SiteUser>(user.Id);
            StoreLocationSearch storeSearch = new StoreLocationSearch()
            {
                IsActiveLocation = true
            };
            DeliveryAddressSearch addressSearch = new DeliveryAddressSearch()
            {
                UserId = updatedUser.Id
            };
            IEnumerable<StoreLocation> storeLocationList = await pizzaDb.GetListAsync<StoreLocation>("Name", SortOrder.Ascending, storeSearch);
            IEnumerable<DeliveryAddress> deliveryAddressList = await pizzaDb.GetListAsync<DeliveryAddress>("Name", SortOrder.Ascending, addressSearch);
            List<SelectListItem> deliveryAddressSelectList = new List<SelectListItem>();
            List<SelectListItem> storeLocationSelectList = new List<SelectListItem>();
            deliveryAddressSelectList.Add(new SelectListItem()
            {
                Text = "New Address",
                Value = "0"
            });
            foreach (DeliveryAddress deliveryAddress in deliveryAddressList)
            {
                deliveryAddressSelectList.Add(new SelectListItem()
                {
                    Text = deliveryAddress.Name,
                    Value = deliveryAddress.Id.ToString()
                });
            }
            foreach (StoreLocation storeLocation in storeLocationList)
            {
                storeLocationSelectList.Add(new SelectListItem()
                {
                    Text = storeLocation.Name,
                    Value = storeLocation.Id.ToString()
                });
            }
            CartServices cartServices = new CartServices();
            return new CheckoutViewModel()
            {
                OrderTypeList = customerOrderTypeList,
                DeliveryStateList = stateList.Select(s => s.Abbreviation),
                DeliveryAddressTypeList = deliveryAddressTypeList,
                DeliveryAddressSelectList = deliveryAddressSelectList,
                StoreLocationSelectList = storeLocationSelectList,
                SaveNewDeliveryAddress = true,
                CartVm = await cartServices.CreateViewModelAsync(updatedUser.ConfirmOrderCartId, pizzaDb, quantityList)
            };
        }
    }
}