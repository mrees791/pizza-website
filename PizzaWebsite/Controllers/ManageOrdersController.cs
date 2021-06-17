﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DataLibrary.Models;
using DataLibrary.Models.JoinLists;
using DataLibrary.Models.QueryFilters;
using DataLibrary.Models.Tables;
using Microsoft.AspNet.Identity;
using PizzaWebsite.Controllers.BaseControllers;
using PizzaWebsite.Models;
using PizzaWebsite.Models.Carts;
using PizzaWebsite.Models.ManageOrders;
using PizzaWebsite.Models.ManageStores;
using PizzaWebsite.Models.Services;
using PizzaWebsite.Models.ViewModelServices;

namespace PizzaWebsite.Controllers
{
    /// <summary>
    /// Used by employees for managing and updating customer orders.
    /// </summary>
    [Authorize(Roles = "Employee")]
    public class ManageOrdersController : BaseController
    {
        private readonly CustomerOrderServices _customerOrderServices;
        private readonly StoreServices _storeServices;
        private readonly CartServices _cartServices;

        public ManageOrdersController()
        {
            _customerOrderServices = new CustomerOrderServices();
            _storeServices = new StoreServices();
            _cartServices = new CartServices();
        }

        public async Task<ActionResult> Index(int? page, int? rowsPerPage, string name, string phoneNumber)
        {
            ValidatePageQuery(ref page, ref rowsPerPage, 10);
            StoreLocationFilter searchFilter = new StoreLocationFilter
            {
                Name = name,
                PhoneNumber = phoneNumber
            };

            // Load authorized store list.
            SiteUser currentUser = await GetCurrentUserAsync();
            Employee currentEmployee = await PizzaDb.GetEmployeeAsync(currentUser);
            PaginationViewModel paginationVm = new PaginationViewModel();
            List<StoreViewModel> storeVmList = new List<StoreViewModel>();
            IList<string> userRoleList = await UserManager.GetRolesAsync(User.Identity.GetUserId());
            IEnumerable<StoreLocation> storeList = await _storeServices.LoadAuthorizedStoreLocationListAsync(page.Value, rowsPerPage.Value, userRoleList,
                searchFilter, paginationVm, currentEmployee, Request, PizzaDb);

            foreach (StoreLocation store in storeList)
            {
                string manageOrdersButtonHref = $"{Url.Action("Store")}/{store.Id}?{CreateStoreSearchQueryString()}&OrdersPage=1&OrdersRowsPerPage=10";

                storeVmList.Add(new StoreViewModel()
                {
                    Id = store.Id,
                    Name = store.Name,
                    City = store.City,
                    PhoneNumber = store.PhoneNumber,
                    StreetAddress = store.StreetAddress,
                    ZipCode = store.ZipCode,
                    IsActiveLocation = store.IsActiveLocation,
                    ManageOrdersButtonHref = manageOrdersButtonHref
                });
            }

            SearchStoresViewModel model = new SearchStoresViewModel()
            {
                StoreVmList = storeVmList,
                PaginationVm = paginationVm
            };

            return View("SearchStores", model);
        }

        public async Task<ActionResult> Store(int? id, int? ordersPage, int? ordersRowsPerPage, string ordersUserId)
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
            SiteUser currentUser = await GetCurrentUserAsync();
            Employee currentEmployee = await PizzaDb.GetEmployeeAsync(currentUser);
            bool isAuthorized = AuthorizedToAllStores() || await PizzaDb.Commands.IsEmployedAtLocationAsync(currentEmployee, store);
            if (!isAuthorized)
            {
                return StoreAuthorizationErrorMessage(id.Value);
            }
            // Load paged orders list.
            ValidatePageQuery(ref ordersPage, ref ordersRowsPerPage, 10);
            StoreOrderFilter searchFilter = new StoreOrderFilter()
            {
                StoreId = id.Value,
                UserId = ordersUserId
            };
            IEnumerable<CustomerOrder> customerOrderList =
                await PizzaDb.GetPagedListAsync<CustomerOrder>(ordersPage.Value, ordersRowsPerPage.Value, "Id", SortOrder.Descending, searchFilter);
            int totalPages = await PizzaDb.GetNumberOfPagesAsync<CustomerOrder>(ordersRowsPerPage.Value, searchFilter);
            int totalNumberOfItems = await PizzaDb.GetNumberOfRecordsAsync<CustomerOrder>(searchFilter);
            // Create view models
            PaginationViewModel paginationVm = new PaginationViewModel()
            {
                CurrentPage = ordersPage.Value,
                RowsPerPage = ordersRowsPerPage.Value,
                QueryString = Request.QueryString,
                TotalNumberOfItems = totalNumberOfItems,
                TotalPages = totalPages
            };
            List<CustomerOrderViewModel> orderVmList = new List<CustomerOrderViewModel>();
            foreach (CustomerOrder customerOrder in customerOrderList)
            {
                CartViewModel cartVm = await _cartServices.CreateViewModelAsync(customerOrder.CartId, PizzaDb, ListServices.DefaultQuantityList);
                CustomerOrderViewModel customerOrderVm = await _customerOrderServices.CreateViewModelAsync(false,
                    customerOrder, null, PizzaDb, ListServices.DefaultQuantityList);
                orderVmList.Add(customerOrderVm);
            }
            StoreOrderListViewModel model = new StoreOrderListViewModel()
            {
                StoreOrderVmList = orderVmList,
                PaginationVm = paginationVm,
                StoreSearchQueryString = CreateStoreSearchQueryString()
            };

