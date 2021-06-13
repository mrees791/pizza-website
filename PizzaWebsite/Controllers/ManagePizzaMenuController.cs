using DataLibrary.Models;
using DataLibrary.Models.Builders;
using DataLibrary.Models.QueryFilters;
using DataLibrary.Models.QuerySearches;
using DataLibrary.Models.Tables;
using DataLibrary.Models.Utility;
using PizzaWebsite.Controllers.BaseControllers;
using PizzaWebsite.Models;
using PizzaWebsite.Models.ManageMenus;
using PizzaWebsite.Models.PizzaBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PizzaWebsite.Controllers
{
    [Authorize(Roles = "Admin,Executive")]
    public class ManagePizzaMenuController : BaseManageMenuController<MenuPizza, ManageMenuPizzaViewModel>
    {
        public async Task<ActionResult> Index(int? page, int? rowsPerPage, string name)
        {
            ValidatePageQuery(ref page, ref rowsPerPage, 10);
            MenuPizzaFilter searchFilter = new MenuPizzaFilter()
            {
                PizzaName = name
            };
            return await Index(page.Value, rowsPerPage.Value, "PizzaName", searchFilter);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(ManageMenuPizzaViewModel model)
        {
            return await Add(model, model.Name);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ManageMenuPizzaViewModel model)
        {
            return await Edit(model, model.Name);
        }

        protected override async Task<ManageMenuPizzaViewModel> RecordToViewModelAsync(MenuPizza record)
        {
            MenuPizzaBuilder pizzaBuilder = new MenuPizzaBuilder();
            await pizzaBuilder.InitializeAsync(new MenuItemSearch() { AvailableForPurchase = true }, PizzaDb);
            Dictionary<int, string> cheeseDictionary = new Dictionary<int, string>();
            Dictionary<int, string> crustFlavorDictionary = new Dictionary<int, string>();
            Dictionary<int, string> sauceDictionary = new Dictionary<int, string>();
            List<PizzaTopping> toppingList = new List<PizzaTopping>();
            foreach (MenuPizzaCheese cheese in pizzaBuilder.CheeseList)
            {
                cheeseDictionary.Add(cheese.Id, cheese.Name);
            }
            foreach (MenuPizzaCrustFlavor crustFlavor in pizzaBuilder.CrustFlavorList)
            {
                crustFlavorDictionary.Add(crustFlavor.Id, crustFlavor.Name);
            }
            foreach (MenuPizzaSauce sauce in pizzaBuilder.SauceList)
            {
                sauceDictionary.Add(sauce.Id, sauce.Name);
            }
            foreach (MenuPizzaTopping menuTopping in record.ToppingList)
            {
                PizzaTopping topping = new PizzaTopping()
                {
                    ToppingTypeId = menuTopping.MenuPizzaToppingTypeId,
                    ToppingAmount = menuTopping.ToppingAmount,
                    ToppingHalf = menuTopping.ToppingHalf
                };
                toppingList.Add(topping);
            }
            List<PizzaToppingViewModel> toppingVmList = PizzaBuilderManager.CreateToppingViewModelList(toppingList, pizzaBuilder.ToppingTypeList);
            return new ManageMenuPizzaViewModel()
            {
                Id = record.Id,
                Name = record.PizzaName,
                AvailableForPurchase = record.AvailableForPurchase,
                SelectedCategory = record.CategoryName,
                Description = record.Description,
                SelectedCheeseAmount = record.CheeseAmount,
                SelectedCheeseId = record.MenuPizzaCheeseId,
                SelectedCrustFlavorId = record.MenuPizzaCrustFlavorId,
                SelectedSauceAmount = record.SauceAmount,
                SelectedSauceId = record.MenuPizzaSauceId,
                SortOrder = record.SortOrder,
                CategoryList = pizzaBuilder.CategoryList,
                CheeseAmountList = pizzaBuilder.CheeseAmountList,
                SauceAmountList = pizzaBuilder.SauceAmountList,
                CheeseDictionary = cheeseDictionary,
                CrustFlavorDictionary = crustFlavorDictionary,
                SauceDictionary = sauceDictionary,
                MeatToppingVmList = toppingVmList.Where(t => t.Category == "Meats"),
                VeggieToppingVmList = toppingVmList.Where(t => t.Category == "Veggie")
            };
        }

        protected override MenuPizza ViewModelToRecord(ManageMenuPizzaViewModel model)
        {
            return new MenuPizza()
            {
                Id = model.Id,
                AvailableForPurchase = model.AvailableForPurchase,
                CategoryName = model.SelectedCategory,
                PizzaName = model.Name,
                CheeseAmount = model.SelectedCheeseAmount,
                Description = model.Description,
                MenuPizzaCheeseId = model.SelectedCheeseId,
                MenuPizzaCrustFlavorId = model.SelectedCrustFlavorId,
                MenuPizzaSauceId = model.SelectedSauceId,
                SauceAmount = model.SelectedSauceAmount,
                SortOrder = model.SortOrder,
                ToppingList = GetToppingRecordsFromViewModel(model)
            };
        }

        private List<MenuPizzaTopping> GetToppingRecordsFromViewModel(ManageMenuPizzaViewModel model)
        {
            List<MenuPizzaTopping> toppingRecordList = new List<MenuPizzaTopping>();
            IEnumerable<PizzaToppingViewModel> toppingVmList = model.GetToppingVmList();
            foreach (PizzaToppingViewModel toppingVm in toppingVmList)
            {
                if (toppingVm.SelectedAmount != "None")
                {
                    toppingRecordList.Add(new MenuPizzaTopping()
                    {
                        MenuPizzaId = model.Id,
                        MenuPizzaToppingTypeId = toppingVm.Id,
                        ToppingAmount = toppingVm.SelectedAmount,
                        ToppingHalf = toppingVm.SelectedToppingHalf
                    });
                }
            }
            return toppingRecordList;
        }
    }
}