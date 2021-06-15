using System.ComponentModel.DataAnnotations;
using PizzaWebsite.Models.Attributes;

namespace PizzaWebsite.Models.ManageStores
{
    public class AddEmployeeToRosterViewModel
    {
        [ValidEmployeeId]
        [Display(Name = "Employee ID")]
        public string EmployeeId { get; set; }

        public int StoreId { get; set; }
        public string StoreName { get; set; }
    }
}