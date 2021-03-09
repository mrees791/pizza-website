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
        public ActionResult EditStoreLocation(StoreLocationViewModel model)
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
            StoreLocation storeLocationRecord = storeLocationRecords.FirstOrDefault();
            bool idExists = storeLocationRecord != null;

            if (!idExists)
            {
                ErrorViewModel errorModel = new ErrorViewModel();
                errorModel.ErrorMessage = $"Store with ID: {id.Value} does not exist.";
                errorModel.ReturnUrlAction = $"{Url.Action("ManageStores")}?{Request.QueryString}";
                return View("Error", errorModel);
            }

            StoreLocationViewModel storeLocationVm = new StoreLocationViewModel(storeLocationRecord);
            return View("CreateEditStoreLocation", storeLocationVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateStoreLocation(StoreLocationViewModel model)
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
            StoreLocationViewModel model = new StoreLocationViewModel();
            return View("CreateEditStoreLocation", model);
        }

        public async Task<ActionResult> ManageStores(int? page, int? rowsPerPage, string storeName, string phoneNumber)
        {
            // Set default values
            if (!page.HasValue)
            {
                page = 1;
            }
            if (!rowsPerPage.HasValue)
            {
                rowsPerPage = 10;
            }

            // Apply search filters
            StoreLocationFilter searchFilter = new StoreLocationFilter();
            searchFilter.Name = storeName;
            searchFilter.PhoneNumber = phoneNumber;

            int totalPages = await PizzaDb.GetNumberOfPagesAsync<StoreLocation>(searchFilter, rowsPerPage.Value);
            List<StoreLocation> storeLocationRecords = await PizzaDb.GetListPagedAsync<StoreLocation>(searchFilter, page.Value, rowsPerPage.Value, "Name");

            // Create view model
            ManageStoresViewModel manageStoresVm = new ManageStoresViewModel();

            // Navigation pane
            //manageStoresVm.CurrentPage = page.Value;
            //manageStoresVm.TotalPages = totalPages;

            int maxPagesListed = 5;

            /*if (page.Value > 0 && page.Value <= manageStoresVm.TotalPages)
            {
                if (page.Value == 1)
                {
                    for (int iPage = 1; iPage <= manageStoresVm.TotalPages && iPage < page.Value + maxPagesListed; iPage++)
                    {
                        manageStoresVm.PageRange.Add(iPage);
                    }
                }
                // Others needed
            }*/

            /*if (manageStoresVm.PageRange.Count() < maxPagesListed)
            {
                manageStoresVm.PageRange.Add(manageStoresVm.TotalPages);
            }*/

            foreach (var location in storeLocationRecords)
            {
                manageStoresVm.StoreLocationVmList.Add(new StoreLocationViewModel(location));
            }

            return View(manageStoresVm);
        }
    }
}