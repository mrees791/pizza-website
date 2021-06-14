using DataLibrary.Models;
using DataLibrary.Models.JoinLists;
using DataLibrary.Models.QuerySearches;
using DataLibrary.Models.Tables;
using DataLibrary.Models.Utility;
using PizzaWebsite.Models.Attributes;
using PizzaWebsite.Models.Carts;
using PizzaWebsite.Models.Geography;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PizzaWebsite.Models.Shop
{
    public class CheckoutViewModel
    {
        [Required]
        [Display(Name = "Order Type")]
        public string SelectedOrderType { get; set; }
        [Required]
        [Display(Name = "Store")]
        public int SelectedStoreLocationId { get; set; }
        // Delivery Info
        [Display(Name = "Save New Address")]
        public bool SaveNewDeliveryAddress { get; set; }

        [Display(Name = "Delivery Address")]
        public int SelectedDeliveryAddressId { get; set; }
        [RequiredIfDelivery("A delivery address name is required.")]
        [Display(Name = "Address Name")]
        [MaxLength(50)]
        public string DeliveryAddressName { get; set; }
        [RequiredIfDelivery("A delivery address type is required.")]
        [Display(Name = "Address Type")]
        public string SelectedDeliveryAddressType { get; set; }
        [RequiredIfDelivery("A delivery street is required.")]
        [Display(Name = "Street Address")]
        [MaxLength(50)]
        public string DeliveryStreetAddress { get; set; }
        [RequiredIfDelivery("A delivery city is required.")]
        [Display(Name = "City")]
        [MaxLength(50)]
        public string DeliveryCity { get; set; }
        [RequiredIfDelivery("A delivery state is required.")]
        [Display(Name = "State")]
        public string SelectedDeliveryState { get; set; }
        [RequiredIfDelivery("A delivery zip code is required.")]
        [ZipCode]
        [Display(Name = "Zip Code")]
        public string DeliveryZipCode { get; set; }
        [RequiredIfDelivery("A delivery phone number is required.")]
        [PhoneNumber]
        [Display(Name = "Phone Number")]
        public string DeliveryPhoneNumber { get; set; }
        public CartViewModel CartVm { get; set; }
        public List<string> OrderTypeList { get; set; }
        public List<SelectListItem> DeliveryAddressSelectList { get; set; }
        public List<State> DeliveryStateSelectList { get; set; }
        public List<string> DeliveryAddressTypeSelectList { get; set; }
        public List<SelectListItem> StoreLocationSelectList { get; set; }

        public bool IsDelivery()
        {
            return SelectedOrderType == "Delivery";
        }

        public bool IsNewDeliveryAddress()
        {
            return SelectedDeliveryAddressId == 0;
        }

        public async Task InitializeAsync(bool isPostBack, SiteUser user, PizzaDatabase pizzaDb)
        {
            if (!isPostBack)
            {
                SaveNewDeliveryAddress = true;
            }

            await pizzaDb.Commands.CheckoutCartAsync(user);

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

            CartVm = new CartViewModel()
            {
                CartItemList = new List<CartItemViewModel>()
            };

            OrderTypeList = ListUtility.CreateCustomerOrderTypeList();
            DeliveryStateSelectList = StateListCreator.CreateStateList();
            DeliveryAddressTypeSelectList = ListUtility.CreateDeliveryAddressTypeList();
            DeliveryAddressSelectList = deliveryAddressSelectList;
            StoreLocationSelectList = storeLocationSelectList;

            await CartVm.InitializeAsync(updatedUser.ConfirmOrderCartId, pizzaDb);
        }
    }
}