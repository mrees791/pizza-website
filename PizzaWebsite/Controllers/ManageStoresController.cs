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
    public class ManageStoresController : BaseController
    {
        public async Task<ActionResult> Index(int? page, int? rowsPerPage, string storeName, string phoneNumber)
        {
            var manageStoresVm = new ManagePagedListViewModel<ManageStoreViewModel>();

            object searchFilters = new
            {
                Name = storeName,
                PhoneNumber = phoneNumber
            };

            List<StoreLocation> storeEntities = await LoadPagedEntitiesAsync<StoreLocation>(PizzaDb, Request, manageStoresVm.PaginationVm, page, rowsPerPage, "Name", searchFilters);

            foreach (StoreLocation storeEntity in storeEntities)
            {
                ManageStoreViewModel model = EntityToViewModel(storeEntity);
                manageStoresVm.ItemViewModelList.Add(model);
            }

            return View(manageStoresVm);
        }

        public ActionResult CreateStore()
        {
            ManageStoreViewModel model = new ManageStoreViewModel();
            return View("ManageStore", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateStore(ManageStoreViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("ManageStore", model);
            }

            StoreLocation storeEntity = ViewModelToEntity(model);
            PizzaDb.Insert(storeEntity);

            ConfirmationViewModel confirmationModel = new ConfirmationViewModel();
            confirmationModel.ConfirmationMessage = $"{model.Name} has been added to the database.";
            confirmationModel.ReturnUrlAction = $"{Url.Action("Index")}?{Request.QueryString}";

            return View("CreateEditConfirmation", confirmationModel);
        }

        public async Task<ActionResult> EditStore(int? id)
        {
            List<StoreLocation> storeEntities = await PizzaDb.GetListAsync<StoreLocation>(new { Id = id.Value });
            StoreLocation storeEntity = storeEntities.FirstOrDefault();
            ManageStoreViewModel model = EntityToViewModel(storeEntity);

            return View("ManageStore", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditStore(ManageStoreViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("ManageStore", model);
            }

            PizzaDb.Update(ViewModelToEntity(model));

            ConfirmationViewModel confirmationModel = new ConfirmationViewModel();
            confirmationModel.ConfirmationMessage = $"Your changes to {model.Name} have been confirmed.";
            confirmationModel.ReturnUrlAction = $"{Url.Action("Index")}?{Request.QueryString}";

            return View("CreateEditConfirmation", confirmationModel);
        }

        public ManageStoreViewModel EntityToViewModel(StoreLocation entity)
        {
            return new ManageStoreViewModel()
            {
                Id = entity.Id,
                City = entity.City,
                IsActiveLocation = entity.IsActiveLocation,
                Name = entity.Name,
                PhoneNumber = entity.PhoneNumber,
                SelectedState = entity.State,
                StreetAddress = entity.StreetAddress,
                ZipCode = entity.ZipCode
            };
        }

        private StoreLocation ViewModelToEntity(ManageStoreViewModel model)
        {
            return new StoreLocation()
            {
                Id = model.Id,
                City = model.City,
                IsActiveLocation = model.IsActiveLocation,
                Name = model.Name,
                PhoneNumber = model.PhoneNumber,
                State = model.SelectedState,
                StreetAddress = model.StreetAddress,
                ZipCode = model.ZipCode
            };
        }
    }
}