using System.ComponentModel.DataAnnotations;

namespace PizzaWebsite.Models.ManageEmployees
{
    public class EmployeeLocationViewModel
    {
        [Display(Name = "Store Name")]
        public string Name { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "State")]
        public string State { get; set; }

        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActiveLocation { get; set; }
    }
}