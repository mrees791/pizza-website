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
    public class ManagePizzaCrustFlavorMenuController : BaseManageMenuController<MenuPizzaCrustFlavor, ManageMenuPizzaCrustFlavorViewModel>
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
        public ActionResult Add(ManageMenuPizzaCrustFlavorViewModel model)
        {
            return Add(model, model.Name);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ManageMenuPizzaCrustFlavorViewModel model)
        {
            return Edit(model, model.Name);
        }

        protected override ManageMenuPizzaCrustFlavorViewModel EntityToViewModel(MenuPizzaCrustFlavor entity)
        {
            return new ManageMenuPizzaCrustFlavorViewModel
            {
                Id = entity.Id,
                AvailableForPurchase = entity.AvailableForPurchase,
                Description = entity.Description,
                HasMenuIcon = entity.HasMenuIcon,
                HasPizzaBuilderImage = entity.HasPizzaBuilderImage,
                Name = entity.Name,
                MenuIconFile = entity.MenuIconFile,
                PizzaBuilderImageFile = entity.PizzaBuilderImageFile,
                SortOrder = entity.SortOrder
            };
        }

        protected override MenuPizzaCrustFlavor ViewModelToEntity(ManageMenuPizzaCrustFlavorViewModel model)
        {
            return new MenuPizzaCrustFlavor()
            {
                Id = model.Id,
                AvailableForPurchase = model.AvailableForPurchase,
                Description = model.Description,
                HasMenuIcon = model.HasMenuIcon,
                HasPizzaBuilderImage = model.HasPizzaBuilderImage,
                Name = model.Name,
                MenuIconFile = model.MenuIconFile,
                PizzaBuilderImageFile = model.PizzaBuilderImageFile,
                SortOrder = model.SortOrder
            };
        }
    }
}
}