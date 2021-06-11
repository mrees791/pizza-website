using DataLibrary.Models;
using DataLibrary.Models.QueryFilters;
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
            ManageMenuPizzaViewModel model = new ManageMenuPizzaViewModel();
            await model.CreateFromRecordAsync(PizzaDb, record);
            return model;
        }

        private void AddToppingsToRecord(MenuPizza record, List<PizzaToppingViewModel> toppings)
        {
            foreach (PizzaToppingViewModel topping in toppings)
            {
                if (topping.SelectedAmount != "None")
                {
                    record.Toppings.Add(new MenuPizzaTopping()
                    {
                        MenuPizzaId = record.Id,
                        MenuPizzaToppingTypeId = topping.Id,
                        ToppingAmount = topping.SelectedAmount,
                        ToppingHalf = topping.SelectedToppingHalf
                    });
                }
            }
        }

        protected override MenuPizza ViewModelToRecord(ManageMenuPizzaViewModel model)
        {
            MenuPizza record = new MenuPizza()
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
                SortOrder = model.SortOrder
            };
            AddToppingsToRecord(record, model.ToppingList);
            return record;
        }
    }
}