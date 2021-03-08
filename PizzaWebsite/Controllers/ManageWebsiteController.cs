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
                return View(model);
            }

            PizzaDb.Update(model.ToDbModel());
            return RedirectToAction(nameof(ManageStores));
        }

        [HttpGet]
        public async Task<ActionResult> EditStoreLocation(int? id)
        {
            List<StoreLocation> storeLocationRecords = await PizzaDb.GetListAsync<StoreLocation>(new { Id = id.Value });
            StoreLocation storeLocationRecord = storeLocationRecords.FirstOrDefault();

            if (storeLocationRecord != null)
            {
                StoreLocationViewModel storeLocationVm = new StoreLocationViewModel(storeLocationRecord);
                return View(storeLocationVm);
            }

            throw new NotImplementedException();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateStoreLocation(StoreLocationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            PizzaDb.Insert(model.ToDbModel());
            return RedirectToAction(nameof(ManageStores));
        }

        public ActionResult CreateStoreLocation()
        {
            return View("EditStoreLocation", new StoreLocationViewModel());
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

            foreach (var location in storeLocationRecords)
            {
                manageStoresVm.StoreLocationVmList.Add(new StoreLocationViewModel(location));
            }

            return View(manageStoresVm);
        }
    }
}