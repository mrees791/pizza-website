using System.Threading.Tasks;
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
    public class ManagePizzaCheeseMenuController : BaseManageMenuController<MenuPizzaCheese, ManageMenuPizzaCheeseViewModel>
    {
        public ManagePizzaCheeseMenuController()
        {
        }

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

        public async Task<ActionResult> ManageImages(int? id)
        {
            if (!id.HasValue)
            {
                return MissingIdErrorMessage();
            }
            MenuPizzaCheese record = await PizzaDb.GetAsync<MenuPizzaCheese>(id.Value);
            if (record == null)
            {
                return InvalidIdErrorMessage(id.Value);
            }
            UploadMenuImageFormViewModel menuIconVm = new UploadMenuImageFormViewModel()
            {
                Name = "Menu Icon",
                Description = $"This is the icon the user will click on when creating their pizza in the pizza builder. The dimensions must be {MenuIconValidation.RequiredWidth}x{MenuIconValidation.RequiredHeight}.",
                ImageUrl = DirectoryServices.GetMenuImageUrl(record.Id, record.GetMenuCategoryType(), MenuImageType.MenuIcon),
                DropAreaId = "menuIconDropArea",
                ErrorMessageId = "menuIconError",
                ImageId = "menuIcon"
            };
            UploadMenuImageFormViewModel pizzaBuilderImageVm = new UploadMenuImageFormViewModel()
            {
                Name = "Pizza Builder Image",
                Description = $"This is the image that will be shown on the cheese layer of the pizza builder. The dimensions must be {PizzaBuilderImageValidation.RequiredWidth}x{PizzaBuilderImageValidation.RequiredHeight}.",
                ImageUrl = DirectoryServices.GetMenuImageUrl(record.Id, record.GetMenuCategoryType(), MenuImageType.PizzaBuilder),
                DropAreaId = "pizzaBuilderImageDropArea",
                ErrorMessageId = "pizzaBuilderImageError",
                ImageId = "pizzaBuilderImage"
            };
            ManagePizzaMenuIngredientImagesViewModel model = new ManagePizzaMenuIngredientImagesViewModel()
            {
                Id = id.Value,
                ViewTitle = $"Manage Images - {record.Name}",
                MenuIconVm = menuIconVm,
                PizzaBuilderImageVm = pizzaBuilderImageVm
            };
            return View(model);
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