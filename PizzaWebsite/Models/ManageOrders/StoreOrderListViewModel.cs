using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.ManageOrders
{
    public class StoreOrderListViewModel
    {
        public CustomerOrderViewModel CustomerOrderVm { get; set; }
        public IEnumerable<CustomerOrderViewModel> CustomerOrderVmList { get; set; }
        public PaginationViewModel PaginationVm { get; set; }
    }
}