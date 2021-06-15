using DataLibrary.Models;
using DataLibrary.Models.QueryFilters;
using DataLibrary.Models.Services;
using DataLibrary.Models.Tables;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PizzaWebsite.Models;
using PizzaWebsite.Models.Geography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PizzaWebsite.Controllers.BaseControllers
{
    public abstract class BaseController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private PizzaDatabase _pizzaDb;
        private ListServices _listServices;
        private GeographyServices _geographyServices;

        public ListServices ListServices
        {
            get
            {
                return _listServices ?? new ListServices();
            }
            private set
            {
                _listServices = value;
            }
        }

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

        public GeographyServices GeographyServices
        {
            get
            {
                return _geographyServices ?? new GeographyServices();
            }
            private set
            {
                _geographyServices = value;
            }
        }

        protected async Task<SiteUser> GetCurrentUserAsync()
        {
            return await PizzaDb.GetSiteUserByIdAsync(User.Identity.GetUserId());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}