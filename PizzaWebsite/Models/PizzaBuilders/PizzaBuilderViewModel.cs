﻿using DataLibrary.Models;
using DataLibrary.Models.QuerySearches;
using DataLibrary.Models.Tables;
using DataLibrary.Models.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PizzaWebsite.Models.PizzaBuilders
{
    public abstract class PizzaBuilderViewModel
    {
        [Display(Name = "Sauce")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a sauce.")]
        public int SelectedSauceId { get; set; }
        [Display(Name = "Sauce Amount")]
        public string SelectedSauceAmount { get; set; }
        [Display(Name = "Cheese")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a cheese.")]
        public int SelectedCheeseId { get; set; }
        [Display(Name = "Cheese Amount")]
        public string SelectedCheeseAmount { get; set; }
        [Display(Name = "Crust Flavor")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a crust flavor.")]
        public int SelectedCrustFlavorId { get; set; }
        public List<PizzaToppingViewModel> ToppingList { get; set; }
    }
}