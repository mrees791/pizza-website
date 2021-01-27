using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.Menu
{
    /// <summary>
    /// Provides the base model for all menu item models.
    /// </summary>
    public abstract class MenuItemModel
    {
        public bool IsNewRecord { get; set; }

        [Display(Name = "Available For Purchase")]
        public bool AvailableForPurchase { get; set; }

        public bool HasMenuIcon { get; set; }

        [Display(Name = "Menu Icon")]
        public string MenuIconUrl { get; set; }
    }
}