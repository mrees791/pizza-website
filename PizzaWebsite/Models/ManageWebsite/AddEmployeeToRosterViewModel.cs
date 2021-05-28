using PizzaWebsite.Models.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.ManageWebsite
{
    public class AddEmployeeToRosterViewModel
    {
        public int StoreId { get; set; }

        [Display(Name = "Employee ID")]
        [StringLength(256, ErrorMessage = "Employee ID cannot be longer than 256 characters.")]
        [ValidEmployeeId]
        [Required]
        public string Id { get; set; }
    }
}