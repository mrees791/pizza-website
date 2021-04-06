using DataLibrary.Models;
using DataLibrary.Models.Interfaces;
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
    public class BaseController : Controller
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

        protected async Task<List<TEntity>> LoadPagedEntitiesAsync<TEntity>(PizzaDatabase database, HttpRequestBase request, PaginationViewModel paginationVm,
            int? page, int? rowsPerPage, string sortColumnName, object searchFilters) where TEntity : IRecord
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

            int totalNumberOfItems = await database.GetNumberOfRecords<TEntity>(searchFilters);
            int totalPages = await database.GetNumberOfPagesAsync<TEntity>(searchFilters, rowsPerPage.Value);
            List<TEntity> entities = await database.GetListPagedAsync<TEntity>(searchFilters, page.Value, rowsPerPage.Value, sortColumnName);

            // Navigation pane
            paginationVm.QueryString = request.QueryString;
            paginationVm.CurrentPage = page.Value;
            paginationVm.RowsPerPage = rowsPerPage.Value;
            paginationVm.TotalPages = totalPages;
            paginationVm.TotalNumberOfItems = totalNumberOfItems;

            return entities;
        }

        protected async Task<SiteUser> GetCurrentUser()
        {
            List<SiteUser> users = await PizzaDb.GetListAsync<SiteUser>(new { UserName = User.Identity.Name });
            return users.FirstOrDefault();
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