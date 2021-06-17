using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.ManageOrders
{
    public class StoreOrderListViewModel
    {
        public CustomerOrderViewModel StoreOrderVm { get; set; }
        public IEnumerable<CustomerOrderViewModel> StoreOrderVmList { get; set; }
        public PaginationViewModel PaginationVm { get; set; }
        public string StoreSearchQueryString { get; set; }
    }
}