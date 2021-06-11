using DataLibrary.Models;
using DataLibrary.Models.QueryFilters;
using DataLibrary.Models.Tables;
using PizzaWebsite.Controllers.BaseControllers;
using PizzaWebsite.Models;
using PizzaWebsite.Models.Employees;
using PizzaWebsite.Models.ManageUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PizzaWebsite.Controllers
{
    [Authorize(Roles = "Admin,Executive,Manager")]
    public class ManageUsersController : BaseManageWebsiteController<SiteUser>
    {
        public async Task<ActionResult> Index(int? page, int? rowsPerPage, string userId, string email)
        {
            ValidatePageQuery(ref page, ref rowsPerPage, 10);
            SiteUserFilter searchFilter = new SiteUserFilter()
            {
                Id = userId,
                Email = email
            };
            PaginationViewModel paginationVm = new PaginationViewModel();
            List<ManageUserViewModel> userVmList = new List<ManageUserViewModel>();
            IEnumerable<SiteUser> userList = await LoadPagedRecordsAsync(page.Value, rowsPerPage.Value, "Id", SortOrder.Ascending, searchFilter, PizzaDb, paginationVm);
            foreach (SiteUser user in userList)
            {
                ManageUserViewModel userVm = new ManageUserViewModel()
                {
                    Id = user.Id,
                    Email = user.Email,
                    IsBanned = user.IsBanned
                };
                userVmList.Add(userVm);
            }
            var model = new ManagePagedListViewModel<ManageUserViewModel>()
            {
                PaginationVm = paginationVm,
                ItemViewModelList = userVmList
            };
            return View(model);
        }

        /// <summary>
        /// Replaces periods in the user's ID with (dot).
        /// This is needed by {id} in the MapRoute method of the RouteConfig class.
        /// The {id} section of the route won't work with periods so we use (dot) as a placeholder.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetUrlSafeId(string id)
        {
            return id.Replace(".", "(dot)");
        }

        public string FromUrlSafeId(string urlSafeId)
        {
            return urlSafeId.Replace("(dot)", ".");
        }

        [Authorize(Roles = "Admin,Executive")]
        public async Task<ActionResult> ManageUser(string id)
        {
            SiteUser user = await PizzaDb.GetSiteUserByIdAsync(FromUrlSafeId(id));
            ManageUserViewModel model = new ManageUserViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                IsBanned = user.IsBanned
            };
            return View("ManageUser", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Executive")]
        public async Task<ActionResult> ManageUser(ManageUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("ManageUser", model);
            }
            string id = FromUrlSafeId(model.Id);
            SiteUser user = await PizzaDb.GetSiteUserByIdAsync(id);
            user.IsBanned = model.IsBanned;
            int rowsAffected = await PizzaDb.UpdateAsync(user);
            if (rowsAffected == 0)
            {
                ModelState.AddModelError("", $"Unable to update user with ID: {user.Id}");
                return View("ManageUser", model);
            }
            ConfirmationViewModel confirmationVm = new ConfirmationViewModel()
            {
                ConfirmationMessage = $"Your changes to {id} have been confirmed.",
                ReturnUrlAction = $"{Url.Action("Index")}?{Request.QueryString}"
            };
            return View("CreateEditConfirmation", confirmationVm);
        }
    }
}