﻿using DataLibrary.Models;
using DataLibrary.Models.JoinLists;
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
            if (IsAuthorizedToSeeAllStores())
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
        private async Task<IEnumerable<StoreLocation>> LoadEmployedStoreLocationListAsync(int? page, int? rowsPerPage, StoreLocationFilter searchFilter, PaginationViewModel paginationVm)
        {
            /*if (!page.HasValue)
            {
                page = 1;
            }
            if (!rowsPerPage.HasValue)
            {
                rowsPerPage = 10;
            }*/

            Employee employee = await PizzaDb.GetEmployeeAsync(await GetCurrentUserAsync());
            var joinList = new EmployeeLocationOnStoreLocationJoinList();

            await joinList.LoadPagedListByEmployeeIdAsync(employee.Id, searchFilter, page.Value, rowsPerPage.Value, PizzaDb);
            int totalNumberOfItems = await joinList.GetNumberOfResultsByEmployeeIdAsync(employee.Id, searchFilter, rowsPerPage.Value, PizzaDb);
            int totalPages = await joinList.GetNumberOfPagesByEmployeeIdAsync(employee.Id, searchFilter, rowsPerPage.Value, PizzaDb);

            // Navigation pane
            paginationVm.QueryString = Request.QueryString;
            paginationVm.CurrentPage = page.Value;
            paginationVm.RowsPerPage = rowsPerPage.Value;
            paginationVm.TotalPages = totalPages;
            paginationVm.TotalNumberOfItems = totalNumberOfItems;

            return joinList.Items.Select(j => j.Table2);
        }

        private bool IsAuthorizedToSeeAllStores()
        {
            return User.IsInRole("Admin") || User.IsInRole("Executive");
        }

        public async Task<ActionResult> RemoveEmployeeFromRoster(int id)
        {
            RemoveEmployeeFromRosterViewModel removeEmployeeVm = new RemoveEmployeeFromRosterViewModel();
            await removeEmployeeVm.InitializeAsync(id, PizzaDb);

            return View(removeEmployeeVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveEmployeeFromRoster(RemoveEmployeeFromRosterViewModel model)
        {
            await model.InitializeAsync(model.EmployeeLocationId, PizzaDb);
            await model.ValidateAsync(ModelState, PizzaDb);

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
            AddEmployeeToRosterViewModel addEmployeeVm = new AddEmployeeToRosterViewModel();
            await addEmployeeVm.InitializeAsync(id, PizzaDb);

            return View(addEmployeeVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddEmployeeToRoster(AddEmployeeToRosterViewModel model)
        {
            await model.InitializeAsync(model.StoreId, PizzaDb);

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
            StoreLocation store = await PizzaDb.GetAsync<StoreLocation>(id.Value);
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

            await PizzaDb.UpdateAsync(ViewModelToRecord(model));

            ConfirmationViewModel confirmationModel = new ConfirmationViewModel();
            confirmationModel.ConfirmationMessage = $"Your changes to {model.Name} have been confirmed.";
            confirmationModel.ReturnUrlAction = $"{Url.Action("Index")}?{Request.QueryString}";

            return View("CreateEditConfirmation", confirmationModel);
        }

        public async Task<ActionResult> EmployeeRoster(int id)
        {
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
    }
}