using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Runtime.Remoting.Messaging;
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
        private readonly IEnumerable<SelectListItem> _orderStatusDeliveryItems;
        private readonly IEnumerable<SelectListItem> _orderStatusPickupItems;

        public ManageOrdersController()
        {
            _customerOrderServices = new CustomerOrderServices();
            _storeServices = new StoreServices();
            _cartServices = new CartServices();
            _orderStatusDeliveryItems = CreateOrderStatusSelectListItems(true);
            _orderStatusPickupItems = CreateOrderStatusSelectListItems(false);
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
            bool isAuthorized = await AuthorizedToManageStoreOrders(currentEmployee, store);
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
            List<OrderListItemViewModel> orderVmList = new List<OrderListItemViewModel>();
            foreach (CustomerOrder customerOrder in customerOrderList)
            {
                CartViewModel cartVm = await _cartServices.CreateViewModelAsync(customerOrder.CartId, PizzaDb, ListServices.DefaultQuantityList);
                CustomerOrderViewModel customerOrderVm = await _customerOrderServices.CreateViewModelAsync(false,
                    customerOrder, null, PizzaDb, ListServices.DefaultQuantityList);
                OrderListItemViewModel listItemVm = new OrderListItemViewModel()
                {
                    CustomerOrderVm = customerOrderVm,
                    SelectedOrderStatus = customerOrder.OrderStatus,
                    OrderStatusListItems = GetOrderStatusSelectListItems(customerOrder.IsDelivery),
                    OrderStatusSelectId = $"orderStatusSelect-{customerOrder.Id}"
                };
                orderVmList.Add(listItemVm);
            }
            StoreOrderListViewModel model = new StoreOrderListViewModel()
            {
                OrderListItemVmList = orderVmList,
                PaginationVm = paginationVm,
                StoreSearchQueryString = CreateStoreSearchQueryString()
            };

            return View("StoreOrderList", model);
        }

        private IEnumerable<SelectListItem> GetOrderStatusSelectListItems(bool isDelivery)
        {
            if (isDelivery)
            {
                return _orderStatusDeliveryItems;
            }

            return _orderStatusPickupItems;
        }

        private IEnumerable<SelectListItem> CreateOrderStatusSelectListItems(bool isDelivery)
        {
            List<SelectListItem> itemList = new List<SelectListItem>
            {
                GetOrderStatusSelectListItem(OrderStatus.Cancelled),
                GetOrderStatusSelectListItem(OrderStatus.OrderPlaced),
                GetOrderStatusSelectListItem(OrderStatus.Prep),
                GetOrderStatusSelectListItem(OrderStatus.Bake),
                GetOrderStatusSelectListItem(OrderStatus.Box)
            };

            if (isDelivery)
            {
                itemList.Add(GetOrderStatusSelectListItem(OrderStatus.OutForDelivery));
            }
            else
            {
                itemList.Add(GetOrderStatusSelectListItem(OrderStatus.ReadyForPickup));
            }

            itemList.Add(GetOrderStatusSelectListItem(OrderStatus.Complete));

            return itemList;
        }


        private SelectListItem GetOrderStatusSelectListItem(OrderStatus orderStatus)
        {
            switch (orderStatus)
            {
                case OrderStatus.OrderPlaced:
                    return new SelectListItem()
                    {
                        Text = "Order Placed",
                        Value = $"{(int)OrderStatus.OrderPlaced}"
                    };
                case OrderStatus.Prep:
                    return new SelectListItem()
                    {
                        Text = "Prepping",
                        Value = $"{(int)OrderStatus.Prep}"
                    };
                case OrderStatus.Bake:
                    return new SelectListItem()
                    {
                        Text = "Baking",
                        Value = $"{(int)OrderStatus.Bake}"
                    };
                case OrderStatus.Box:
                    return new SelectListItem()
                    {
                        Text = "Boxing",
                        Value = $"{(int)OrderStatus.Box}"
                    };
                case OrderStatus.ReadyForPickup:
                    return new SelectListItem()
                    {
                        Text = "Ready for Pickup",
                        Value = $"{(int)OrderStatus.ReadyForPickup}"
                    };
                case OrderStatus.OutForDelivery:
                    return new SelectListItem()
                    {
                        Text = "Out for Delivery",
                        Value = $"{(int)OrderStatus.OutForDelivery}"
                    };
                case OrderStatus.Complete:
                    return new SelectListItem()
                    {
                        Text = "Complete",
                        Value = $"{(int)OrderStatus.Complete}"
                    };
                case OrderStatus.Cancelled:
                    return new SelectListItem()
                    {
                        Text = "Cancelled",
                        Value = $"{(int)OrderStatus.Cancelled}"
                    };
            }

            throw new Exception($"Unable to get order phase status for {orderStatus.ToString()}");
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
            if (storeLocation == null)
            {
                return StoreDoesNotExistErrorMessage(customerOrder.StoreId);
            }
            SiteUser currentUser = await GetCurrentUserAsync();
            Employee currentEmployee = await PizzaDb.GetEmployeeAsync(currentUser);
            bool isAuthorized = await AuthorizedToManageStoreOrders(currentEmployee, storeLocation);

            if (!isAuthorized)
            {
                return OrderAuthorizationErrorMessage(id.Value);
            }

            CustomerOrderViewModel customerOrderVm = await _customerOrderServices.CreateViewModelAsync(true, customerOrder,
                deliveryInfo, PizzaDb, ListServices.DefaultQuantityList);

            StoreOrderCartViewModel model = new StoreOrderCartViewModel()
            {
                CustomerOrderVm = customerOrderVm,
                StoreSearchQueryString = CreateStoreSearchQueryString()
            };

            return View("StoreOrder", model);
        }

        private bool AuthorizedToAllStores()
        {
            return User.IsInRole("Admin") || User.IsInRole("Executive");
        }

        private async Task<bool> AuthorizedToManageStoreOrders(Employee employee, StoreLocation store)
        {
            return AuthorizedToAllStores() || await PizzaDb.Commands.IsEmployedAtLocationAsync(employee, store);
        }

        private string CreateStoreSearchQueryString()
        {
            return $"Page={Request["Page"]}&RowsPerPage={Request["RowsPerPage"]}&Name={Request["Name"]}&PhoneNumber={Request["PhoneNumber"]}";
        }

        [HttpPost]
        public async Task<ActionResult> UpdateOrderStatusAjax(int customerOrderId, int orderStatus)
        {
            Response.StatusCode = (int)HttpStatusCode.OK;

            SiteUser currentUser = await GetCurrentUserAsync();
            if (currentUser == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json($"Current user cannot be found.",
                    MediaTypeNames.Text.Plain);
            }

            CustomerOrder customerOrder = await PizzaDb.GetAsync<CustomerOrder>(customerOrderId);

            if (customerOrder == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json($"Order with ID {customerOrderId} does not exist.",
                    MediaTypeNames.Text.Plain);
            }

            StoreLocation store = await PizzaDb.GetAsync<StoreLocation>(customerOrder.StoreId);

            if (store == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json($"Store with ID {customerOrder.StoreId} does not exist.",
                    MediaTypeNames.Text.Plain);
            }

            Employee currentEmployee = await PizzaDb.GetEmployeeAsync(currentUser);

            if (currentEmployee == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json($"Employee ID cannot be found for user with ID {currentUser.Id}.",
                    MediaTypeNames.Text.Plain);
            }

            bool authorized = await AuthorizedToManageStoreOrders(currentEmployee, store);

            if (!authorized)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json($"{currentEmployee.Id} is not authorized to manage order ID {customerOrder.Id}.",
                    MediaTypeNames.Text.Plain);
            }

            // Update record
            customerOrder.OrderStatus = orderStatus;
            int rowsUpdated = await PizzaDb.UpdateAsync(customerOrder);

            if (rowsUpdated == 0)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json($"0 rows updated when updating order {customerOrder.Id}.",
                    MediaTypeNames.Text.Plain);
            }

            string responseText = $"{rowsUpdated} records updated.";
            return Json(responseText, MediaTypeNames.Text.Plain);
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