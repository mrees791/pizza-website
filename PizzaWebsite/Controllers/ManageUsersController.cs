﻿using DataLibrary.Models;
using DataLibrary.Models.QueryFilters;
using DataLibrary.Models.Tables;
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
    [Authorize(Roles = "Admin,Executive")]
    public class ManageUsersController : BaseManageWebsiteController<SiteUser>
    {
        public async Task<ActionResult> Index(int? page, int? rowsPerPage, string userId, string email)
        {
            ValidatePageQuery(ref page, ref rowsPerPage, 10);
            var manageUsersVm = new ManagePagedListViewModel<ManageUserViewModel>();

            SiteUserFilter searchFilter = new SiteUserFilter()
            {
                Id = userId,
                Email = email
            };

            IEnumerable<SiteUser> userList = await LoadPagedRecordsAsync(page.Value, rowsPerPage.Value, "Id", SortOrder.Ascending, searchFilter, PizzaDb, manageUsersVm.PaginationVm);

            foreach (SiteUser user in userList)
            {
                ManageUserViewModel model = new ManageUserViewModel()
                {
                    Id = user.Id,
                    Email = user.Email,
                    IsBanned = user.IsBanned
                };
                manageUsersVm.ItemViewModelList.Add(model);
            }

            return View(manageUsersVm);
        }

        public string FromUrlSafeId(string urlSafeId)
        {
            return urlSafeId.Replace("(dot)", ".");
        }

        public async Task<ActionResult> ManageUser(string id)
        {
            SiteUser user = await PizzaDb.GetSiteUserByIdAsync(FromUrlSafeId(id));

            ManageUserViewModel manageUserVm = new ManageUserViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                IsBanned = user.IsBanned
            };
            
            return View("ManageUser", manageUserVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ManageUser(ManageUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("ManageUser", model);
            }

            string id = FromUrlSafeId(model.Id);

            SiteUser user = await PizzaDb.GetSiteUserByIdAsync(id);

            // Update user record
            user.IsBanned = model.IsBanned;
            int rowsAffected = await PizzaDb.UpdateAsync(user);

            if (rowsAffected == 0)
            {
                ModelState.AddModelError("", $"Unable to update user with ID: {user.Id}");
                return View("ManageUser", model);
            }

            ConfirmationViewModel confirmationModel = new ConfirmationViewModel();
            confirmationModel.ConfirmationMessage = $"Your changes to {id} have been confirmed.";
            confirmationModel.ReturnUrlAction = $"{Url.Action("Index")}?{Request.QueryString}";

            return View("CreateEditConfirmation", confirmationModel);
        }
    }
}