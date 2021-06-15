using System.Collections.Generic;

namespace PizzaWebsite.Models.ManageEmployees
{
    public class ViewEmployeeLocationsViewModel
    {
        public string EmployeeId { get; set; }
        public IEnumerable<EmployeeLocationViewModel> EmployeeLocationVmList { get; set; }
        public EmployeeLocationViewModel ItemViewModel { get; set; }
    }
}