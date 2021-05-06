using DataLibrary.Models.OldTables;
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
    public class ManageUsersController : BaseController
    {
        public async Task<ActionResult> Index(int? page, int? rowsPerPage, string userName, string email)
        {
            var manageUsersVm = new ManagePagedListViewModel<ManageUserViewModel>();

            object searchFilters = new
            {
                UserName = userName,
                Email = email
            };

            List<SiteUser> userEntities = await LoadPagedEntitiesAsync<SiteUser>(PizzaDb, Request, manageUsersVm.PaginationVm, page, rowsPerPage, "UserName", searchFilters);

            foreach (SiteUser userEntity in userEntities)
            {
                ManageUserViewModel model = new ManageUserViewModel()
                {
                    Id = userEntity.Id,
                    Email = userEntity.Email,
                    IsBanned = userEntity.IsBanned,
                    UserName = userEntity.UserName
                };
                manageUsersVm.ItemViewModelList.Add(model);
            }

            return View(manageUsersVm);
        }

        public async Task<ActionResult> ManageUser(int? id)
        {
            List<SiteUser> userEntities = await PizzaDb.GetListAsync<SiteUser>(new { Id = id.Value });
            SiteUser userEntity = userEntities.FirstOrDefault();

            ManageUserViewModel manageUserVm = new ManageUserViewModel()
            {
                Id = userEntity.Id,
                Email = userEntity.Email,
                IsBanned = userEntity.IsBanned,
                UserName = userEntity.UserName
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

            List<SiteUser> userEntities = await PizzaDb.GetListAsync<SiteUser>(new { Id = model.Id });
            SiteUser userEntity = userEntities.FirstOrDefault();

            // Update user record
            userEntity.IsBanned = model.IsBanned;
            PizzaDb.Update(userEntity);

            ConfirmationViewModel confirmationModel = new ConfirmationViewModel();
            confirmationModel.ConfirmationMessage = $"Your changes to {model.UserName} have been confirmed.";
            confirmationModel.ReturnUrlAction = $"{Url.Action("Index")}?{Request.QueryString}";

            return View("CreateEditConfirmation", confirmationModel);
        }
    }
}