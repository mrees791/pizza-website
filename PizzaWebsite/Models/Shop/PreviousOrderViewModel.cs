using PizzaWebsite.Models.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.Shop
{
    public class PreviousOrderViewModel
    {
        public int Id { get; set; }
        public DateTime DateOfOrder { get; set; }
        public decimal OrderTotal { get; set; }

        public CartViewModel CartViewModel { get; set; }
    }
}