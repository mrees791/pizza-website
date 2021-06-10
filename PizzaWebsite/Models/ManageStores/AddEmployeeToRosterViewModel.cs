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
        [Display(Name = "Employee ID")]
        [StringLength(256, ErrorMessage = "Employee ID cannot be longer than 256 characters.")]
        [ValidEmployeeId]
        [Required]
        public string EmployeeId { get; set; }
        public int StoreId { get; set; }
        public string StoreName { get; set; }
    }
}