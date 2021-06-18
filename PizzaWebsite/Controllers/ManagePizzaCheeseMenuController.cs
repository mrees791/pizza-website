using System.Threading.Tasks;
using System.Web.Mvc;
using DataLibrary.Models.QueryFilters;
using DataLibrary.Models.Tables;
using PizzaWebsite.Controllers.BaseControllers;
using PizzaWebsite.Models.ManageMenus;

namespace PizzaWebsite.Controllers
{
    [Authorize(Roles = "Admin,Executive")]
    public class
        ManagePizzaCheeseMenuController : BaseManageMenuController<MenuPizzaCheese, ManageMenuPizzaCheeseViewModel>
    {
        public async Task<ActionResult> Index(int? page, int? rowsPerPage, string name)
        {
            ValidatePageQuery(ref page, ref rowsPerPage, 10);
            MenuPizzaCheeseFilter searchFilter = new MenuPizzaCheeseFilter
            {
                Name = name
            };
            return await Index(page.Value, rowsPerPage.Value, "Name", searchFilter);
        }

        public override async Task<ActionResult> Add()
        {
            MenuPizzaCheese cheese = new MenuPizzaCheese
            {
                AvailableForPurchase = true
            };
            ManageMenuPizzaCheeseViewModel model = await RecordToViewModelAsync(cheese);
            return View("Manage", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(ManageMenuPizzaCheeseViewModel model)
        {
            return await Add(model, model.Name);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ManageMenuPizzaCheeseViewModel model)
        {
            return await Edit(model, model.Name);
        }

        protected override async Task<ManageMenuPizzaCheeseViewModel> RecordToViewModelAsync(MenuPizzaCheese record)
        {
            return await Task.FromResult(new ManageMenuPizzaCheeseViewModel
            {
                Id = record.Id,
                SortOrder = record.SortOrder,
                AvailableForPurchase = record.AvailableForPurchase,
                Description = record.Description,
                Name = record.Name,
                PriceLight = record.PriceLight,
                PriceRegular = record.PriceRegular,
                PriceExtra = record.PriceExtra
            });
        }

        protected override MenuPizzaCheese ViewModelToRecord(ManageMenuPizzaCheeseViewModel model)
        {
            return new MenuPizzaCheese
            {
                Id = model.Id,
                SortOrder = model.SortOrder,
                AvailableForPurchase = model.AvailableForPurchase,
                Description = model.Description,
                Name = model.Name,
                PriceLight = model.PriceLight,
                PriceRegular = model.PriceRegular,
                PriceExtra = model.PriceExtra
            };
        }
    }
}