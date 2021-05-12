using PizzaWebsite.Models.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PizzaWebsite.Models.Shop
{
    public class CheckoutViewModel
    {
        [HiddenInput]
        public int OrderConfirmationId { get; set; }
        public CartViewModel Cart { get; set; }

        public CheckoutViewModel()
        {
            Cart = new CartViewModel();
        }
    }
}