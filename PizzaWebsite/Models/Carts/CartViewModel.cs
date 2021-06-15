using System.Collections.Generic;
using System.Linq;

namespace PizzaWebsite.Models.Carts
{
    public class CartViewModel
    {
        public int CartId { get; set; }
        public List<CartItemViewModel> CartItemVmList { get; set; }
        public CostSummaryViewModel CostSummaryVm { get; set; }

        public bool IsEmpty()
        {
            return !CartItemVmList.Any();
        }
    }
}