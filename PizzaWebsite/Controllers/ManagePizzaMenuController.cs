using DataLibrary.Models;
using DataLibrary.Models.Tables;
using DataLibrary.Models.Utility;
using PizzaWebsite.Models;
using PizzaWebsite.Models.PizzaBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PizzaWebsite.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class ManagePizzaMenuController : BaseManageMenuController<MenuPizza, MenuPizzaBuilderViewModel>
    {
        public async Task<ActionResult> Index(int? page, int? rowsPerPage, string name)
        {
            object searchFilters = new
            {
                PizzaName = name
            };

            return await Index(page, rowsPerPage, searchFilters, "PizzaName");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(MenuPizzaBuilderViewModel model)
        {
            return Add(model, model.Name);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MenuPizzaBuilderViewModel model)
        {
            return Edit(model, model.Name);
        }

        protected override MenuPizzaBuilderViewModel EntityToViewModel(MenuPizza entity)
        {
            MenuPizzaBuilderViewModel model = new MenuPizzaBuilderViewModel()
            {
                Id = entity.Id,
                Name = entity.PizzaName,
                AvailableForPurchase = entity.AvailableForPurchase,
                SelectedCategory = entity.CategoryName,
                Description = entity.Description,
                SelectedCheeseAmount = entity.CheeseAmount,
                SelectedCheeseId = entity.MenuPizzaCheeseId,
                SelectedCrustFlavorId = entity.MenuPizzaCrustFlavorId,
                SelectedSauceId = entity.MenuPizzaSauceId,
                SelectedSauceAmount = entity.SauceAmount,
                CategoryList = ListUtility.GetPizzaCategoryList()
            };

            List<PizzaTopping> toppings = new List<PizzaTopping>();

            foreach (MenuPizzaTopping topping in entity.Toppings)
            {
                toppings.Add(new PizzaTopping()
                {
                    ToppingTypeId = topping.MenuPizzaToppingTypeId,
                    ToppingAmount = topping.ToppingAmount,
                    ToppingHalf = topping.ToppingHalf
                });
            }

            PizzaBuilderUtility.LoadNewPizzaBuilderLists(PizzaDb, toppings, model);

            return model;
        }

        private void AddToppingsToEntity(MenuPizza entity, List<PizzaToppingViewModel> toppings)
        {
            foreach (PizzaToppingViewModel topping in toppings)
            {
                if (topping.SelectedAmount != "None")
                {
                    entity.Toppings.Add(new MenuPizzaTopping()
                    {
                        MenuPizzaId = entity.Id,
                        MenuPizzaToppingTypeId = topping.Id,
                        ToppingAmount = topping.SelectedAmount,
                        ToppingHalf = topping.SelectedToppingHalf
                    });
                }
            }
        }

        protected override MenuPizza ViewModelToEntity(MenuPizzaBuilderViewModel model)
        {
            MenuPizza entity = new MenuPizza()
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

            AddToppingsToEntity(entity, model.ToppingList);

            return entity;
        }
    }
}