using DataLibrary.Models;
using DataLibrary.Models.Filters;
using DataLibrary.Models.Filters.TableFilters;
using DataLibrary.Models.Tables;
using Microsoft.AspNet.Identity.Owin;
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
    public class ManageWebsiteController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private PizzaDatabase _pizzaDb;

        public PizzaDatabase PizzaDb
        {
            get
            {
                return _pizzaDb ?? HttpContext.GetOwinContext().Get<PizzaDatabase>();
            }
            private set
            {
                _pizzaDb = value;
            }
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: ManageWebsite
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditStoreLocation(ManageStoreLocationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateEditStoreLocation", model);
            }

            PizzaDb.Update(model.ToDbModel());

            ConfirmationViewModel confirmationModel = new ConfirmationViewModel();
            confirmationModel.ConfirmationMessage = $"Your changes to {model.Name} have been confirmed.";
            confirmationModel.ReturnUrlAction = $"{Url.Action("ManageStores")}?{Request.QueryString}";

            return View("CreateEditConfirmation", confirmationModel);
        }

        public async Task<ActionResult> EditStoreLocation(int? id)
        {
            if (!id.HasValue)
            {
                ErrorViewModel errorModel = new ErrorViewModel();
                errorModel.ErrorMessage = $"ID is missing.";
                errorModel.ReturnUrlAction = $"{Url.Action("ManageStores")}?{Request.QueryString}";
                return View("Error", errorModel);
            }

            List<StoreLocation> storeLocationRecords = await PizzaDb.GetListAsync<StoreLocation>(new { Id = id.Value });
            StoreLocation storeLocation = storeLocationRecords.FirstOrDefault();
            bool idExists = storeLocation != null;

            if (!idExists)
            {
                ErrorViewModel errorModel = new ErrorViewModel();
                errorModel.ErrorMessage = $"Store with ID: {id.Value} does not exist.";
                errorModel.ReturnUrlAction = $"{Url.Action("ManageStores")}?{Request.QueryString}";
                return View("Error", errorModel);
            }

            ManageStoreLocationViewModel storeLocationVm = new ManageStoreLocationViewModel();
            storeLocationVm.FromDbModel(storeLocation);
            return View("CreateEditStoreLocation", storeLocationVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateStoreLocation(ManageStoreLocationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateEditStoreLocation", model);
            }

            PizzaDb.Insert(model.ToDbModel());

            ConfirmationViewModel confirmationModel = new ConfirmationViewModel();
            confirmationModel.ConfirmationMessage = $"{model.Name} has been added to the database.";
            confirmationModel.ReturnUrlAction = $"{Url.Action("ManageStores")}?{Request.QueryString}";

            return View("CreateEditConfirmation", confirmationModel);
        }

        public ActionResult CreateStoreLocation()
        {
            ManageStoreLocationViewModel model = new ManageStoreLocationViewModel();
            return View("CreateEditStoreLocation", model);
        }

        public async Task<ActionResult> ManageStores(int? page, int? rowsPerPage, string storeName, string phoneNumber)
        {
            var manageStoresVm = new ManageListViewModel<ManageStoreLocationViewModel, StoreLocation, StoreLocationFilter>();

            // Apply search filters
            manageStoresVm.SearchFilter.Name = storeName;
            manageStoresVm.SearchFilter.PhoneNumber = phoneNumber;

            await manageStoresVm.LoadViewModelRecordsAsync(PizzaDb, Request, page, rowsPerPage, "Name");

            return View(manageStoresVm);
        }

        public async Task<ActionResult> ManageUsers(int? page, int? rowsPerPage, string userName, string email)
        {
            var manageStoresVm = new ManageListViewModel<ManageUserViewModel, SiteUser, SiteUserFilter>();

            // Apply search filters
            manageStoresVm.SearchFilter.UserName = userName;
            manageStoresVm.SearchFilter.Email = email;

            await manageStoresVm.LoadViewModelRecordsAsync(PizzaDb, Request, page, rowsPerPage, "UserName");

            return View(manageStoresVm);
        }
    }
}