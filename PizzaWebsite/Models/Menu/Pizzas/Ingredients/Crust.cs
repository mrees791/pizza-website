using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.Menu.Pizzas.Ingredients
{
    public class Crust : PizzaIngredient
    {
        [DataType(DataType.Currency)]
        [Display(Name = "Small Crust Price")]
        [Required(ErrorMessage = "You must set a price.")]
        [Range(minimum:0.01,maximum:double.MaxValue,ErrorMessage = "You must set a valid price.")]
        public decimal SmallPrice { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Medium Crust Price")]
        [Required(ErrorMessage = "You must set a price.")]
        [Range(minimum: 0.01, maximum: double.MaxValue, ErrorMessage = "You must set a valid price.")]
        public decimal MediumPrice { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Large Crust Price")]
        [Required(ErrorMessage = "You must set a price.")]
        [Range(minimum: 0.01, maximum: double.MaxValue, ErrorMessage = "You must set a valid price.")]
        public decimal LargePrice { get; set; }
    }
}