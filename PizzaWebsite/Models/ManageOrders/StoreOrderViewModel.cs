﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.ManageOrders
{
    public class StoreOrderViewModel
    {
        public string StoreSearchQueryString { get; set; }
        public CustomerOrderViewModel CustomerOrderVm { get; set; }
    }
}