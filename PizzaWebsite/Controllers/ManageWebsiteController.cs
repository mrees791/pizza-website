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
    public class ManageWebsiteController : BaseController
    {

        // GET: ManageWebsite
        public ActionResult Index()
        {
            return View();
        }


        /*

        public async Task<ActionResult> ManageEmployee(string id)
        {
            List<Employee> employeeRecords = await PizzaDb.GetListAsync<Employee>(new { Id = id });
            Employee employee = employeeRecords.FirstOrDefault();

            ManageEmployeeViewModel manageEmployeeVm = new ManageEmployeeViewModel();
            manageEmployeeVm.FromEntity(employee);
            manageEmployeeVm.IsManager = await UserManager.IsInRoleAsync(employee.UserId, "Manager");
            return View("ManageEmployee", manageEmployeeVm);
        }

        public async Task<ActionResult> ManageUser(int? id)
        {
            List<SiteUser> userRecords = await PizzaDb.GetListAsync<SiteUser>(new { Id = id.Value });
            SiteUser user = userRecords.FirstOrDefault();

            ManageUserViewModel manageUserVm = new ManageUserViewModel();
            manageUserVm.FromEntity(user);
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

            // Find user record
            List<SiteUser> userRecords = await PizzaDb.GetListAsync<SiteUser>(new { Id = model.Id });
            SiteUser user = userRecords.FirstOrDefault();

            // Update user record
            user.IsBanned = model.IsBanned;
            PizzaDb.Update(user);

            ConfirmationViewModel confirmationModel = new ConfirmationViewModel();
            confirmationModel.ConfirmationMessage = $"Your changes to {model.UserName} have been confirmed.";
            confirmationModel.ReturnUrlAction = $"{Url.Action("ManageUsers")}?{Request.QueryString}";

            return View("CreateEditConfirmation", confirmationModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateStoreLocation(ManageStoreLocationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateEditStoreLocation", model);
            }

            PizzaDb.Insert(model.ToEntity());

            ConfirmationViewModel confirmationModel = new ConfirmationViewModel();
            confirmationModel.ConfirmationMessage = $"{model.Name} has been added to the database.";
            confirmationModel.ReturnUrlAction = $"{Url.Action("ManageStores")}?{Request.QueryString}";

            return View("CreateEditConfirmation", confirmationModel);
        }

        public ActionResult CreateStoreLocation()
        {
            ManageStoreLocationViewModel model = new ManageStoreLocationViewModel();
            return View("CreateEditStoreLocation", model);
        }

        public async Task<ActionResult> ManageEmployees(int? page, int? rowsPerPage, string employeeId)
        {
            var manageEmployeesVm = new PagedListViewModel<ManageEmployeeViewModel, Employee>();

            object searchFilters = new
            {
                Id = employeeId
            };

            await manageEmployeesVm.LoadViewModelRecordsAsync(PizzaDb, Request, page, rowsPerPage, "Id", searchFilters);

            return View(manageEmployeesVm);
        }

        public async Task<ActionResult> ManageStores(int? page, int? rowsPerPage, string storeName, string phoneNumber)
        {
            var manageStoresVm = new PagedListViewModel<ManageStoreLocationViewModel, StoreLocation>();

            object searchFilters = new
            {
                Name = storeName,
                PhoneNumber = phoneNumber
            };

            await manageStoresVm.LoadViewModelRecordsAsync(PizzaDb, Request, page, rowsPerPage, "Name", searchFilters);

            return View(manageStoresVm);
        }

        public async Task<ActionResult> ManageUsers(int? page, int? rowsPerPage, string userName, string email)
        {
            var manageUsersVm = new PagedListViewModel<ManageUserViewModel, SiteUser>();

            object searchFilters = new
            {
                UserName = userName,
                Email = email
            };

            await manageUsersVm.LoadViewModelRecordsAsync(PizzaDb, Request, page, rowsPerPage, "UserName", searchFilters);

            return View(manageUsersVm);
        }*/
    }
}