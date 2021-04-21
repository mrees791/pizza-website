using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.PizzaBuilders
{
    public class PizzaBuilderViewModel
    {
        public PizzaBuilderViewModel()
        {
            SauceList = new Dictionary<int, string>();
            CheeseList = new Dictionary<int, string>();
            CrustFlavorList = new Dictionary<int, string>();
            ToppingList = new List<PizzaToppingViewModel>();
        }

        public Dictionary<int, string> SauceList { get; set; }
        [Display(Name = "Sauce")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a sauce.")]
        public int SelectedSauceId { get; set; }
        public List<string> SauceAmountList { get; set; }
        [Display(Name = "Amount")]
        public string SelectedSauceAmount { get; set; }
        public Dictionary<int, string> CheeseList { get; set; }
        [Display(Name = "Cheese")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a cheese.")]
        public int SelectedCheeseId { get; set; }
        public List<string> CheeseAmountList { get; set; }
        [Display(Name = "Amount")]
        public string SelectedCheeseAmount { get; set; }
        public Dictionary<int, string> CrustFlavorList { get; set; }
        [Display(Name = "Crust Flavor")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a crust flavor.")]
        public int SelectedCrustFlavorId { get; set; }
        public List<PizzaToppingViewModel> ToppingList { get; set; }
    }
}