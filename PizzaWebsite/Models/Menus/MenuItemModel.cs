using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.Menus
{
    /// <summary>
    /// Provides the base model for all menu item models.
    /// </summary>
    public abstract class MenuItemModel
    {
        public bool IsNewRecord { get; set; }

        public int Id { get; set; }

        [Display(Name = "Name")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Available For Purchase")]
        public bool AvailableForPurchase { get; set; }
    }
}