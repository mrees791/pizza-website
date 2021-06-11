using DataLibrary.Models;
using DataLibrary.Models.QueryFilters;
using DataLibrary.Models.Tables;
using PizzaWebsite.Models;
using PizzaWebsite.Models.ManageEmployees;
using PizzaWebsite.Models.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DataLibrary.Models.JoinLists;
using PizzaWebsite.Controllers.BaseControllers;

namespace PizzaWebsite.Controllers
{
    [Authorize(Roles = "Admin,Executive,Manager")]
    public class ManageEmployeesController : BaseManageWebsiteController<Employee>
    {
        public async Task<ActionResult> Index(int? page, int? rowsPerPage, string employeeId, string userId)
        {
            ValidatePageQuery(ref page, ref rowsPerPage, 10);

            EmployeeFilter searchFilter = new EmployeeFilter()
            {
                Id = employeeId,
                UserId = userId
            };

            PaginationViewModel paginationVm = new PaginationViewModel();
            List<ManageEmployeeViewModel> employeeVmList = new List<ManageEmployeeViewModel>();
            IEnumerable<Employee> employeeList = await LoadPagedRecordsAsync(page.Value, rowsPerPage.Value, "Id", SortOrder.Ascending, searchFilter, PizzaDb, paginationVm);

            foreach (Employee employee in employeeList)
            {
                bool isManager = await UserManager.IsInRoleAsync(employee.UserId, "Manager");

                ManageEmployeeViewModel employeeVm = new ManageEmployeeViewModel()
                {
                    Id = employee.Id,
                    UserId = employee.UserId,
                    IsManager = isManager
                };
                employeeVmList.Add(employeeVm);
            }

            var viewModel = new ManagePagedListViewModel<ManageEmployeeViewModel>()
            {
                ItemViewModelList = employeeVmList,
                PaginationVm = paginationVm
            };

            return View(viewModel);
        }

        public async Task<ActionResult> ViewLocations(string id)
        {
            List<EmployeeLocationViewModel> employeeLocationVmList = new List<EmployeeLocationViewModel>();
            var joinList = new EmployeeLocationOnStoreLocationJoinList();
            await joinList.LoadListByEmployeeIdAsync(id, PizzaDb);

            foreach (Join<EmployeeLocation, StoreLocation> join in joinList.Items)
            {
                EmployeeLocationViewModel employeeLocationVm = new EmployeeLocationViewModel()
                {
                    Name = join.Table2.Name,
                    PhoneNumber = join.Table2.PhoneNumber,
                    City = join.Table2.City,
                    State = join.Table2.State,
                    ZipCode = join.Table2.ZipCode,
                    IsActiveLocation = join.Table2.IsActiveLocation
                };
                employeeLocationVmList.Add(employeeLocationVm);
            }

            ViewEmployeeLocationsViewModel viewModel = new ViewEmployeeLocationsViewModel()
            {
                EmployeeId = id,
                EmployeeLocationVmList = employeeLocationVmList
            };

            return View(viewModel);
        }

        public ActionResult AddEmployee()
        {
            return View("AddEmployee", new AddEmployeeViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddEmployee(AddEmployeeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            // Server side validation
            await ValidateViewModelAsync(viewModel);

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            // Attempt to add employee to database
            try
            {
                SiteUser siteUser = await PizzaDb.GetSiteUserByNameAsync(viewModel.UserId);
                await PizzaDb.Commands.AddNewEmployeeAsync(viewModel.Id, viewModel.IsManager, siteUser);
            }
            catch
            {
                // todo: Report error
                ModelState.AddModelError("", "Unable to add employee.");
                return View(viewModel);
            }

            ConfirmationViewModel confirmationVm = new ConfirmationViewModel()
            {
                ConfirmationMessage = $"Employee {viewModel.Id} has been added.",
                ReturnUrlAction = $"{Url.Action("Index")}?{Request.QueryString}"
            };

            return View("CreateEditConfirmation", confirmationVm);
        }

        public async Task<ActionResult> ManageEmployee(string id)
        {
            Employee employee = await PizzaDb.GetAsync<Employee>(id);
            bool isManager = await UserManager.IsInRoleAsync(employee.UserId, "Manager");

            ManageEmployeeViewModel viewModel = new ManageEmployeeViewModel()
            {
                Id = employee.Id,
                UserId = employee.UserId,
                IsManager = isManager
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ManageEmployee(ManageEmployeeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            Employee employee = await PizzaDb.GetAsync<Employee>(viewModel.Id);
            await PizzaDb.UpdateAsync(employee);

            if (viewModel.IsManager)
            {
                await UserManager.AddToRoleAsync(employee.UserId, "Manager");
            }
            else
            {
                await UserManager.RemoveFromRoleAsync(employee.UserId, "Manager");
            }

            ConfirmationViewModel confirmationVm = new ConfirmationViewModel()
            {
                ConfirmationMessage = $"Your changes to {viewModel.Id} have been confirmed.",
                ReturnUrlAction = $"{Url.Action("Index")}?{Request.QueryString}"
            };

            return View("CreateEditConfirmation", confirmationVm);
        }

        private async Task ValidateViewModelAsync(AddEmployeeViewModel viewModel)
        {
            // Check if user exists
            SiteUser siteUser = await PizzaDb.GetSiteUserByNameAsync(viewModel.UserId);

            if (siteUser == null)
            {
                ModelState.AddModelError("UserId", "User does not exist.");
            }

            // Make sure employee ID isn't already taken
            Employee employee = await PizzaDb.GetAsync<Employee>(viewModel.Id);

            if (employee != null)
            {
                ModelState.AddModelError("Id", "Employee ID is already taken.");
            }

            // Make sure user isn't already employed
            if (siteUser != null)
            {
                SiteRole employeeRole = await PizzaDb.GetSiteRoleByNameAsync("Employee");
                bool alreadyEmployed = await PizzaDb.UserIsInRole(siteUser, employeeRole);

                if (alreadyEmployed)
                {
                    ModelState.AddModelError("UserId", "User is already employed.");
                }
            }
        }
    }
}