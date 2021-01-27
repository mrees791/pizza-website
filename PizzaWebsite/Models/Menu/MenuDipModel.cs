using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.Menu
{
    public class MenuDipModel : MenuItemModel
    {
        [Display(Name = "Name")]
        [Required(ErrorMessage = "You must name the dip.")]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Price")]
        [Required(ErrorMessage = "You must set a price.")]
        public decimal Price { get; set; }

        [Display(Name = "Item Details")]
        public string ItemDetails { get; set; }
    }
}