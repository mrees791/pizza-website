using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using DataLibrary.Models;
using DataLibrary.Models.JoinLists;
using DataLibrary.Models.QueryFilters;
using DataLibrary.Models.Tables;
using PizzaWebsite.Controllers.BaseControllers;
using PizzaWebsite.Models;
using PizzaWebsite.Models.Employees;
using PizzaWebsite.Models.ManageEmployees;

namespace PizzaWebsite.Controllers
{
    [Authorize(Roles = "Admin,Executive,Manager")]
    public class ManageEmployeesController : BaseManageWebsiteController<Employee>
    {
        public async Task<ActionResult> Index(int? page, int? rowsPerPage, string employeeId, string userId)
        {
            ValidatePageQuery(ref page, ref rowsPerPage, 10);
            EmployeeFilter searchFilter = new EmployeeFilter
            {
                Id = employeeId,
                UserId = userId
            };
            PaginationViewModel paginationVm = new PaginationViewModel();
            List<ManageEmployeeViewModel> employeeVmList = new List<ManageEmployeeViewModel>();
            IEnumerable<Employee> employeeList = await LoadPagedRecordsAsync(page.Value, rowsPerPage.Value, "Id",
                SortOrder.Ascending, searchFilter, PizzaDb, paginationVm);
            foreach (Employee employee in employeeList)
            {
                bool isManager = await UserManager.IsInRoleAsync(employee.UserId, "Manager");
                ManageEmployeeViewModel employeeVm = new ManageEmployeeViewModel
                {
                    Id = employee.Id,
                    UserId = employee.UserId,
                    IsManager = isManager
                };
                employeeVmList.Add(employeeVm);
            }

            ManagePagedListViewModel<ManageEmployeeViewModel> model =
                new ManagePagedListViewModel<ManageEmployeeViewModel>
                {
                    ItemViewModelList = employeeVmList,
                    PaginationVm = paginationVm
                };
            return View(model);
        }

        public async Task<ActionResult> ViewLocations(string id)
        {
            List<EmployeeLocationViewModel> employeeLocationVmList = new List<EmployeeLocationViewModel>();
            EmployeeLocationOnStoreLocationJoinList joinList = new EmployeeLocationOnStoreLocationJoinList();
            await joinList.LoadListByEmployeeIdAsync(id, PizzaDb);
            foreach (Join<EmployeeLocation, StoreLocation> join in joinList.Items)
            {
                EmployeeLocationViewModel employeeLocationVm = new EmployeeLocationViewModel
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

            ViewEmployeeLocationsViewModel model = new ViewEmployeeLocationsViewModel
            {
                EmployeeId = id,
                EmployeeLocationVmList = employeeLocationVmList
            };
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

            await ValidateViewModelAsync(model);
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
            catch
            {
                // todo: Report error
                ModelState.AddModelError("", "Unable to add employee.");
                return View(model);
            }

            ConfirmationViewModel confirmationVm = new ConfirmationViewModel
            {
                ConfirmationMessage = $"Employee {model.Id} has been added.",
                ReturnUrlAction = $"{Url.Action("Index")}?{Request.QueryString}"
            };
            return View("CreateEditConfirmation", confirmationVm);
        }

        public async Task<ActionResult> ManageEmployee(string id)
        {
            Employee employee = await PizzaDb.GetAsync<Employee>(id);
            bool isManager = await UserManager.IsInRoleAsync(employee.UserId, "Manager");
            ManageEmployeeViewModel model = new ManageEmployeeViewModel
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

            ConfirmationViewModel confirmationVm = new ConfirmationViewModel
            {
                ConfirmationMessage = $"Your changes to {model.Id} have been confirmed.",
                ReturnUrlAction = $"{Url.Action("Index")}?{Request.QueryString}"
            };
            return View("CreateEditConfirmation", confirmationVm);
        }

        private async Task ValidateViewModelAsync(AddEmployeeViewModel model)
        {
            SiteUser siteUser = await PizzaDb.GetSiteUserByNameAsync(model.UserId);
            if (siteUser == null)
            {
                ModelState.AddModelError("UserId", "User does not exist.");
            }

            if (!await EmployeeIdIsAvailable(model.Id))
            {
                ModelState.AddModelError("Id", "Employee ID is already taken.");
            }

            if (siteUser != null)
            {
                if (await AlreadyEmployed(siteUser, model.Id))
                {
                    ModelState.AddModelError("UserId", "User is already employed.");
                }
            }
        }

        private async Task<bool> EmployeeIdIsAvailable(string employeeId)
        {
            return await PizzaDb.GetAsync<Employee>(employeeId) == null ? true : false;
        }

        private async Task<bool> AlreadyEmployed(SiteUser siteUser, string employeeId)
        {
            SiteRole employeeRole = await PizzaDb.GetSiteRoleByNameAsync("Employee");
            return await PizzaDb.UserIsInRole(siteUser, employeeRole);
        }
    }
}