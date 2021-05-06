using DataLibrary.Models.OldTables;
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
    public class ManagePizzaCheeseMenuController : BaseManageMenuController<MenuPizzaCheese, ManageMenuPizzaCheeseViewModel>
    {
        public async Task<ActionResult> Index(int? page, int? rowsPerPage, string name)
        {
            object searchFilters = new
            {
                Name = name
            };

            return await Index(page, rowsPerPage, searchFilters, "Name");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ManageMenuPizzaCheeseViewModel model)
        {
            return Add(model, model.Name);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ManageMenuPizzaCheeseViewModel model)
        {
            return Edit(model, model.Name);
        }

        protected override ManageMenuPizzaCheeseViewModel EntityToViewModel(MenuPizzaCheese entity)
        {
            return new ManageMenuPizzaCheeseViewModel
            {
                Id = entity.Id,
                SortOrder = entity.SortOrder,
                AvailableForPurchase = entity.AvailableForPurchase,
                Description = entity.Description,
                HasMenuIcon = entity.HasMenuIcon,
                HasPizzaBuilderImage = entity.HasPizzaBuilderImage,
                MenuIconFile = entity.MenuIconFile,
                Name = entity.Name,
                PizzaBuilderImageFile = entity.PizzaBuilderImageFile,
                PriceLight = entity.PriceLight,
                PriceRegular = entity.PriceRegular,
                PriceExtra = entity.PriceExtra
            };
        }

        protected override MenuPizzaCheese ViewModelToEntity(ManageMenuPizzaCheeseViewModel model)
        {
            return new MenuPizzaCheese()
            {
                Id = model.Id,
                SortOrder = model.SortOrder,
                AvailableForPurchase = model.AvailableForPurchase,
                Description = model.Description,
                HasMenuIcon = model.HasMenuIcon,
                HasPizzaBuilderImage = model.HasPizzaBuilderImage,
                MenuIconFile = model.MenuIconFile,
                Name = model.Name,
                PizzaBuilderImageFile = model.PizzaBuilderImageFile,
                PriceLight = model.PriceLight,
                PriceRegular = model.PriceRegular,
                PriceExtra = model.PriceExtra
            };
        }
    }
}