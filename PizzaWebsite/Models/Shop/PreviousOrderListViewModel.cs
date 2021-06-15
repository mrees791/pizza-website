using System.Collections.Generic;
using System.Linq;

namespace PizzaWebsite.Models.Shop
{
    public class PreviousOrderListViewModel
    {
        public PreviousOrderViewModel PreviousOrderViewModel { get; set; }
        public IEnumerable<PreviousOrderViewModel> PreviousOrderVmList { get; set; }
        public PaginationViewModel PaginationVm { get; set; }

        public bool HasPreviousOrders()
        {
            return PreviousOrderVmList.Any();
        }
    }
}