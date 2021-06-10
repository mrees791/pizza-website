using DataLibrary.Models;
using DataLibrary.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PizzaWebsite.Models.ManageStores
{
    public class RemoveEmployeeFromRosterViewModel
    {
        public int EmployeeLocationId { get; set; }
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public string EmployeeId { get; set; }
    }
}