using DataLibrary.Models;
using DataLibrary.Models.QueryFilters;
using DataLibrary.Models.Tables;
using PizzaWebsite.Models;
using PizzaWebsite.Models.ManageEmployees;
using PizzaWebsite.Models.ManageWebsite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PizzaWebsite.Controllers
{
    [Authorize(Roles = "Admin,Executive")]
    public class ManageEmployeesController : BaseManageWebsiteController<Employee>
    {
        public async Task<ActionResult> Index(int? page, int? rowsPerPage, string employeeId, string userId)
        {
            var manageEmployeesVm = new ManagePagedListViewModel<ManageEmployeeViewModel>();

            EmployeeFilter searchFilter = new EmployeeFilter()
            {
                Id = employeeId,
                UserId = userId
            };

            List<Employee> employeeList = await LoadPagedRecordsAsync(page, rowsPerPage, "Id", SortOrder.Ascending, searchFilter, PizzaDb, Request,
                manageEmployeesVm.PaginationVm);

            foreach (Employee employee in employeeList)
            {
                SiteUser siteUser = await PizzaDb.GetSiteUserByIdAsync(employee.UserId);
                bool isManager = await UserManager.IsInRoleAsync(employee.UserId, "Manager");

                ManageEmployeeViewModel model = new ManageEmployeeViewModel()
                {
                    Id = employee.Id,
                    UserId = employee.UserId,
                    IsManager = isManager
                };
                manageEmployeesVm.ItemViewModelList.Add(model);
            }

            return View(manageEmployeesVm);
        }

        public async Task<ActionResult> ViewLocations(string id)
        {
            ViewEmployeeLocationsViewModel model = new ViewEmployeeLocationsViewModel();
            await model.InitializeAsync(id, PizzaDb);

            return View(model);
        }

        public ActionResult AddEmployee()
        {
            return View("AddEmployee", new AddEmployeeViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddEmployee(AddEmployeeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Server side validation
            await model.ValidateAsync(ModelState, PizzaDb);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Attempt to add employee to database
            try
            {
                SiteUser siteUser = await PizzaDb.GetSiteUserByNameAsync(model.UserId);
                await PizzaDb.Commands.AddNewEmployeeAsync(model.Id, model.IsManager, siteUser);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }

            // Show confirmation page
            ConfirmationViewModel confirmationModel = new ConfirmationViewModel()
            {
                ConfirmationMessage = $"Employee {model.Id} has been added.",
                ReturnUrlAction = $"{Url.Action("Index")}?{Request.QueryString}"
            };

            return View("CreateEditConfirmation", confirmationModel);
        }

        public async Task<ActionResult> ManageEmployee(string id)
        {
            Employee employee = await PizzaDb.GetAsync<Employee>(id);
            bool isManager = await UserManager.IsInRoleAsync(employee.UserId, "Manager");

            ManageEmployeeViewModel model = new ManageEmployeeViewModel()
            {
                Id = employee.Id,
                UserId = employee.UserId,
                IsManager = isManager
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ManageEmployee(ManageEmployeeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Employee employee = await PizzaDb.GetAsync<Employee>(model.Id);
            await PizzaDb.UpdateAsync(employee);

            if (model.IsManager)
            {
                await UserManager.AddToRoleAsync(employee.UserId, "Manager");
            }
            else
            {
                await UserManager.RemoveFromRoleAsync(employee.UserId, "Manager");
            }

            ConfirmationViewModel confirmationModel = new ConfirmationViewModel();
            confirmationModel.ConfirmationMessage = $"Your changes to {model.Id} have been confirmed.";
            confirmationModel.ReturnUrlAction = $"{Url.Action("Index")}?{Request.QueryString}";

            return View("CreateEditConfirmation", confirmationModel);
        }
    }
}