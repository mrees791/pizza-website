using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using DataLibrary.Models;
using DataLibrary.Models.JoinLists;
using DataLibrary.Models.QueryFilters;
using DataLibrary.Models.Tables;
using Microsoft.AspNet.Identity;
using PizzaWebsite.Controllers.BaseControllers;
using PizzaWebsite.Models;
using PizzaWebsite.Models.Employees;
using PizzaWebsite.Models.ManageStores;
using PizzaWebsite.Models.Services;

namespace PizzaWebsite.Controllers
{
    [Authorize(Roles = "Admin,Executive,Manager")]
    public class ManageStoresController : BaseManageWebsiteController<StoreLocation>
    {
        private readonly StoreServices _storeServices;

        public ManageStoresController()
        {
            _storeServices = new StoreServices();
        }

        public async Task<ActionResult> Index(int? page, int? rowsPerPage, string storeName, string phoneNumber)
        {
            ValidatePageQuery(ref page, ref rowsPerPage, 10);
            StoreLocationFilter searchFilter = new StoreLocationFilter
            {
                Name = storeName,
                PhoneNumber = phoneNumber
            };
            SiteUser currentUser = await GetCurrentUserAsync();
            Employee currentEmployee = await PizzaDb.GetEmployeeAsync(currentUser);
            PaginationViewModel paginationVm = new PaginationViewModel();
            List<ManageStoreViewModel> storeVmList = new List<ManageStoreViewModel>();
            IList<string> userRoleList = await UserManager.GetRolesAsync(User.Identity.GetUserId());
            IEnumerable<StoreLocation> storeList = await _storeServices.LoadAuthorizedStoreLocationListAsync(page.Value, rowsPerPage.Value, userRoleList,
                searchFilter, paginationVm, currentEmployee, Request, PizzaDb);
            foreach (StoreLocation store in storeList)
            {
                storeVmList.Add(RecordToViewModel(store));
            }

            ManagePagedListViewModel<ManageStoreViewModel> model = new ManagePagedListViewModel<ManageStoreViewModel>
            {
                ItemViewModelList = storeVmList,
                PaginationVm = paginationVm
            };
            return View(model);
        }

        private bool IsAuthorizedToManageAllStores()
        {
            return User.IsInRole("Admin") || User.IsInRole("Executive");
        }

        private async Task<bool> IsAuthorizedToManageStoreAsync(Employee employee, StoreLocation storeLocation)
        {
            if (IsAuthorizedToManageAllStores())
            {
                return true;
            }

            // Authorized if the employee is both currently employed at that store and is a manager.
            if (User.IsInRole("Manager"))
            {
                return await PizzaDb.Commands.IsEmployedAtLocationAsync(employee, storeLocation);
            }

            return false;
        }

