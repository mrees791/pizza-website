using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.Menu.Pizzas.Ingredients
{
    public class Sauce : PizzaIngredient
    {
        [DataType(DataType.Currency)]
        [Display(Name = "Light Sauce Price")]
        [Required(ErrorMessage = "You must set a price.")]
        public decimal LightPrice { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Regular Sauce Price")]
        [Required(ErrorMessage = "You must set a price.")]
        public decimal RegularPrice { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Extra Sauce Price")]
        [Required(ErrorMessage = "You must set a price.")]
        public decimal ExtraPrice { get; set; }
    }
}