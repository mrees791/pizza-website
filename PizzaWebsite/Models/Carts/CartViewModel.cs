using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.Carts
{

    public class CartViewModel
    {
        public List<CartItemViewModel> CartItemList { get; set; }

        public CartViewModel()
        {
            CartItemList = new List<CartItemViewModel>();
        }

        public bool IsEmpty()
        {
            return !CartItemList.Any();
        }
    }
}