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
    public class ManagePizzaCheeseMenuController : BaseController
    {
        public async Task<ActionResult> Index(int? page, int? rowsPerPage, string name)
        {
            var viewModelList = new ManagePagedListViewModel<ManageMenuPizzaCheeseViewModel>();

            object searchFilters = new
            {
                Name = name
            };

            List<MenuPizzaCheese> entityList = await LoadPagedEntitiesAsync<MenuPizzaCheese>(PizzaDb, Request, viewModelList.PaginationVm, page, rowsPerPage, "Name", searchFilters);

            foreach (MenuPizzaCheese entity in entityList)
            {
                viewModelList.ItemViewModelList.Add(EntityToViewModel(entity));
            }

            return View(viewModelList);
        }

        public ActionResult Add()
        {
            return View("Manage", new ManageMenuPizzaCheeseViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ManageMenuPizzaCheeseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Manage", model);
            }

            PizzaDb.Insert(ViewModelToEntity(model));

            ConfirmationViewModel confirmationModel = new ConfirmationViewModel();
            confirmationModel.ConfirmationMessage = $"{model.Name} has been added to the database.";
            confirmationModel.ReturnUrlAction = $"{Url.Action("Index")}?{Request.QueryString}";

            return View("CreateEditConfirmation", confirmationModel);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            List<MenuPizzaCheese> entityList = await PizzaDb.GetListAsync<MenuPizzaCheese>(new { Id = id.Value });
            ManageMenuPizzaCheeseViewModel model = EntityToViewModel(entityList.FirstOrDefault());

            return View("Manage", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ManageMenuPizzaCheeseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Manage", model);
            }

            PizzaDb.Update(ViewModelToEntity(model));

            ConfirmationViewModel confirmationModel = new ConfirmationViewModel();
            confirmationModel.ConfirmationMessage = $"Your changes to {model.Name} have been confirmed.";
            confirmationModel.ReturnUrlAction = $"{Url.Action("Index")}?{Request.QueryString}";

            return View("CreateEditConfirmation", confirmationModel);
        }

        public ManageMenuPizzaCheeseViewModel EntityToViewModel(MenuPizzaCheese entity)
        {
            return new ManageMenuPizzaCheeseViewModel
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
                PriceExtra = entity.PriceExtra
            };
        }

        public MenuPizzaCheese ViewModelToEntity(ManageMenuPizzaCheeseViewModel model)
        {
            return new MenuPizzaCheese()
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
                PriceExtra = model.PriceExtra
            };
        }
    }
}