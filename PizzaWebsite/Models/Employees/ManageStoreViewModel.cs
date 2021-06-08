using PizzaWebsite.Models.Attributes;
using PizzaWebsite.Models.Geography;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.Employees
{
    public class ManageStoreViewModel
    {
        public ManageStoreViewModel()
        {
            StateList = StateListCreator.CreateStateList();
        }

        public List<State> StateList { get; set; }
        public int Id { get; set; }

        [Required]
        [Display(Name = "Store Name")]
        [MaxLength(50)]
        public string Name { get; set; }

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
        [ZipCode]
        public string ZipCode { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [PhoneNumber]
        public string PhoneNumber { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActiveLocation { get; set; }

        public bool IsNewRecord()
        {
            return Id == 0;
        }
    }
}