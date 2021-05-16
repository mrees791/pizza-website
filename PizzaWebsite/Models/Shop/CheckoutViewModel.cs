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
        public bool AddNewDeliveryAddress { get; set; }
        public List<State> DeliveryStateSelectList { get; set; }
        public List<string> DeliveryAddressTypeSelectList { get; set; }

        //[Required]
        [Display(Name = "Address Name")]
        [MaxLength(50)]
        public string DeliveryAddressName { get; set; }

        //[Required]
        [Display(Name = "Address Type")]
        public string SelectedDeliveryAddressType { get; set; }

        //[Required]
        [Display(Name = "Street Address")]
        [MaxLength(50)]
        public string DeliveryStreetAddress { get; set; }

        //[Required]
        [Display(Name = "City")]
        [MaxLength(50)]
        public string DeliveryCity { get; set; }

        //[Required]
        [Display(Name = "State")]
        public string SelectedDeliveryState { get; set; }

        //[Required]
        //[Display(Name = "Zip Code")]
        //[ZipCode]
        public string DeliveryZipCode { get; set; }

        //[Required]
        //[Display(Name = "Phone Number")]
        //[PhoneNumber]
        [RequiredIfDelivery("A phone number is required.")]
        public string DeliveryPhoneNumber { get; set; }

        public bool IsDelivery()
        {
            return SelectedOrderType == "Delivery";
        }

        public async Task InitializeAsync(SiteUser user, PizzaDatabase pizzaDb)
        {
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