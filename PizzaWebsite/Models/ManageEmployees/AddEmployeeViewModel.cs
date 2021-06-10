using DataLibrary.Models;
using DataLibrary.Models.Tables;
using PizzaWebsite.Models.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

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