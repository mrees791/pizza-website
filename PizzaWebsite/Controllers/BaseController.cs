using DataLibrary.Models;
using DataLibrary.Models.QueryFilters;
using DataLibrary.Models.Tables;
using Microsoft.AspNet.Identity;
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

        protected async Task<List<TRecord>> LoadPagedRecordsAsync<TRecord>(int? page, int? rowsPerPage, string orderByColumn, SortOrder sortOrder,
            QueryFilterBase searchFilter, PizzaDatabase database, HttpRequestBase request, PaginationViewModel paginationVm) where TRecord : Record
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

            List<TRecord> recordList = new List<TRecord>();
            int totalNumberOfItems = await database.GetNumberOfRecordsAsync<TRecord>(searchFilter);
            int totalPages = await database.GetNumberOfPagesAsync<TRecord>(rowsPerPage.Value, searchFilter);
            recordList.AddRange(await database.GetPagedListAsync<TRecord>(page.Value, rowsPerPage.Value, orderByColumn, sortOrder, searchFilter));

            // Navigation pane
            paginationVm.QueryString = request.QueryString;
            paginationVm.CurrentPage = page.Value;
            paginationVm.RowsPerPage = rowsPerPage.Value;
            paginationVm.TotalPages = totalPages;
            paginationVm.TotalNumberOfItems = totalNumberOfItems;

            return recordList;
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