using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models
{
    public abstract class MenuItemWithIconModel
    {
        public bool HasMenuIcon { get; set; }

        [Display(Name = "Menu Icon")]
        public string MenuIconUrl { get; set; }
    }
}