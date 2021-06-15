using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using DataLibrary.Models;
using DataLibrary.Models.JoinLists;
using DataLibrary.Models.QuerySearches;
using DataLibrary.Models.Tables;
using Microsoft.AspNet.Identity;
using PizzaWebsite.Controllers.BaseControllers;
using PizzaWebsite.Models;
using PizzaWebsite.Models.Shop;
using PizzaWebsite.Models.ViewModelServices;

namespace PizzaWebsite.Controllers
{
    [Authorize]
    public class ManagePreviousOrdersController : BaseController
    {
        private readonly PreviousOrderServices _previousOrderServices;

        public ManagePreviousOrdersController()
        {
            _previousOrderServices = new PreviousOrderServices();
        }

        public async Task<ActionResult> Index(int? page, int? rowsPerPage)
        {
            if (!page.HasValue)
            {
                page = 1;
            }

            if (!rowsPerPage.HasValue)
            {
                rowsPerPage = 10;
            }

            PreviousOrderSearch search = new PreviousOrderSearch
            {
                UserId = User.Identity.GetUserId()
            };
            PaginationViewModel paginationVm = new PaginationViewModel
            {
                QueryString = Request.QueryString,
                CurrentPage = page.Value,
                RowsPerPage = rowsPerPage.Value,
                TotalNumberOfItems = await PizzaDb.GetNumberOfRecordsAsync<CustomerOrder>(search),
                TotalPages = await PizzaDb.GetNumberOfPagesAsync<CustomerOrder>(rowsPerPage.Value, search)
            };
            IEnumerable<int> quantityList = ListServices.DefaultQuantityList;
            List<PreviousOrderViewModel> previousOrderVmList = new List<PreviousOrderViewModel>();
            IEnumerable<CustomerOrder> previousOrderList =
                await PizzaDb.GetPagedListAsync<CustomerOrder>(page.Value, rowsPerPage.Value, "Id",
                    SortOrder.Descending, search);
            foreach (CustomerOrder prevOrder in previousOrderList)
            {
                PreviousOrderViewModel orderVm =
                    await _previousOrderServices.CreateViewModelAsync(false, prevOrder, null, PizzaDb, quantityList);
                previousOrderVmList.Add(orderVm);
            }

            PreviousOrderListViewModel previousOrdersVm = new PreviousOrderListViewModel
            {
                PaginationVm = paginationVm,
                PreviousOrderVmList = previousOrderVmList
            };
            return View(previousOrdersVm);
        }

        public async Task<ActionResult> PreviousOrder(int? id)
        {
            if (!id.HasValue)
            {
                return PreviousOrderMissingIdErrorMessage();
            }

            Join<CustomerOrder, DeliveryInfo> orderJoin = await GetFirstOrDefaultCustomerOrderAsync(id.Value);
            if (orderJoin == null)
            {
                return PreviousOrderDoesNotExistErrorMessage();
            }

            SiteUser currentUser = await GetCurrentUserAsync();
            bool authorizedToViewOrder =
                !await PizzaDb.Commands.UserOwnsCustomerOrderAsync(currentUser, orderJoin.Table1);
            if (authorizedToViewOrder)
            {
                return PreviousOrderAuthorizationErrorMessage();
            }

            PreviousOrderViewModel model = await _previousOrderServices.CreateViewModelAsync(true, orderJoin.Table1,
                orderJoin.Table2, PizzaDb, ListServices.DefaultQuantityList);
            return View(model);
        }

        public async Task<ActionResult> OrderAgain(int? id)
        {
            if (!id.HasValue)
            {
                return PreviousOrderMissingIdErrorMessage();
            }

            CustomerOrder customerOrder = await PizzaDb.GetAsync<CustomerOrder>(id);
            if (customerOrder == null)
            {
                return PreviousOrderDoesNotExistErrorMessage();
            }

            SiteUser currentUser = await GetCurrentUserAsync();
            bool authorized = await PizzaDb.Commands.UserOwnsCustomerOrderAsync(currentUser, customerOrder);
            if (!authorized)
            {
                return PreviousOrderAuthorizationErrorMessage();
            }

            await PizzaDb.Commands.ReorderPreviousOrder(currentUser, customerOrder);
            return RedirectToAction("Cart", "Shop");
        }

        public async Task<ActionResult> PreviousOrderStatus(int? id)
        {
            if (!id.HasValue)
            {
                return PreviousOrderMissingIdErrorMessage();
            }

            Join<CustomerOrder, DeliveryInfo> orderJoin = await GetFirstOrDefaultCustomerOrderAsync(id.Value);
            if (orderJoin == null)
            {
                return PreviousOrderDoesNotExistErrorMessage();
            }

            SiteUser currentUser = await GetCurrentUserAsync();
            bool authorizedToViewOrder =
                !await PizzaDb.Commands.UserOwnsCustomerOrderAsync(currentUser, orderJoin.Table1);
            if (authorizedToViewOrder)
            {
                return PreviousOrderAuthorizationErrorMessage();
            }

            OrderStatusViewModel model = new OrderStatusViewModel
            {
                CustomerOrderId = id.Value
            };
            return View(model);
        }

        private async Task<Join<CustomerOrder, DeliveryInfo>> GetFirstOrDefaultCustomerOrderAsync(int customerOrderId)
        {
            CustomerOrderOnDeliveryInfoJoinList join = new CustomerOrderOnDeliveryInfoJoinList();
            await join.LoadFirstOrDefaultByCustomerOrderIdAsync(customerOrderId, PizzaDb);
            return join.Items.FirstOrDefault();
        }

        private ActionResult PreviousOrderMissingIdErrorMessage()
        {
            ErrorMessageViewModel model = new ErrorMessageViewModel
            {
                Header = "Error",
                ErrorMessage = "Order ID is missing.",
                ReturnUrlAction = $"{Url.Action("Index")}?{Request.QueryString}",
                ShowReturnLink = true
            };
            return View("ErrorMessage", model);
        }

        private ActionResult PreviousOrderDoesNotExistErrorMessage()
        {
            ErrorMessageViewModel model = new ErrorMessageViewModel
            {
                Header = "Error",
                ErrorMessage = "This order does not exist.",
                ReturnUrlAction = $"{Url.Action("Index")}?{Request.QueryString}",
                ShowReturnLink = true
            };
            return View("ErrorMessage", model);
        }

        private ActionResult PreviousOrderAuthorizationErrorMessage()
        {
            ErrorMessageViewModel model = new ErrorMessageViewModel
            {
                Header = "Authorization Error",
                ErrorMessage = "You are not authorized to access this order.",
                ReturnUrlAction = $"{Url.Action("Index")}?{Request.QueryString}",
                ShowReturnLink = true
            };
            return View("ErrorMessage", model);
        }
    }
}