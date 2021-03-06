using DataLibrary.Models;
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
            List<StoreLocation> storeLocationRecords = await PizzaDb.GetListAsync<StoreLocation>(new { Id = id });
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
            return View(new StoreLocationViewModel());
        }

        [HttpGet]
        public ActionResult ManageStores(string name)
        {
            ManageStoresViewModel manageStoresVm = new ManageStoresViewModel();

            // Apply filters, paging here
            /*List<StoreLocation> storeLocationRecords = await PizzaDb.GetListAsync<StoreLocation>();

            foreach (var location in storeLocationRecords)
            {
                manageStoresVm.StoreLocationVmList.Add(new StoreLocationViewModel(location));
            }*/

            return View(manageStoresVm);
        }

        /*public async Task<ActionResult> ManageStores()
        {
            ManageStoresViewModel manageStoresVm = new ManageStoresViewModel();
            List<StoreLocation> storeLocationRecords = await PizzaDb.GetListAsync<StoreLocation>();

            foreach (var location in storeLocationRecords)
            {
                manageStoresVm.StoreLocationVmList.Add(new StoreLocationViewModel(location));
            }

            return View(manageStoresVm);
        }*/
    }
}