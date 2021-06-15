using DataLibrary.Models;
using DataLibrary.Models.JoinLists;
using PizzaWebsite.Models.ViewModelServices;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

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