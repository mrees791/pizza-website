using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.Menu.Pizzas.Ingredients
{
    public class Topping : PizzaIngredient
    {
        [DataType(DataType.Currency)]
        [Display(Name = "Light Topping Price")]
        [Required(ErrorMessage = "You must set a price.")]
        public decimal LightPrice { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Regular Topping Price")]
        [Required(ErrorMessage = "You must set a price.")]
        public decimal RegularPrice { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Extra Topping Price")]
        [Required(ErrorMessage = "You must set a price.")]
        public decimal ExtraPrice { get; set; }

        [Display(Name = "Topping Type")]
        [Required(ErrorMessage = "Topping type is required.")]
        public ToppingType ToppingType { get; set; }
    }
}