using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DataLibrary.Models.QueryFilters;
using DataLibrary.Models.Tables;
using PizzaWebsite.Controllers.BaseControllers;
using PizzaWebsite.Models;
using PizzaWebsite.Models.ManageMenuImages;
using PizzaWebsite.Models.ManageMenus;

namespace PizzaWebsite.Controllers
{
    [Authorize(Roles = "Admin,Executive")]
    public class ManagePizzaCrustMenuController : BaseManageMenuController<MenuPizzaCrust, ManageMenuPizzaCrustViewModel>
    {
        public ManagePizzaCrustMenuController()
        {
            MenuIconValidation.RequiredWidth = 100;
            MenuIconValidation.RequiredHeight = 100;
            PizzaBuilderImageValidation.RequiredWidth = 525;
            PizzaBuilderImageValidation.RequiredHeight = 525;
        }

        public async Task<ActionResult> Index(int? page, int? rowsPerPage, string name)
        {
            ValidatePageQuery(ref page, ref rowsPerPage, 10);
            MenuPizzaCrustFilter searchFilter = new MenuPizzaCrustFilter
            {
                Name = name
            };
            return await Index(page.Value, rowsPerPage.Value, "Name", searchFilter);
        }

        public override async Task<ActionResult> Add()
        {
            MenuPizzaCrust crust = new MenuPizzaCrust
            {
                AvailableForPurchase = true
            };
            ManageMenuPizzaCrustViewModel model = await RecordToViewModelAsync(crust);
            return View("Manage", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(ManageMenuPizzaCrustViewModel model)
        {
            return await Add(model, model.Name);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ManageMenuPizzaCrustViewModel model)
        {
            return await Edit(model, model.Name);
        }

        public async Task<ActionResult> ManageImages(int? id)
        {
            if (!id.HasValue)
            {
                return MissingIdErrorMessage();
            }
            MenuPizzaCrust record = await PizzaDb.GetAsync<MenuPizzaCrust>(id.Value);
            if (record == null)
            {
                return InvalidIdErrorMessage(id.Value);
            }
            // todo: Finish menu icon and pizza builder image descriptions for this model.
            ManagePizzaMenuIngredientImagesViewModel model = new ManagePizzaMenuIngredientImagesViewModel()
            {
                Id = id.Value,
                MenuIconUrl = DirectoryServices.GetMenuImageUrl(record.Id, record.GetMenuCategoryType(), MenuImageType.MenuIcon),
                MenuIconDescription = "(Menu icon description)",
                PizzaBuilderImageUrl = DirectoryServices.GetMenuImageUrl(record.Id, record.GetMenuCategoryType(), MenuImageType.PizzaBuilderImage),
                PizzaBuilderImageDescription = "(Pizza builder image description)"
            };
            return View(model);
        }

        protected override async Task<ManageMenuPizzaCrustViewModel> RecordToViewModelAsync(MenuPizzaCrust record)
        {
            return await Task.FromResult(new ManageMenuPizzaCrustViewModel
            {
                Id = record.Id,
                AvailableForPurchase = record.AvailableForPurchase,
                Description = record.Description,
                Name = record.Name,
                SortOrder = record.SortOrder,
                PriceSmall = record.PriceSmall,
                PriceMedium = record.PriceMedium,
                PriceLarge = record.PriceLarge
            });
        }

        protected override MenuPizzaCrust ViewModelToRecord(ManageMenuPizzaCrustViewModel model)
        {
            return new MenuPizzaCrust
            {
                Id = model.Id,
                AvailableForPurchase = model.AvailableForPurchase,
                Description = model.Description,
                Name = model.Name,
                SortOrder = model.SortOrder,
                PriceSmall = model.PriceSmall,
                PriceMedium = model.PriceMedium,
                PriceLarge = model.PriceLarge
            };
        }
    }
}