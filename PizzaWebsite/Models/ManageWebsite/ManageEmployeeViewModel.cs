using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.ManageWebsite
{
    public class ManageEmployeeViewModel
    {
        [Display(Name = "Employee ID")]
        public string Id { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Display(Name = "Currently Employed")]
        public bool CurrentlyEmployed { get; set; }

        [Display(Name = "Is Manager")]
        public bool IsManager { get; set; }
    }
}