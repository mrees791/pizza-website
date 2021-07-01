using System.Threading.Tasks;
using System.Web.Mvc;
using DataLibrary.Models.QueryFilters;
using DataLibrary.Models.Tables;
using PizzaWebsite.Controllers.BaseControllers;
using PizzaWebsite.Models.ManageMenuImages;
using PizzaWebsite.Models.ManageMenus;

namespace PizzaWebsite.Controllers
{
    [Authorize(Roles = "Admin,Executive")]
    public class
        ManagePizzaToppingTypeMenuController : BaseManageMenuController<MenuPizzaToppingType,
            ManageMenuPizzaToppingTypeViewModel>
    {
        public ManagePizzaToppingTypeMenuController()
        {
            PizzaBuilderImageValidation = new MenuImageValidation()
            {
                RequiredWidth = 100,
                RequiredHeight = 50
            };
        }

        public async Task<ActionResult> Index(int? page, int? rowsPerPage, string name)
        {
            ValidatePageQuery(ref page, ref rowsPerPage, 10);
            MenuPizzaToppingTypeFilter searchFilter = new MenuPizzaToppingTypeFilter
            {
                Name = name
            };
            return await Index(page.Value, rowsPerPage.Value, "Name", searchFilter);
        }

        public override async Task<ActionResult> Add()
        {
            MenuPizzaToppingType topping = new MenuPizzaToppingType
            {
                AvailableForPurchase = true
            };
            ManageMenuPizzaToppingTypeViewModel model = await RecordToViewModelAsync(topping);
            return View("Manage", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(ManageMenuPizzaToppingTypeViewModel model)
        {
            return await Add(model, model.Name);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ManageMenuPizzaToppingTypeViewModel model)
        {
            return await Edit(model, model.Name);
        }

        protected override async Task<ManageMenuPizzaToppingTypeViewModel> RecordToViewModelAsync(
            MenuPizzaToppingType record)
        {
            return await Task.FromResult(new ManageMenuPizzaToppingTypeViewModel
            {
                Id = record.Id,
                AvailableForPurchase = record.AvailableForPurchase,
                CategoryName = record.CategoryName,
                Description = record.Description,
                Name = record.Name,
                SortOrder = record.SortOrder,
                PriceLight = record.PriceLight,
                PriceRegular = record.PriceRegular,
                PriceExtra = record.PriceExtra,
                ToppingCategoryList = ListServices.ToppingCategoryList
            });
        }

        protected override MenuPizzaToppingType ViewModelToRecord(ManageMenuPizzaToppingTypeViewModel model)
        {
            return new MenuPizzaToppingType
            {
                Id = model.Id,
                AvailableForPurchase = model.AvailableForPurchase,
                CategoryName = model.CategoryName,
                Description = model.Description,
                Name = model.Name,
                SortOrder = model.SortOrder,
                PriceLight = model.PriceLight,
                PriceRegular = model.PriceRegular,
                PriceExtra = model.PriceExtra
            };
        }
    }
}