        public async Task<ActionResult> RemoveEmployeeFromRoster(int id)
        {
            Employee currentEmployee = await GetCurrentEmployeeAsync();
            EmployeeLocation employeeLocation = await PizzaDb.GetAsync<EmployeeLocation>(id);
            StoreLocation store = await PizzaDb.GetAsync<StoreLocation>(employeeLocation.StoreId);
            Employee employee = await PizzaDb.GetAsync<Employee>(employeeLocation.EmployeeId);
            bool authorized = await IsAuthorizedToManageStoreAsync(currentEmployee, store);
            if (!authorized)
            {
                return NotAuthorizedToManageStoreErrorMessage(currentEmployee, store);
            }

            RemoveEmployeeFromRosterViewModel model = new RemoveEmployeeFromRosterViewModel
            {
                EmployeeId = employee.Id,
                EmployeeLocationId = employeeLocation.Id,
                StoreId = store.Id,
                StoreName = store.Name
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveEmployeeFromRoster(RemoveEmployeeFromRosterViewModel model)
        {
            Employee currentEmployee = await GetCurrentEmployeeAsync();
            StoreLocation store = await PizzaDb.GetAsync<StoreLocation>(model.StoreId);
            bool authorized = await IsAuthorizedToManageStoreAsync(currentEmployee, store);
            if (!authorized)
            {
                return NotAuthorizedToManageStoreErrorMessage(currentEmployee, store);
            }

            await ValidateViewModelAsync(model);
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Attempt to remove employee from store roster
            try
            {
                EmployeeLocation employeeLocation = await PizzaDb.GetAsync<EmployeeLocation>(model.EmployeeLocationId);
                await PizzaDb.DeleteAsync(employeeLocation);
            }
            catch
            {
                // todo: Report error
                ModelState.AddModelError("", "Error removing employee from roster.");
                return View(model);
            }

            ConfirmationViewModel confirmationVm = new ConfirmationViewModel
            {
                ConfirmationMessage = $"Employee {model.EmployeeId} has been removed from {model.StoreName}'s roster.",
                ReturnUrlAction = $"{Url.Action("EmployeeRoster")}/{model.StoreId}?{Request.QueryString}"
            };
            return View("CreateEditConfirmation", confirmationVm);
        }

        public async Task<ActionResult> AddEmployeeToRoster(int id)
        {
            Employee currentEmployee = await GetCurrentEmployeeAsync();
            StoreLocation store = await PizzaDb.GetAsync<StoreLocation>(id);
            bool authorized = await IsAuthorizedToManageStoreAsync(currentEmployee, store);
            if (!authorized)
            {
                return NotAuthorizedToManageStoreErrorMessage(currentEmployee, store);
            }

            AddEmployeeToRosterViewModel model = new AddEmployeeToRosterViewModel
            {
                StoreId = store.Id,
                StoreName = store.Name
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddEmployeeToRoster(AddEmployeeToRosterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Employee currentEmployee = await GetCurrentEmployeeAsync();
            StoreLocation store = await PizzaDb.GetAsync<StoreLocation>(model.StoreId);
            bool authorized = await IsAuthorizedToManageStoreAsync(currentEmployee, store);

            if (!authorized)
            {
                return NotAuthorizedToManageStoreErrorMessage(currentEmployee, store);
            }

            await ValidateViewModelAsync(model);
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Attempt to add employee to store roster
            try
            {
                EmployeeLocation employeeLocation = new EmployeeLocation
                {
                    EmployeeId = model.EmployeeId,
                    StoreId = model.StoreId
                };
                await PizzaDb.InsertAsync(employeeLocation);
            }
            catch
            {
                // todo: Report error
                ModelState.AddModelError("", "Unable to add employee to roster.");
                return View(model);
            }

            ConfirmationViewModel confirmationVm = new ConfirmationViewModel
            {
                ConfirmationMessage = $"Employee {model.EmployeeId} has been added to {model.StoreName}'s roster.",
                ReturnUrlAction = $"{Url.Action("EmployeeRoster")}/{model.StoreId}?{Request.QueryString}"
            };
            return View("CreateEditConfirmation", confirmationVm);
        }

        [Authorize(Roles = "Admin,Executive")]
        public ActionResult CreateStore()
        {
            ManageStoreViewModel model = new ManageStoreViewModel
            {
                StateList = GeographyServices.StateList
            };
            return View("ManageStore", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Executive")]
        public async Task<ActionResult> CreateStore(ManageStoreViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("ManageStore", model);
            }

            StoreLocation store = ViewModelToRecord(model);
            await PizzaDb.InsertAsync(store);
            ConfirmationViewModel confirmationVm = new ConfirmationViewModel
            {
                ConfirmationMessage = $"{model.Name} has been added to the database.",
                ReturnUrlAction = $"{Url.Action("Index")}?{Request.QueryString}"
            };
            return View("CreateEditConfirmation", confirmationVm);
        }

        public async Task<ActionResult> EditStore(int? id)
        {
            if (!id.HasValue)
            {
                return MissingStoreIdErrorMessage();
            }

            StoreLocation store = await PizzaDb.GetAsync<StoreLocation>(id.Value);
            if (store == null)
            {
                return StoreDoesNotExistErrorMessage(id.Value);
            }

            Employee currentEmployee = await PizzaDb.GetEmployeeAsync(await GetCurrentUserAsync());
            bool authorized = await IsAuthorizedToManageStoreAsync(currentEmployee, store);
            if (!authorized)
            {
                return NotAuthorizedToManageStoreErrorMessage(currentEmployee, store);
            }

            ManageStoreViewModel model = RecordToViewModel(store);
            return View("ManageStore", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditStore(ManageStoreViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("ManageStore", model);
            }

            StoreLocation store = await PizzaDb.GetAsync<StoreLocation>(model.Id);
            if (store == null)
            {
                return StoreDoesNotExistErrorMessage(model.Id);
            }

            Employee currentEmployee = await PizzaDb.GetEmployeeAsync(await GetCurrentUserAsync());
            bool authorized = await IsAuthorizedToManageStoreAsync(currentEmployee, store);
            if (!authorized)
            {
                return NotAuthorizedToManageStoreErrorMessage(currentEmployee, store);
            }

            await PizzaDb.UpdateAsync(ViewModelToRecord(model));
            ConfirmationViewModel confirmationVm = new ConfirmationViewModel
            {
                ConfirmationMessage = $"Your changes to {model.Name} have been confirmed.",
                ReturnUrlAction = $"{Url.Action("Index")}?{Request.QueryString}"
            };
            return View("CreateEditConfirmation", confirmationVm);
        }

        public async Task<ActionResult> EmployeeRoster(int id)
        {
            StoreLocation store = await PizzaDb.GetAsync<StoreLocation>(id);
            if (store == null)
            {
                return StoreDoesNotExistErrorMessage(id);
            }

            Employee currentEmployee = await PizzaDb.GetEmployeeAsync(await GetCurrentUserAsync());
            bool authorized = await IsAuthorizedToManageStoreAsync(currentEmployee, store);
            if (!authorized)
            {
                return NotAuthorizedToManageStoreErrorMessage(currentEmployee, store);
            }

            SiteRole managerRole = await PizzaDb.GetSiteRoleByNameAsync("Manager");
            List<EmployeeRosterItemViewModel> rosterVmList = new List<EmployeeRosterItemViewModel>();
            EmployeeOnEmployeeLocationJoinList joinList = new EmployeeOnEmployeeLocationJoinList();
            await joinList.LoadListByStoreIdAsync(store.Id, PizzaDb);
            foreach (Join<Employee, EmployeeLocation> join in joinList.Items)
            {
                SiteUser siteUser = await PizzaDb.GetSiteUserByIdAsync(join.Table1.UserId);
                bool isManager = await PizzaDb.UserIsInRole(siteUser, managerRole);
                EmployeeRosterItemViewModel itemVm = new EmployeeRosterItemViewModel
                {
                    EmployeeId = join.Table1.Id,
                    EmployeeLocationId = join.Table2.Id,
                    UserId = join.Table1.UserId,
                    IsManager = isManager
                };
                rosterVmList.Add(itemVm);
            }

            EmployeeRosterViewModel model = new EmployeeRosterViewModel
            {
                StoreId = store.Id,
                StoreName = store.Name,
                EmployeeRosterVmList = rosterVmList
            };
            return View(model);
        }

        private ActionResult NotAuthorizedToManageStoreErrorMessage(Employee employee, StoreLocation store)
        {
            ErrorMessageViewModel model = new ErrorMessageViewModel
            {
                Header = "Authorization Error",
                ErrorMessage = CreateManageStoreErrorMessage(employee.Id, store.Name),
                ReturnUrlAction = $"{Url.Action("Index")}?{Request.QueryString}",
                ShowReturnLink = true
            };
            return View("ErrorMessage", model);
        }

        private ActionResult MissingStoreIdErrorMessage()
        {
            ErrorMessageViewModel model = new ErrorMessageViewModel
            {
                Header = "Error",
                ErrorMessage = CreateMissingStoreIdMessage(),
                ReturnUrlAction = $"{Url.Action("Index")}?{Request.QueryString}",
                ShowReturnLink = true
            };
            return View("ErrorMessage", model);
        }

        private ActionResult StoreDoesNotExistErrorMessage(int storeId)
        {
            ErrorMessageViewModel model = new ErrorMessageViewModel
            {
                Header = "Error",
                ErrorMessage = CreateStoreDoesNotExistErrorMessage(storeId),
                ReturnUrlAction = $"{Url.Action("Index")}?{Request.QueryString}",
                ShowReturnLink = true
            };
            return View("ErrorMessage", model);
        }

        public ManageStoreViewModel RecordToViewModel(StoreLocation record)
        {
            return new ManageStoreViewModel
            {
                Id = record.Id,
                City = record.City,
                IsActiveLocation = record.IsActiveLocation,
                Name = record.Name,
                PhoneNumber = record.PhoneNumber,
                SelectedState = record.State,
                StreetAddress = record.StreetAddress,
                ZipCode = record.ZipCode,
                StateList = GeographyServices.StateList
            };
        }

        private StoreLocation ViewModelToRecord(ManageStoreViewModel model)
        {
            return new StoreLocation
            {
                Id = model.Id,
                City = model.City,
                IsActiveLocation = model.IsActiveLocation,
                Name = model.Name,
                PhoneNumber = model.PhoneNumber,
                State = model.SelectedState,
                StreetAddress = model.StreetAddress,
                ZipCode = model.ZipCode
            };
        }

        private string CreateMissingStoreIdMessage()
        {
            return "Store ID is missing.";
        }

        private string CreateStoreDoesNotExistErrorMessage(int storeId)
        {
            return $"Store with ID {storeId} does not exist.";
        }

        private string CreateManageStoreErrorMessage(string employeeId, string storeName)
        {
            return $"{employeeId} is not authorized to manage {storeName}.";
        }

        private async Task ValidateViewModelAsync(AddEmployeeToRosterViewModel model)
        {
            // Make sure employee exists.
            Employee employee = await PizzaDb.GetAsync<Employee>(model.EmployeeId);
            if (employee == null)
            {
                ModelState.AddModelError("EmployeeId", $"Employee with ID {model.EmployeeId} does not exist.");
            }
            else
            {
                // Make sure store exists.
                StoreLocation storeLocation = await PizzaDb.GetAsync<StoreLocation>(model.StoreId);
                if (storeLocation == null)
                {
                    ModelState.AddModelError("", $"Store with ID {model.StoreId} does not exist.");
                }
                else
                {
                    // Make sure employee isn't already employed at this location.
                    bool alreadyEmployedAtLocation =
                        await PizzaDb.Commands.IsEmployedAtLocationAsync(employee, storeLocation);
                    if (alreadyEmployedAtLocation)
                    {
                        ModelState.AddModelError("EmployeeId",
                            $"Employee with ID {model.EmployeeId} is already employed at this location.");
                    }
                }
            }
        }

        private async Task ValidateViewModelAsync(RemoveEmployeeFromRosterViewModel model)
        {
            // Make sure employee exists.
            Employee employee = await PizzaDb.GetAsync<Employee>(model.EmployeeId);
            if (employee == null)
            {
                ModelState.AddModelError("EmployeeId", $"Employee with ID {model.EmployeeId} does not exist.");
            }
            else
            {
                // Make sure store exists.
                StoreLocation storeLocation = await PizzaDb.GetAsync<StoreLocation>(model.StoreId);

                if (storeLocation == null)
                {
                    ModelState.AddModelError("", $"Store with ID {model.StoreId} does not exist.");
                }
                else
                {
                    // Make sure employee isn't already employed at this location.
                    bool isEmployedAtLocation = await PizzaDb.Commands.IsEmployedAtLocationAsync(employee, storeLocation);
                    if (!isEmployedAtLocation)
                    {
                        ModelState.AddModelError("EmployeeId",
                            $"Employee with ID {model.EmployeeId} is not employed at {model.StoreId}.");
                    }
                }
            }
        }
    }
}