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

            StoreLocationFilter searchFilter = new StoreLocationFilter()
            {
                Name = storeName,
                PhoneNumber = phoneNumber
            };

            PaginationViewModel paginationVm = new PaginationViewModel();
            List<ManageStoreViewModel> storeVmList = new List<ManageStoreViewModel>();
            IEnumerable<StoreLocation> storeList = await LoadAuthorizedStoreLocationListAsync(page.Value, rowsPerPage.Value, searchFilter, paginationVm);

            foreach (StoreLocation store in storeList)
            {
                storeVmList.Add(RecordToViewModel(store));
            }

            var manageStoresVm = new ManagePagedListViewModel<ManageStoreViewModel>()
            {
                ItemViewModelList = storeVmList,
                PaginationVm = paginationVm
            };

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
            Employee currentEmployee = await PizzaDb.GetEmployeeAsync(await GetCurrentUserAsync());
            var joinList = new EmployeeLocationOnStoreLocationJoinList();
            await joinList.LoadPagedListByEmployeeIdAsync(currentEmployee.Id, searchFilter, page, rowsPerPage, PizzaDb);
            int totalNumberOfItems = await joinList.GetNumberOfResultsByEmployeeIdAsync(currentEmployee.Id, searchFilter, rowsPerPage, PizzaDb);
            int totalPages = await joinList.GetNumberOfPagesByEmployeeIdAsync(currentEmployee.Id, searchFilter, rowsPerPage, PizzaDb);

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
            StoreLocation store = await PizzaDb.GetAsync<StoreLocation>(employeeLocation.StoreId);
            Employee employee = await PizzaDb.GetAsync<Employee>(employeeLocation.EmployeeId);
            bool authorized = await IsAuthorizedToManageStoreAsync(currentEmployee, store);

            if (!authorized)
            {
                return NotAuthorizedToManageStoreErrorMessage(currentEmployee, store);
            }

            RemoveEmployeeFromRosterViewModel viewModel = new RemoveEmployeeFromRosterViewModel()
            {
                EmployeeId = employee.Id,
                EmployeeLocationId = employeeLocation.Id,
                StoreId = store.Id,
                StoreName = store.Name
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveEmployeeFromRoster(RemoveEmployeeFromRosterViewModel viewModel)
        {
            Employee currentEmployee = await GetCurrentEmployeeAsync();
            StoreLocation store = await PizzaDb.GetAsync<StoreLocation>(viewModel.StoreId);
            bool authorized = await IsAuthorizedToManageStoreAsync(currentEmployee, store);

            if (!authorized)
            {
                return NotAuthorizedToManageStoreErrorMessage(currentEmployee, store);
            }

            await ValidateViewModelAsync(viewModel);

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            // Attempt to remove employee from store roster
            try
            {
                EmployeeLocation employeeLocation = await PizzaDb.GetAsync<EmployeeLocation>(viewModel.EmployeeLocationId);
                await PizzaDb.DeleteAsync(employeeLocation);
            }
            catch
            {
                // todo: Report error
                ModelState.AddModelError("", "Error removing employee from roster.");
                return View(viewModel);
            }

            ConfirmationViewModel confirmationVm = new ConfirmationViewModel()
            {
                ConfirmationMessage = $"Employee {viewModel.EmployeeId} has been removed from {viewModel.StoreName}'s roster.",
                ReturnUrlAction = $"{Url.Action("EmployeeRoster")}/{viewModel.StoreId}?{Request.QueryString}"
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

            AddEmployeeToRosterViewModel viewModel = new AddEmployeeToRosterViewModel()
            {
                StoreId = store.Id,
                StoreName = store.Name
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddEmployeeToRoster(AddEmployeeToRosterViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            Employee currentEmployee = await GetCurrentEmployeeAsync();
            StoreLocation store = await PizzaDb.GetAsync<StoreLocation>(viewModel.StoreId);
            bool authorized = await IsAuthorizedToManageStoreAsync(currentEmployee, store);

            if (!authorized)
            {
                return NotAuthorizedToManageStoreErrorMessage(currentEmployee, store);
            }

            // Server side validation
            await ValidateViewModelAsync(viewModel);

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            // Attempt to add employee to store roster
            try
            {
                EmployeeLocation employeeLocation = new EmployeeLocation()
                {
                    EmployeeId = viewModel.EmployeeId,
                    StoreId = viewModel.StoreId
                };

                await PizzaDb.InsertAsync(employeeLocation);
            }
            catch
            {
                // todo: Report error
                ModelState.AddModelError("", "Unable to add employee to roster.");
                return View(viewModel);
            }

            ConfirmationViewModel confirmationVm = new ConfirmationViewModel()
            {
                ConfirmationMessage = $"Employee {viewModel.EmployeeId} has been added to {viewModel.StoreName}'s roster.",
                ReturnUrlAction = $"{Url.Action("EmployeeRoster")}/{viewModel.StoreId}?{Request.QueryString}"
            };

            return View("CreateEditConfirmation", confirmationVm);
        }

        [Authorize(Roles = "Admin,Executive")]
        public ActionResult CreateStore()
        {
            return View("ManageStore", new ManageStoreViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Executive")]
        public async Task<ActionResult> CreateStore(ManageStoreViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("ManageStore", viewModel);
            }

            StoreLocation store = ViewModelToRecord(viewModel);
            await PizzaDb.InsertAsync(store);

            ConfirmationViewModel confirmationVm = new ConfirmationViewModel()
            {
                ConfirmationMessage = $"{viewModel.Name} has been added to the database.",
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
            ManageStoreViewModel viewModel = RecordToViewModel(store);
            bool authorized = await IsAuthorizedToManageStoreAsync(currentEmployee, store);

            if (!authorized)
            {
                return NotAuthorizedToManageStoreErrorMessage(currentEmployee, store);
            }

            return View("ManageStore", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditStore(ManageStoreViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("ManageStore", viewModel);
            }

            StoreLocation store = await PizzaDb.GetAsync<StoreLocation>(viewModel.Id);

            if (store == null)
            {
                return StoreDoesNotExistErrorMessage(viewModel.Id);
            }

            Employee currentEmployee = await PizzaDb.GetEmployeeAsync(await GetCurrentUserAsync());
            bool authorized = await IsAuthorizedToManageStoreAsync(currentEmployee, store);

            if (!authorized)
            {
                return NotAuthorizedToManageStoreErrorMessage(currentEmployee, store);
            }

            await PizzaDb.UpdateAsync(ViewModelToRecord(viewModel));

            ConfirmationViewModel confirmationModel = new ConfirmationViewModel();
            confirmationModel.ConfirmationMessage = $"Your changes to {viewModel.Name} have been confirmed.";
            confirmationModel.ReturnUrlAction = $"{Url.Action("Index")}?{Request.QueryString}";

            return View("CreateEditConfirmation", confirmationModel);
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

            var rosterVmList = new List<EmployeeRosterItemViewModel>();
            SiteRole managerRole = await PizzaDb.GetSiteRoleByNameAsync("Manager");
            var joinList = new EmployeeOnEmployeeLocationJoinList();
            await joinList.LoadListByStoreIdAsync(store.Id, PizzaDb);

            foreach (Join<Employee, EmployeeLocation> join in joinList.Items)
            {
                SiteUser siteUser = await PizzaDb.GetSiteUserByIdAsync(join.Table1.UserId);
                bool isManager = await PizzaDb.UserIsInRole(siteUser, managerRole);

                EmployeeRosterItemViewModel itemVm = new EmployeeRosterItemViewModel()
                {
                    EmployeeId = join.Table1.Id,
                    EmployeeLocationId = join.Table2.Id,
                    UserId = join.Table1.UserId,
                    IsManager = isManager
                };

                rosterVmList.Add(itemVm);
            }

            EmployeeRosterViewModel rosterVm = new EmployeeRosterViewModel()
            {
                StoreId = store.Id,
                StoreName = store.Name,
                EmployeeRosterVmList = rosterVmList
            };

            return View(rosterVm);
        }

        private ActionResult NotAuthorizedToManageStoreErrorMessage(Employee employee, StoreLocation store)
        {
            ErrorMessageViewModel viewModel = new ErrorMessageViewModel()
            {
                Header = "Authorization Error",
                ErrorMessage = CreateManageStoreErrorMessage(employee.Id, store.Name),
                ReturnUrlAction = $"{Url.Action("Index")}?{Request.QueryString}",
                ShowReturnLink = true
            };

            return View("ErrorMessage", viewModel);
        }

        private ActionResult MissingStoreIdErrorMessage()
        {
            ErrorMessageViewModel viewModel = new ErrorMessageViewModel()
            {
                Header = "Error",
                ErrorMessage = CreateMissingStoreIdMessage(),
                ReturnUrlAction = $"{Url.Action("Index")}?{Request.QueryString}",
                ShowReturnLink = true
            };

            return View("ErrorMessage", viewModel);
        }

        private ActionResult StoreDoesNotExistErrorMessage(int storeId)
        {
            ErrorMessageViewModel viewModel = new ErrorMessageViewModel()
            {
                Header = "Error",
                ErrorMessage = CreateStoreDoesNotExistErrorMessage(storeId),
                ReturnUrlAction = $"{Url.Action("Index")}?{Request.QueryString}",
                ShowReturnLink = true
            };

            return View("ErrorMessage", viewModel);
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

        private string CreateMissingStoreIdMessage()
        {
            return $"Store ID is missing.";
        }

        private string CreateStoreDoesNotExistErrorMessage(int storeId)
        {
            return $"Store with ID {storeId} does not exist.";
        }

        private string CreateManageStoreErrorMessage(string employeeId, string storeName)
        {
            return $"{employeeId} is not authorized to manage {storeName}.";
        }

        private async Task ValidateViewModelAsync(AddEmployeeToRosterViewModel viewModel)
        {
            // Make sure employee exists.
            Employee employee = await PizzaDb.GetAsync<Employee>(viewModel.EmployeeId);

            if (employee == null)
            {
                ModelState.AddModelError("EmployeeId", $"Employee with ID {viewModel.EmployeeId} does not exist.");
            }
            else
            {
                // Make sure store exists.
                StoreLocation storeLocation = await PizzaDb.GetAsync<StoreLocation>(viewModel.StoreId);

                if (storeLocation == null)
                {
                    ModelState.AddModelError("", $"Store with ID {viewModel.StoreId} does not exist.");
                }
                else
                {
                    // Make sure employee isn't already employed at this location.
                    bool alreadyEmployedAtLocation = await PizzaDb.Commands.IsEmployedAtLocation(employee, storeLocation);

                    if (alreadyEmployedAtLocation)
                    {
                        ModelState.AddModelError("EmployeeId", $"Employee with ID {viewModel.EmployeeId} is already employed at this location.");
                    }
                }
            }
        }

        private async Task ValidateViewModelAsync(RemoveEmployeeFromRosterViewModel viewModel)
        {
            // Make sure employee exists.
            Employee employee = await PizzaDb.GetAsync<Employee>(viewModel.EmployeeId);

            if (employee == null)
            {
                ModelState.AddModelError("EmployeeId", $"Employee with ID {viewModel.EmployeeId} does not exist.");
            }
            else
            {
                // Make sure store exists.
                StoreLocation storeLocation = await PizzaDb.GetAsync<StoreLocation>(viewModel.StoreId);

                if (storeLocation == null)
                {
                    ModelState.AddModelError("", $"Store with ID {viewModel.StoreId} does not exist.");
                }
                else
                {
                    // Make sure employee isn't already employed at this location.
                    bool isEmployedAtLocation = await PizzaDb.Commands.IsEmployedAtLocation(employee, storeLocation);

                    if (!isEmployedAtLocation)
                    {
                        ModelState.AddModelError("EmployeeId", $"Employee with ID {viewModel.EmployeeId} is not employed at {viewModel.StoreId}.");
                    }
                }
            }
        }
    }
}