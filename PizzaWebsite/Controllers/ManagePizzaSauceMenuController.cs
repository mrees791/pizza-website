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
    public class ManagePizzaSauceMenuController : BaseManageMenuController<MenuPizzaSauce, ManageMenuPizzaSauceViewModel>
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
        public ActionResult Add(ManageMenuPizzaSauceViewModel model)
        {
            return Add(model, model.Name);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ManageMenuPizzaSauceViewModel model)
        {
            return Edit(model, model.Name);
        }

        protected override ManageMenuPizzaSauceViewModel EntityToViewModel(MenuPizzaSauce entity)
        {
            return new ManageMenuPizzaSauceViewModel
            {
                Id = entity.Id,
                AvailableForPurchase = entity.AvailableForPurchase,
                Description = entity.Description,
                HasMenuIcon = entity.HasMenuIcon,
                HasPizzaBuilderImage = entity.HasPizzaBuilderImage,
                MenuIconFile = entity.MenuIconFile,
                Name = entity.Name,
                PizzaBuilderImageFile = entity.PizzaBuilderImageFile,
                PriceLight = entity.PriceLight,
                PriceRegular = entity.PriceRegular,
                PriceExtra = entity.PriceExtra,
                SortOrder = entity.SortOrder
            };
        }

        protected override MenuPizzaSauce ViewModelToEntity(ManageMenuPizzaSauceViewModel model)
        {
            return new MenuPizzaSauce()
            {
                Id = model.Id,
                AvailableForPurchase = model.AvailableForPurchase,
                Description = model.Description,
                HasMenuIcon = model.HasMenuIcon,
                HasPizzaBuilderImage = model.HasPizzaBuilderImage,
                MenuIconFile = model.MenuIconFile,
                Name = model.Name,
                PizzaBuilderImageFile = model.PizzaBuilderImageFile,
                PriceLight = model.PriceLight,
                PriceRegular = model.PriceRegular,
                PriceExtra = model.PriceExtra,
                SortOrder = model.SortOrder
            };
        }
    }
}