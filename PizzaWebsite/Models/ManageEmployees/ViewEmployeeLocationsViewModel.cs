using DataLibrary.Models;
using DataLibrary.Models.JoinLists;
using DataLibrary.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PizzaWebsite.Models.ManageEmployees
{
    public class ViewEmployeeLocationsViewModel
    {
        public string EmployeeId { get; set; }
        public IEnumerable<EmployeeLocationViewModel> EmployeeLocationVmList { get; set; }
        public EmployeeLocationViewModel ItemViewModel { get; set; }
    }
}