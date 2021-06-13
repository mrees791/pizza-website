using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.Shop
{
    public class PreviousOrderListViewModel
    {
        public PreviousOrderViewModel PreviousOrderViewModel { get; set; }
        public List<PreviousOrderViewModel> PreviousOrderVmList { get; set; }
        public PaginationViewModel PaginationVm { get; set; }

        public bool HasPreviousOrders()
        {
            return PreviousOrderVmList.Any();
        }
    }
}