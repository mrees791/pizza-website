using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PizzaWebsite.Models.ManageOrders
{
    public class StoreOrderListViewModel
    {
        public CustomerOrderViewModel OrderVm { get; set; }
        public IEnumerable<OrderListItemViewModel> OrderListItemVmList { get; set; }
        public PaginationViewModel PaginationVm { get; set; }
        public string StoreSearchQueryString { get; set; }
    }
}