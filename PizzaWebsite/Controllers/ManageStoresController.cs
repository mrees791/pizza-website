using DataLibrary.Models;
using DataLibrary.Models.JoinLists;
using DataLibrary.Models.QueryFilters;
using DataLibrary.Models.Tables;
using PizzaWebsite.Models;
using PizzaWebsite.Models.Employees;
using PizzaWebsite.Models.ManageStores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PizzaWebsite.Controllers
{
    [Authorize(Roles = "Admin,Executive,Manager")]
    public class ManageStoresController : BaseManageWebsiteController<StoreLocation>
    {
        public async Task<ActionResult> Index(int? page, int? rowsPerPage, string storeName, string phoneNumber)
        {
            ValidatePageQuery(ref page, ref rowsPerPage, 10);
            var manageStoresVm = new ManagePagedListViewModel<ManageStoreViewModel>();

            StoreLocationFilter searchFilter = new StoreLocationFilter()
            {
                Name = storeName,
                PhoneNumber = phoneNumber
            };

            IEnumerable<StoreLocation> storeList = await LoadAuthorizedStoreLocationListAsync(page.Value, rowsPerPage.Value, searchFilter, manageStoresVm.PaginationVm);

            foreach (StoreLocation store in storeList)
            {
                ManageStoreViewModel model = RecordToViewModel(store);
                manageStoresVm.ItemViewModelList.Add(model);
            }

            return View(manageStoresVm);
        }

        private async Task<IEnumerable<StoreLocation>> LoadAuthorizedStoreLocationListAsync(int page, int rowsPerPage, StoreLocationFilter searchFilter, PaginationViewModel paginationVm)
        {
            if (IsAuthorizedToManageAllStores())
            {
                return await LoadPagedRecordsAsync(page, rowsPerPage, "Name", SortOrder.Ascending, searchFilter, PizzaDb, paginationVm);
            }

            // Only select stores that the current user is employed at.
            return await LoadEmployedStoreLocationListAsync(page, rowsPerPage, searchFilter, paginationVm);
        }

        /// <summary>
        /// Only select stores that the current user is employed at.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rowsPerPage"></param>
        /// <param name="searchFilter"></param>
        /// <param name="paginationVm"></param>
        /// <returns></returns>
        private async Task<IEnumerable<StoreLocation>> LoadEmployedStoreLocationListAsync(int page, int rowsPerPage, StoreLocationFilter searchFilter, PaginationViewModel paginationVm)
        {
            Employee employee = await PizzaDb.GetEmployeeAsync(await GetCurrentUserAsync());
            var joinList = new EmployeeLocationOnStoreLocationJoinList();
            await joinList.LoadPagedListByEmployeeIdAsync(employee.Id, searchFilter, page, rowsPerPage, PizzaDb);
            int totalNumberOfItems = await joinList.GetNumberOfResultsByEmployeeIdAsync(employee.Id, searchFilter, rowsPerPage, PizzaDb);
            int totalPages = await joinList.GetNumberOfPagesByEmployeeIdAsync(employee.Id, searchFilter, rowsPerPage, PizzaDb);

            // Navigation pane
            paginationVm.QueryString = Request.QueryString;
            paginationVm.CurrentPage = page;
            paginationVm.RowsPerPage = rowsPerPage;
            paginationVm.TotalPages = totalPages;
            paginationVm.TotalNumberOfItems = totalNumberOfItems;

            return joinList.Items.Select(j => j.Table2);
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
                return await PizzaDb.Commands.IsEmployedAtLocation(employee, storeLocation);
            }

            return false;
        }

        public async Task<ActionResult> RemoveEmployeeFromRoster(int id)
        {
            Employee currentEmployee = await GetCurrentEmployeeAsync();
            EmployeeLocation employeeLocation = await PizzaDb.GetAsync<EmployeeLocation>(id);
            StoreLocation storeLocation = await PizzaDb.GetAsync<StoreLocation>(employeeLocation.StoreId);
            Employee employee = await PizzaDb.GetAsync<Employee>(employeeLocation.EmployeeId);

            bool authorized = await IsAuthorizedToManageStoreAsync(currentEmployee, storeLocation);
            if (!authorized)
            {
                throw new Exception(CreateManageStoreErrorMessage(currentEmployee.Id, storeLocation.Id));
            }

            RemoveEmployeeFromRosterViewModel model = new RemoveEmployeeFromRosterViewModel()
            {
                EmployeeId = employee.Id,
                EmployeeLocationId = employeeLocation.Id,
                StoreId = storeLocation.Id,
                StoreName = storeLocation.Name
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveEmployeeFromRoster(RemoveEmployeeFromRosterViewModel model)
        {
            Employee currentEmployee = await GetCurrentEmployeeAsync();
            StoreLocation storeLocation = await PizzaDb.GetAsync<StoreLocation>(model.StoreId);

            bool authorized = await IsAuthorizedToManageStoreAsync(currentEmployee, storeLocation);
            if (!authorized)
            {
                throw new Exception(CreateManageStoreErrorMessage(model.EmployeeId, model.StoreId));
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
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error removing employee from roster.");
                return View(model);
            }

            // Show confirmation page
            ConfirmationViewModel confirmationModel = new ConfirmationViewModel()
            {
                ConfirmationMessage = $"Employee {model.EmployeeId} has been removed from {model.StoreName}'s roster.",
                ReturnUrlAction = $"{Url.Action("EmployeeRoster")}/{model.StoreId}?{Request.QueryString}"
            };

            return View("CreateEditConfirmation", confirmationModel);
        }

        public async Task<ActionResult> AddEmployeeToRoster(int id)
        {
            Employee currentEmployee = await GetCurrentEmployeeAsync();
            StoreLocation storeLocation = await PizzaDb.GetAsync<StoreLocation>(id);

            bool authorized = await IsAuthorizedToManageStoreAsync(currentEmployee, storeLocation);
            if (!authorized)
            {
                throw new Exception(CreateManageStoreErrorMessage(currentEmployee.Id, storeLocation.Id));
            }

            AddEmployeeToRosterViewModel addEmployeeVm = new AddEmployeeToRosterViewModel()
            {
                StoreId = storeLocation.Id,
                StoreName = storeLocation.Name
            };

            return View(addEmployeeVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddEmployeeToRoster(AddEmployeeToRosterViewModel model)
        {
            Employee currentEmployee = await GetCurrentEmployeeAsync();
            StoreLocation storeLocation = await PizzaDb.GetAsync<StoreLocation>(model.StoreId);

            bool authorized = await IsAuthorizedToManageStoreAsync(currentEmployee, storeLocation);
            if (!authorized)
            {
                throw new Exception(CreateManageStoreErrorMessage(currentEmployee.Id, storeLocation.Id));
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Server side validation
            await ValidateViewModelAsync(model);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Attempt to add employee to store roster
            try
            {
                EmployeeLocation employeeLocation = new EmployeeLocation()
                {
                    EmployeeId = model.EmployeeId,
                    StoreId = model.StoreId
                };

                await PizzaDb.InsertAsync(employeeLocation);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to add employee to roster.");
                return View(model);
            }

            // Show confirmation page
            ConfirmationViewModel confirmationModel = new ConfirmationViewModel()
            {
                ConfirmationMessage = $"Employee {model.EmployeeId} has been added to {model.StoreName}'s roster.",
                ReturnUrlAction = $"{Url.Action("EmployeeRoster")}/{model.StoreId}?{Request.QueryString}"
            };

            return View("CreateEditConfirmation", confirmationModel);
        }

        [Authorize(Roles = "Admin,Executive")]
        public ActionResult CreateStore()
        {
            ManageStoreViewModel model = new ManageStoreViewModel();
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

            ConfirmationViewModel confirmationModel = new ConfirmationViewModel();
            confirmationModel.ConfirmationMessage = $"{model.Name} has been added to the database.";
            confirmationModel.ReturnUrlAction = $"{Url.Action("Index")}?{Request.QueryString}";

            return View("CreateEditConfirmation", confirmationModel);
        }

        public async Task<ActionResult> EditStore(int? id)
        {
            if (!id.HasValue)
            {
                throw new Exception($"Missing store ID.");
            }

            StoreLocation store = await PizzaDb.GetAsync<StoreLocation>(id.Value);

            if (store == null)
            {
                throw new Exception($"Store with ID {id.Value} does not exist.");
            }

            Employee currentEmployee = await PizzaDb.GetEmployeeAsync(await GetCurrentUserAsync());
            ManageStoreViewModel model = RecordToViewModel(store);

            bool authorized = await IsAuthorizedToManageStoreAsync(currentEmployee, store);
            if (!authorized)
            {
                throw new Exception(CreateManageStoreErrorMessage(currentEmployee.Id, store.Id));
            }

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

            Employee currentEmployee = await PizzaDb.GetEmployeeAsync(await GetCurrentUserAsync());
            StoreLocation store = await PizzaDb.GetAsync<StoreLocation>(model.Id);

            bool authorized = await IsAuthorizedToManageStoreAsync(currentEmployee, store);
            if (!authorized)
            {
                throw new Exception(CreateManageStoreErrorMessage(currentEmployee.Id, store.Id));
            }

            await PizzaDb.UpdateAsync(ViewModelToRecord(model));

            ConfirmationViewModel confirmationModel = new ConfirmationViewModel();
            confirmationModel.ConfirmationMessage = $"Your changes to {model.Name} have been confirmed.";
            confirmationModel.ReturnUrlAction = $"{Url.Action("Index")}?{Request.QueryString}";

            return View("CreateEditConfirmation", confirmationModel);
        }

        public async Task<ActionResult> EmployeeRoster(int id)
        {
            Employee currentEmployee = await PizzaDb.GetEmployeeAsync(await GetCurrentUserAsync());
            StoreLocation store = await PizzaDb.GetAsync<StoreLocation>(id);

            bool authorized = await IsAuthorizedToManageStoreAsync(currentEmployee, store);
            if (!authorized)
            {
                throw new Exception(CreateManageStoreErrorMessage(currentEmployee.Id, store.Id));
            }

            EmployeeRosterViewModel rosterVm = new EmployeeRosterViewModel();
            await rosterVm.InitializeAsync(id, PizzaDb);

            return View(rosterVm);
        }

        public ManageStoreViewModel RecordToViewModel(StoreLocation record)
        {
            return new ManageStoreViewModel()
            {
                Id = record.Id,
                City = record.City,
                IsActiveLocation = record.IsActiveLocation,
                Name = record.Name,
                PhoneNumber = record.PhoneNumber,
                SelectedState = record.State,
                StreetAddress = record.StreetAddress,
                ZipCode = record.ZipCode
            };
        }

        private StoreLocation ViewModelToRecord(ManageStoreViewModel model)
        {
            return new StoreLocation()
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

        private string CreateManageStoreErrorMessage(string employeeId, int storeId)
        {
            return $"Employee with ID {employeeId} is not allowed to manage store with ID {storeId}.";
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
                    bool alreadyEmployedAtLocation = await PizzaDb.Commands.IsEmployedAtLocation(employee, storeLocation);

                    if (alreadyEmployedAtLocation)
                    {
                        ModelState.AddModelError("EmployeeId", $"Employee with ID {model.EmployeeId} is already employed at this location.");
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
                    bool isEmployedAtLocation = await PizzaDb.Commands.IsEmployedAtLocation(employee, storeLocation);

                    if (!isEmployedAtLocation)
                    {
                        ModelState.AddModelError("EmployeeId", $"Employee with ID {model.EmployeeId} is not employed at {model.StoreId}.");
                    }
                }
            }
        }
    }
}