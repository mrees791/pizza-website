using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.ManageOrders
{
    public class ManageOrdersIndexViewModel
    {
        public StoreViewModel StoreVm { get; set; }
        public IEnumerable<StoreViewModel> StoreVmList { get; set; }
        public PaginationViewModel PaginationVm { get; set; }
    }
}