            return View("StoreOrderList", model);
        }

        public async Task<ActionResult> ViewOrder(int? id)
        {
            if (!id.HasValue)
            {
                return MissingOrderIdErrorMessage();
            }

            var joinList = new CustomerOrderOnDeliveryInfoJoinList();
            await joinList.LoadFirstOrDefaultByCustomerOrderIdAsync(id.Value, PizzaDb);
            Join<CustomerOrder, DeliveryInfo> orderJoin = joinList.Items.FirstOrDefault();
            if (orderJoin == null)
            {
                return OrderDoesNotExistErrorMessage(id.Value);
            }

            CustomerOrder customerOrder = orderJoin.Table1;
            DeliveryInfo deliveryInfo = orderJoin.Table2;
            StoreLocation storeLocation = await PizzaDb.GetAsync<StoreLocation>(customerOrder.StoreId);
            SiteUser currentUser = await GetCurrentUserAsync();
            Employee currentEmployee = await PizzaDb.GetEmployeeAsync(currentUser);
            bool isAuthorized = await AuthorizedToViewOrderAsync(customerOrder, currentEmployee, storeLocation);

            if (!isAuthorized)
            {
                return OrderAuthorizationErrorMessage(id.Value);
            }

            CustomerOrderViewModel customerOrderVm = await _customerOrderServices.CreateViewModelAsync(true, customerOrder,
                deliveryInfo, PizzaDb, ListServices.DefaultQuantityList);

            StoreOrderViewModel model = new StoreOrderViewModel()
            {
                CustomerOrderVm = customerOrderVm,
                StoreSearchQueryString = CreateStoreSearchQueryString()
            };

            return View("StoreOrder", model);
        }

        private async Task<bool> AuthorizedToViewOrderAsync(CustomerOrder customerOrder, Employee employee, StoreLocation storeLocation)
        {
            if (AuthorizedToAllStores())
            {
                return true;
            }
            return await PizzaDb.Commands.IsEmployedAtLocationAsync(employee, storeLocation);
        }

        private bool AuthorizedToAllStores()
        {
            return User.IsInRole("Admin") || User.IsInRole("Executive");
        }

        private string CreateStoreSearchQueryString()
        {
            return $"Page={Request["Page"]}&RowsPerPage={Request["RowsPerPage"]}&Name={Request["Name"]}&PhoneNumber={Request["PhoneNumber"]}";
        }

        private ActionResult StoreAuthorizationErrorMessage(int storeId)
        {
            ErrorMessageViewModel model = new ErrorMessageViewModel
            {
                Header = "Authorization Error",
                ErrorMessage = $"You are not authorized to access store with ID {storeId}.",
                ReturnUrlAction = $"{Url.Action("Index")}?{CreateStoreSearchQueryString()}",
                ShowReturnLink = true
            };
            return View("ErrorMessage", model);
        }

        private ActionResult StoreDoesNotExistErrorMessage(int storeId)
        {
            ErrorMessageViewModel model = new ErrorMessageViewModel
            {
                Header = "Error",
                ErrorMessage = $"Store with ID {storeId} does not exist.",
                ReturnUrlAction = $"{Url.Action("Index")}?{CreateStoreSearchQueryString()}",
                ShowReturnLink = true
            };
            return View("ErrorMessage", model);
        }

        private ActionResult MissingStoreIdErrorMessage()
        {
            ErrorMessageViewModel model = new ErrorMessageViewModel
            {
                Header = "Error",
                ErrorMessage = "Missing store ID.",
                ReturnUrlAction = $"{Url.Action("Index")}?{CreateStoreSearchQueryString()}",
                ShowReturnLink = true
            };
            return View("ErrorMessage", model);
        }

        private ActionResult MissingOrderIdErrorMessage()
        {
            ErrorMessageViewModel model = new ErrorMessageViewModel
            {
                Header = "Error",
                ErrorMessage = "Missing order ID.",
                ReturnUrlAction = $"{Url.Action("Index")}?{CreateStoreSearchQueryString()}",
                ShowReturnLink = true
            };
            return View("ErrorMessage", model);
        }

        private ActionResult OrderDoesNotExistErrorMessage(int orderId)
        {
            ErrorMessageViewModel model = new ErrorMessageViewModel
            {
                Header = "Error",
                ErrorMessage = $"Order with ID {orderId} does not exist.",
                ReturnUrlAction = $"{Url.Action("Index")}?{CreateStoreSearchQueryString()}",
                ShowReturnLink = true
            };
            return View("ErrorMessage", model);
        }

        private ActionResult OrderAuthorizationErrorMessage(int orderId)
        {
            ErrorMessageViewModel model = new ErrorMessageViewModel
            {
                Header = "Authorization Error",
                ErrorMessage = $"You are not authorized to view order ID {orderId}.",
                ReturnUrlAction = $"{Url.Action("Index")}?{CreateStoreSearchQueryString()}",
                ShowReturnLink = true
            };
            return View("ErrorMessage", model);
        }
    }
}