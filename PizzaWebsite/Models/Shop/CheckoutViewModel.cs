using PizzaWebsite.Models.Attributes;
using PizzaWebsite.Models.Carts;
using PizzaWebsite.Models.Geography;
using PizzaWebsite.Models.Manage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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

        [Required]
        [Display(Name = "Address Name")]
        [MaxLength(50)]
        public string DeliveryAddressName { get; set; }

        [Required]
        [Display(Name = "Address Type")]
        public string SelectedDeliveryAddressType { get; set; }

        [Required]
        [Display(Name = "Street Address")]
        [MaxLength(50)]
        public string DeliveryStreetAddress { get; set; }

        [Required]
        [Display(Name = "City")]
        [MaxLength(50)]
        public string DeliveryCity { get; set; }

        [Required]
        [Display(Name = "State")]
        public string SelectedDeliveryState { get; set; }

        [Required]
        [Display(Name = "Zip Code")]
        [ZipCode]
        public string DeliveryZipCode { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [PhoneNumber]
        public string DeliveryPhoneNumber { get; set; }

        public bool IsDelivery()
        {
            return SelectedOrderType == "Delivery";
        }
    }
}