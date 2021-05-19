using DataLibrary.Models;
using DataLibrary.Models.Joins;
using DataLibrary.Models.QuerySearches;
using DataLibrary.Models.Tables;
using DataLibrary.Models.Utility;
using PizzaWebsite.Models.Attributes;
using PizzaWebsite.Models.Carts;
using PizzaWebsite.Models.Geography;
using PizzaWebsite.Models.Manage;
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
        public CartViewModel Cart { get; set; }
        public List<string> OrderTypeList { get; set; }
        [Required]
        [Display(Name = "Order Type")]
        public string SelectedOrderType { get; set; }

        [Required]
        [Display(Name = "Store")]
        public int SelectedStoreLocationId { get; set; }
        public List<SelectListItem> StoreLocationSelectList { get; set; }

        // Cost summary
        public string OrderSubtotal { get; set; }
        public string OrderTax { get; set; }
        public string OrderTotal { get; set; }

        // Delivery Info
        [Display(Name = "Delivery Address")]
        public int SelectedDeliveryAddressId { get; set; }
        public List<SelectListItem> DeliveryAddressSelectList { get; set; }
        public List<State> DeliveryStateSelectList { get; set; }
        public List<string> DeliveryAddressTypeSelectList { get; set; }

        [Display(Name = "Save New Address")]
        public bool SaveNewDeliveryAddress { get; set; }

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

        public bool IsDelivery()
        {
            return SelectedOrderType == "Delivery";
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
            IEnumerable<StoreLocation> storeLocationList = await pizzaDb.GetListAsync<StoreLocation>("Name", storeSearch);
            IEnumerable<DeliveryAddress> deliveryAddressList = await pizzaDb.GetListAsync<DeliveryAddress>("Name", addressSearch);
            IEnumerable<CartItemJoin> cartItemJoinList = await pizzaDb.GetJoinedCartItemListAsync(updatedUser.ConfirmOrderCartId);

            List<SelectListItem> deliveryAddressSelectList = new List<SelectListItem>();
            List<SelectListItem> storeLocationSelectList = new List<SelectListItem>();

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

            CostSummary costSummary = new CostSummary(cartItemJoinList);

            Cart = new CartViewModel()
            {
                CartItemList = new List<CartItemViewModel>()
            };

            OrderTypeList = ListUtility.CreateCustomerOrderTypeList();
            DeliveryStateSelectList = StateListCreator.CreateStateList();
            DeliveryAddressTypeSelectList = ListUtility.CreateDeliveryAddressTypeList();
            DeliveryAddressSelectList = deliveryAddressSelectList;
            StoreLocationSelectList = storeLocationSelectList;
            OrderSubtotal = costSummary.Subtotal.ToString("C", CultureInfo.CurrentCulture);
            OrderTax = costSummary.Tax.ToString("C", CultureInfo.CurrentCulture);
            OrderTotal = costSummary.Total.ToString("C", CultureInfo.CurrentCulture);

            await Cart.LoadCartItems(updatedUser.ConfirmOrderCartId, pizzaDb);
        }
    }
}