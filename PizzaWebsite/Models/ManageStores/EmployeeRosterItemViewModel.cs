using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.ManageStores
{
    public class EmployeeRosterItemViewModel
    {
        public string EmployeeId { get; set; }
        public int EmployeeLocationId { get; set; }
        public string UserId { get; set; }
        public bool IsManager { get; set; }
    }
}