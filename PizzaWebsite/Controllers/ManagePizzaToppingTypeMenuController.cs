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
    public class
        ManagePizzaToppingTypeMenuController : BaseManageMenuController<MenuPizzaToppingType,
            ManageMenuPizzaToppingTypeViewModel>
    {
        public ManagePizzaToppingTypeMenuController()
        {
            MenuIconValidation.RequiredWidth = 100;
            MenuIconValidation.RequiredHeight = 75;
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

        public async Task<ActionResult> ManageImages(int? id)
        {
            if (!id.HasValue)
            {
                return MissingIdErrorMessage();
            }
            MenuPizzaToppingType record = await PizzaDb.GetAsync<MenuPizzaToppingType>(id.Value);
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
                Description = $"This is the image that will be shown on a topping layer of the pizza builder. The dimensions must be {PizzaBuilderImageValidation.RequiredWidth}x{PizzaBuilderImageValidation.RequiredHeight}.",
                ImageUrl = DirectoryServices.GetMenuImageUrl(record.Id, record.GetMenuCategoryType(), MenuImageType.PizzaBuilder),
                DropAreaId = "pizzaBuilderImageDropArea",
                ErrorMessageId = "pizzaBuilderImageError",
                ImageId = "pizzaBuilderImage"
            };
            UploadMenuImageFormViewModel pizzaBuilderLeftImageVm = new UploadMenuImageFormViewModel()
            {
                Name = "Pizza Builder Left Image",
                Description = $"This will be shown when the user only wants their topping on the left half of the pizza. The dimensions must be {PizzaBuilderImageValidation.RequiredWidth}x{PizzaBuilderImageValidation.RequiredHeight}.",
                ImageUrl = DirectoryServices.GetMenuImageUrl(record.Id, record.GetMenuCategoryType(), MenuImageType.PizzaBuilderLeft),
                DropAreaId = "pizzaBuilderLeftImageDropArea",
                ErrorMessageId = "pizzaBuilderLeftImageError",
                ImageId = "pizzaBuilderLeftImage"
            };
            UploadMenuImageFormViewModel pizzaBuilderRightImageVm = new UploadMenuImageFormViewModel()
            {
                Name = "Pizza Builder Right Image",
                Description = $"This will be shown when the user only wants their topping on the right half of the pizza. The dimensions must be {PizzaBuilderImageValidation.RequiredWidth}x{PizzaBuilderImageValidation.RequiredHeight}.",
                ImageUrl = DirectoryServices.GetMenuImageUrl(record.Id, record.GetMenuCategoryType(), MenuImageType.PizzaBuilderRight),
                DropAreaId = "pizzaBuilderRightImageDropArea",
                ErrorMessageId = "pizzaBuilderRightImageError",
                ImageId = "pizzaBuilderRightImage"
            };
            ManagePizzaMenuToppingImagesViewModel model = new ManagePizzaMenuToppingImagesViewModel()
            {
                Id = id.Value,
                ViewTitle = $"Manage Images - {record.Name}",
                MenuIconVm = menuIconVm,
                PizzaBuilderImageVm = pizzaBuilderImageVm,
                PizzaBuilderLeftImageVm = pizzaBuilderLeftImageVm,
                PizzaBuilderRightImageVm = pizzaBuilderRightImageVm
            };
            return View(model);
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