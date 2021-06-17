using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PizzaWebsite.Models.ManageOrders
{
    public class OrderListItemViewModel
    {
        public CustomerOrderViewModel CustomerOrderVm { get; set; }
        public IEnumerable<SelectListItem> OrderStatusListItems { get; set; }
        public int SelectedOrderStatus { get; set; }
    }
}