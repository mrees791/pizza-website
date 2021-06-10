using DataLibrary.Models;
using DataLibrary.Models.Tables;
using PizzaWebsite.Models.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

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