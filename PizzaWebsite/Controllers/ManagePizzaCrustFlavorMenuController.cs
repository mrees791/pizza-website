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
        ManagePizzaCrustFlavorMenuController : BaseManageMenuController<MenuPizzaCrustFlavor,
            ManageMenuPizzaCrustFlavorViewModel>
    {
        public ManagePizzaCrustFlavorMenuController()
        {
            PizzaBuilderIconValidation = new MenuImageValidation()
            {
                RequiredWidth = 100,
                RequiredHeight = 50
            };
        }

        public async Task<ActionResult> Index(int? page, int? rowsPerPage, string name)
        {
            ValidatePageQuery(ref page, ref rowsPerPage, 10);
            MenuPizzaCrustFlavorFilter searchFilter = new MenuPizzaCrustFlavorFilter
            {
                Name = name
            };
            return await Index(page.Value, rowsPerPage.Value, "Name", searchFilter);
        }

        public override async Task<ActionResult> Add()
        {
            MenuPizzaCrustFlavor crustFlavor = new MenuPizzaCrustFlavor
            {
                AvailableForPurchase = true
            };
            ManageMenuPizzaCrustFlavorViewModel model = await RecordToViewModelAsync(crustFlavor);
            return View("Manage", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(ManageMenuPizzaCrustFlavorViewModel model)
        {
            return await Add(model, model.Name);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ManageMenuPizzaCrustFlavorViewModel model)
        {
            return await Edit(model, model.Name);
        }

        protected override async Task<ManageMenuPizzaCrustFlavorViewModel> RecordToViewModelAsync(
            MenuPizzaCrustFlavor record)
        {
            return await Task.FromResult(new ManageMenuPizzaCrustFlavorViewModel
            {
                Id = record.Id,
                AvailableForPurchase = record.AvailableForPurchase,
                Description = record.Description,
                Name = record.Name,
                SortOrder = record.SortOrder
            });
        }

        protected override MenuPizzaCrustFlavor ViewModelToRecord(ManageMenuPizzaCrustFlavorViewModel model)
        {
            return new MenuPizzaCrustFlavor
            {
                Id = model.Id,
                AvailableForPurchase = model.AvailableForPurchase,
                Description = model.Description,
                Name = model.Name,
                SortOrder = model.SortOrder
            };
        }
    }
}