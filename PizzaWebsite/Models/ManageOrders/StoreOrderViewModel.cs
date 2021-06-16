using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace PizzaWebsite.Models.ManageOrders
{
    public class StoreOrderViewModel : CustomerOrderViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}