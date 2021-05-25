using DataLibrary.Models;
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
        [Display(Name = "Sauce Amount")]
        public string SelectedSauceAmount { get; set; }
        public Dictionary<int, string> CheeseList { get; set; }
        [Display(Name = "Cheese")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a cheese.")]
        public int SelectedCheeseId { get; set; }
        public List<string> CheeseAmountList { get; set; }
        [Display(Name = "Cheese Amount")]
        public string SelectedCheeseAmount { get; set; }
        public Dictionary<int, string> CrustFlavorList { get; set; }
        [Display(Name = "Crust Flavor")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a crust flavor.")]
        public int SelectedCrustFlavorId { get; set; }
        public List<PizzaToppingViewModel> ToppingList { get; set; }

        protected virtual async Task LoadBuilderListsAsync(PizzaDatabase pizzaDb, List<PizzaTopping> toppings)
        {
            MenuItemSearch menuItemSearch = new MenuItemSearch()
            {
                AvailableForPurchase = true
            };

            IEnumerable<MenuPizzaToppingType> toppingTypeList = await pizzaDb.GetListAsync<MenuPizzaToppingType>("SortOrder", SortOrder.Ascending, menuItemSearch);
            IEnumerable<MenuPizzaCrustFlavor> crustFlavorList = await pizzaDb.GetListAsync<MenuPizzaCrustFlavor>("SortOrder", SortOrder.Ascending, menuItemSearch);
            IEnumerable<MenuPizzaSauce> pizzaSauceList = await pizzaDb.GetListAsync<MenuPizzaSauce>("SortOrder", SortOrder.Ascending, menuItemSearch);
            IEnumerable<MenuPizzaCheese> pizzaCheeseList = await pizzaDb.GetListAsync<MenuPizzaCheese>("SortOrder", SortOrder.Ascending, menuItemSearch);
            SauceAmountList = ListUtility.GetSauceAmountList();
            CheeseAmountList = ListUtility.GetCheeseAmountList();

            foreach (MenuPizzaCrustFlavor crustFlavor in crustFlavorList)
            {
                CrustFlavorList.Add(crustFlavor.Id, crustFlavor.Name);
            }

            foreach (MenuPizzaSauce sauce in pizzaSauceList)
            {
                SauceList.Add(sauce.Id, sauce.Name);
            }

            foreach (MenuPizzaCheese cheese in pizzaCheeseList)
            {
                CheeseList.Add(cheese.Id, cheese.Name);
            }

            // Create view models for toppings
            for (int iTopping = 0; iTopping < toppingTypeList.Count(); iTopping++)
            {
                MenuPizzaToppingType toppingType = toppingTypeList.ElementAt(iTopping);

                PizzaTopping currentTopping = toppings.Where(t => t.ToppingTypeId == toppingType.Id).FirstOrDefault();

                if (currentTopping == null)
                {
                    currentTopping = new PizzaTopping()
                    {
                        ToppingTypeId = toppingType.Id,
                        ToppingHalf = "Whole",
                        ToppingAmount = "None"
                    };
                }

                PizzaToppingViewModel toppingModel = new PizzaToppingViewModel()
                {
                    ListIndex = iTopping,
                    Category = toppingType.CategoryName,
                    Id = toppingType.Id,
                    Name = toppingType.Name,
                    AmountList = ListUtility.GetToppingAmountList(),
                    ToppingHalfList = ListUtility.GetToppingHalfList(),
                    SelectedAmount = currentTopping.ToppingAmount,
                    SelectedToppingHalf = currentTopping.ToppingHalf
                };
                
                ToppingList.Add(toppingModel);
            }
        }
    }
}