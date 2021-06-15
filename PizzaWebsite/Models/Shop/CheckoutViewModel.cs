using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using PizzaWebsite.Models.Carts;
using PizzaWebsite.Models.Shop.CheckoutAttributes;

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

        [ZipCodeRequiredIfDelivery]
        [Display(Name = "Zip Code")]
        public string DeliveryZipCode { get; set; }

        [PhoneNumberRequiredIfDelivery]
        [Display(Name = "Phone Number")]
        public string DeliveryPhoneNumber { get; set; }

        public CartViewModel CartVm { get; set; }
        public IEnumerable<string> OrderTypeList { get; set; }
        public IEnumerable<string> DeliveryAddressTypeList { get; set; }
        public IEnumerable<string> DeliveryStateList { get; set; }
        public IEnumerable<SelectListItem> StoreLocationSelectList { get; set; }
        public IEnumerable<SelectListItem> DeliveryAddressSelectList { get; set; }

        public bool IsDelivery()
        {
            return SelectedOrderType == "Delivery";
        }

        public bool IsNewDeliveryAddress()
        {
            return SelectedDeliveryAddressId == 0;
        }
    }
}