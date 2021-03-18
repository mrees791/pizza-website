using DataLibrary.Models.Tables;
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
    public class ManagePizzaCrustMenuController : BaseManageMenuController<MenuPizzaCrust, ManageMenuPizzaCrustViewModel>
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
        public ActionResult Add(ManageMenuPizzaCrustViewModel model)
        {
            return Add(model, model.Name);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ManageMenuPizzaCrustViewModel model)
        {
            return Edit(model, model.Name);
        }

        protected override ManageMenuPizzaCrustViewModel EntityToViewModel(MenuPizzaCrust entity)
        {
            return new ManageMenuPizzaCrustViewModel
            {
                Id = entity.Id,
                AvailableForPurchase = entity.AvailableForPurchase,
                Description = entity.Description,
                HasMenuIcon = entity.HasMenuIcon,
                HasPizzaBuilderImage = entity.HasPizzaBuilderImage,
                MenuIconFile = entity.MenuIconFile,
                Name = entity.Name,
                PizzaBuilderImageFile = entity.PizzaBuilderImageFile,
                SortOrder = entity.SortOrder,
                PriceSmall = entity.PriceSmall,
                PriceMedium = entity.PriceMedium,
                PriceLarge = entity.PriceLarge
            };
        }

        protected override MenuPizzaCrust ViewModelToEntity(ManageMenuPizzaCrustViewModel model)
        {
            return new MenuPizzaCrust()
            {
                Id = model.Id,
                AvailableForPurchase = model.AvailableForPurchase,
                Description = model.Description,
                HasMenuIcon = model.HasMenuIcon,
                HasPizzaBuilderImage = model.HasPizzaBuilderImage,
                MenuIconFile = model.MenuIconFile,
                Name = model.Name,
                PizzaBuilderImageFile = model.PizzaBuilderImageFile,
                SortOrder = model.SortOrder,
                PriceSmall = model.PriceSmall,
                PriceMedium = model.PriceMedium,
                PriceLarge = model.PriceLarge
            };
        }
    }
}