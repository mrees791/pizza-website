using System.ComponentModel.DataAnnotations;
using PizzaWebsite.Models.Attributes;

namespace PizzaWebsite.Models.ManageEmployees
{
    public class AddEmployeeViewModel
    {
        [ValidEmployeeId]
        [Display(Name = "Employee ID")]
        public string Id { get; set; }

        [Display(Name = "User Id")]
        [Required]
        public string UserId { get; set; }

        [Display(Name = "Is Manager")]
        public bool IsManager { get; set; }
    }
}