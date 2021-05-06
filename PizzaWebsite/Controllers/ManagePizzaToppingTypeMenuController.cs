using DataLibrary.Models.OldTables;
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
    public class ManagePizzaToppingTypeMenuController : BaseManageMenuController<MenuPizzaToppingType, ManageMenuPizzaToppingTypeViewModel>
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
        public ActionResult Add(ManageMenuPizzaToppingTypeViewModel model)
        {
            return Add(model, model.Name);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ManageMenuPizzaToppingTypeViewModel model)
        {
            return Edit(model, model.Name);
        }

        protected override ManageMenuPizzaToppingTypeViewModel EntityToViewModel(MenuPizzaToppingType entity)
        {
            return new ManageMenuPizzaToppingTypeViewModel
            {
                Id = entity.Id,
                AvailableForPurchase = entity.AvailableForPurchase,
                CategoryName = entity.CategoryName,
                Description = entity.Description,
                HasMenuIcon = entity.HasMenuIcon,
                HasPizzaBuilderImage = entity.HasPizzaBuilderImage,
                MenuIconFile = entity.MenuIconFile,
                Name = entity.Name,
                PizzaBuilderImageFile = entity.PizzaBuilderImageFile,
                SortOrder = entity.SortOrder,
                PriceLight = entity.PriceLight,
                PriceRegular = entity.PriceRegular,
                PriceExtra = entity.PriceExtra,
                ToppingCategoryList = ListUtility.GetToppingCategoryList()
            };
        }

        protected override MenuPizzaToppingType ViewModelToEntity(ManageMenuPizzaToppingTypeViewModel model)
        {
            return new MenuPizzaToppingType()
            {
                Id = model.Id,
                AvailableForPurchase = model.AvailableForPurchase,
                CategoryName = model.CategoryName,
                Description = model.Description,
                HasMenuIcon = model.HasMenuIcon,
                HasPizzaBuilderImage = model.HasPizzaBuilderImage,
                MenuIconFile = model.MenuIconFile,
                Name = model.Name,
                PizzaBuilderImageFile = model.PizzaBuilderImageFile,
                SortOrder = model.SortOrder,
                PriceLight = model.PriceLight,
                PriceRegular = model.PriceRegular,
                PriceExtra = model.PriceExtra
            };
        }
    }
}