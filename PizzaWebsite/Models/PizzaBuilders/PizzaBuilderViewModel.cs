using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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

        public IEnumerable<PizzaToppingViewModel> ToppingVmList { get; set; }
        public IEnumerable<string> SauceAmountList { get; set; }
        public IEnumerable<string> CheeseAmountList { get; set; }
        public Dictionary<int, string> SauceDictionary { get; set; }
        public Dictionary<int, string> CheeseDictionary { get; set; }
        public Dictionary<int, string> CrustFlavorDictionary { get; set; }

        public IEnumerable<PizzaToppingViewModel> GetMeatsToppingVmList()
        {
            return ToppingVmList.Where(t => t.Category == "Meats");
        }

        public IEnumerable<PizzaToppingViewModel> GetVeggieToppingVmList()
        {
            return ToppingVmList.Where(t => t.Category == "Veggie");
        }
    }
}