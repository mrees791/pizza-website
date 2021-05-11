using DataLibrary.Models.QueryFilters;
using DataLibrary.Models.Tables;
using PizzaWebsite.Models;
using PizzaWebsite.Models.ManageWebsite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PizzaWebsite.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class ManageUsersController : BaseController
    {
        public async Task<ActionResult> Index(int? page, int? rowsPerPage, string userName, string email)
        {
            var manageUsersVm = new ManagePagedListViewModel<ManageUserViewModel>();

            SiteUserFilter searchFilter = new SiteUserFilter()
            {
                UserName = userName,
                Email = email
            };

            List<SiteUser> userList = await LoadPagedRecordsAsync<SiteUser>(page, rowsPerPage, "UserName", searchFilter, PizzaDb, Request, manageUsersVm.PaginationVm);

            foreach (SiteUser user in userList)
            {
                ManageUserViewModel model = new ManageUserViewModel()
                {
                    Id = user.Id,
                    Email = user.Email,
                    IsBanned = user.IsBanned,
                    UserName = user.UserName
                };
                manageUsersVm.ItemViewModelList.Add(model);
            }

            return View(manageUsersVm);
        }

        public async Task<ActionResult> ManageUser(int? id)
        {
            List<SiteUser> userList = new List<SiteUser>(await PizzaDb.GetListAsync<SiteUser>(new { Id = id.Value }));
            SiteUser user = userList.FirstOrDefault();

            ManageUserViewModel manageUserVm = new ManageUserViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                IsBanned = user.IsBanned,
                UserName = user.UserName
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

            List<SiteUser> userList = new List<SiteUser>(await PizzaDb.GetListAsync<SiteUser>(new { Id = model.Id }));
            SiteUser user = userList.FirstOrDefault();

            // Update user record
            user.IsBanned = model.IsBanned;
            await PizzaDb.UpdateAsync(user);

            ConfirmationViewModel confirmationModel = new ConfirmationViewModel();
            confirmationModel.ConfirmationMessage = $"Your changes to {model.UserName} have been confirmed.";
            confirmationModel.ReturnUrlAction = $"{Url.Action("Index")}?{Request.QueryString}";

            return View("CreateEditConfirmation", confirmationModel);
        }
    }
}