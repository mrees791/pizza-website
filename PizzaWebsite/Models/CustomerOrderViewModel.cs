using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PizzaWebsite.Models.Carts;

namespace PizzaWebsite.Models
{
    public class CustomerOrderViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Date")]
        public string DateOfOrder { get; set; }

        [Display(Name = "Type")]
        public string OrderType { get; set; }

        [Display(Name = "Total")]
        public string OrderTotal { get; set; }
        public CartViewModel CartVm { get; set; }
        public DeliveryInfoViewModel DeliveryInfoVm { get; set; }

        public bool IsDelivery()
        {
            return DeliveryInfoVm != null;
        }
    }
}