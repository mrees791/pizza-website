using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PizzaWebsite.Models.Manage
{
    public class ManagePizzaIngredientsModel
    {
        [Required]
        [Display(Name = "Ingredient")]
        public string SelectedIngredient { get; set; }

        public IEnumerable<SelectListItem> Ingredients { get; set; }
    }
}