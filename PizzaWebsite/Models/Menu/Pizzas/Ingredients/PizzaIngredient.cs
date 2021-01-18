using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.Menu.Pizzas.Ingredients
{
    public abstract class PizzaIngredient
    {
        [Display(Name = "Available For Purchase")]
        public bool AvailableForPurchase { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "You must name the ingredient.")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        public bool HasMenuIcon { get; set; }

        [Display(Name = "Menu Icon")]
        public string MenuIconUrl { get; set; }

        public bool HasPizzaBuilderIcon { get; set; }

        [Display(Name = "Pizza Builder Icon")]
        public string PizzaBuilderIconUrl { get; set; }
    }
}