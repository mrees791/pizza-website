using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.Menus.Pizzas.Ingredients
{
    public abstract class PizzaIngredientModel : MenuItemModel
    {
        [Display(Name = "Name")]
        [Required(ErrorMessage = "You must name the ingredient.")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        public bool HasPizzaBuilderIcon { get; set; }

        [Display(Name = "Pizza Builder Icon")]
        public string PizzaBuilderIconUrl { get; set; }
    }
}