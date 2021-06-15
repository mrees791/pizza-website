using System.ComponentModel.DataAnnotations;

namespace PizzaWebsite.Models.ManageEmployees
{
    public class ManageEmployeeViewModel
    {
        [Display(Name = "Employee ID")]
        public string Id { get; set; }

        [Display(Name = "User ID")]
        public string UserId { get; set; }

        [Display(Name = "Is Manager")]
        public bool IsManager { get; set; }
    }
}