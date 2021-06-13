using DataLibrary.Models;
using DataLibrary.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PizzaWebsite.Models.Employees
{
    public class EmployeeIndexViewModel
    {
        public string EmployeeId { get; set; }
        public bool AuthorizedToManageMenu { get; set; }
        public bool AuthorizedToManageStores { get; set; }
        public bool AuthorizedToManageUsers { get; set; }
        public bool AuthorizedToManageEmployees { get; set; }
    }
}