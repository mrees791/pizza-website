using DataLibrary.Models;
using DataLibrary.Models.Tables;
using DataLibrary.Models.Utility;
using PizzaWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PizzaWebsite.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class ManagePizzaMenuController : BaseManageMenuController<MenuPizza, ManageMenuPizzaViewModel>
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
        public ActionResult Add(ManageMenuPizzaViewModel model)
        {
            return Add(model, model.Name);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ManageMenuPizzaViewModel model)
        {
            return Edit(model, model.Name);
        }

        protected override ManageMenuPizzaViewModel EntityToViewModel(MenuPizza entity)
        {
            // Load pizza toppings
            List<PizzaTopping> toppings = new List<PizzaTopping>();
            // todo: Remove
            //entity.Toppings = PizzaDb.GetList<MenuPizzaTopping>(new { MenuPizzaId = entity.Id }, "Id");

            ManageMenuPizzaViewModel model = new ManageMenuPizzaViewModel()
            {
                Id = entity.Id,
                Name = entity.PizzaName,
                AvailableForPurchase = entity.AvailableForPurchase,
                SelectedCategory = entity.CategoryName,
                Description = entity.Description,
                SelectedCheeseAmount = entity.CheeseAmount,
                SelectedCheeseId = entity.MenuPizzaCheeseId,
                SelectedCrustFlavorId = entity.MenuPizzaCrustFlavorId,
                SelectedCrustId = entity.MenuPizzaCrustId,
                SelectedSauceId = entity.MenuPizzaSauceId,
                SelectedSauceAmount = entity.SauceAmount,
                CategoryList = ListUtility.GetPizzaCategoryList()
            };

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

        protected override MenuPizza ViewModelToEntity(ManageMenuPizzaViewModel model)
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
                MenuPizzaCrustId = model.SelectedCrustId,
                MenuPizzaSauceId = model.SelectedSauceId,
                SauceAmount = model.SelectedSauceAmount,
                SortOrder = model.SortOrder
            };

            AddToppingsToEntity(entity, model.ToppingList);

            return entity;
        }
    }
}