using System.Collections.Generic;
using System.Linq;

namespace PizzaWebsite.Models.Shop
{
    public class PreviousOrderListViewModel
    {
        public CustomerOrderViewModel PreviousOrderViewModel { get; set; }
        public IEnumerable<CustomerOrderViewModel> PreviousOrderVmList { get; set; }
        public PaginationViewModel PaginationVm { get; set; }

        public bool HasPreviousOrders()
        {
            return PreviousOrderVmList.Any();
        }
    }
}