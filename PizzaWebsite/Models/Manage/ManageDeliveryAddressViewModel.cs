using DataLibrary.Models.Utility;
using PizzaWebsite.Models.Geography;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.Manage
{
    public class ManageDeliveryAddressViewModel
    {
        public ManageDeliveryAddressViewModel()
        {
            StateList = StateListCreator.CreateStateList();
            AddressTypeList = ListUtility.CreateDeliveryAddressTypeList();
        }

        public List<State> StateList { get; set; }
        public List<string> AddressTypeList { get; set; }

        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Address Type")]
        public string SelectedAddressType { get; set; }

        [Required]
        [Display(Name = "Street Address")]
        [MaxLength(50)]
        public string StreetAddress { get; set; }

        [Required]
        [Display(Name = "City")]
        [MaxLength(50)]
        public string City { get; set; }

        [Required]
        [Display(Name = "State")]
        public string SelectedState { get; set; }

        [Required]
        [Display(Name = "Zip Code")]
        [StringLength(5, MinimumLength = 5)]
        public string ZipCode { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [StringLength(10, MinimumLength = 10)]
        public string PhoneNumber { get; set; }

        public bool IsNewRecord()
        {
            return Id == 0;
        }
    }
}