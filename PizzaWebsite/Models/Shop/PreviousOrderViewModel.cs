﻿using DataLibrary.Models;
using DataLibrary.Models.Tables;
using PizzaWebsite.Models.Carts;
using PizzaWebsite.Models.ViewModelServices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PizzaWebsite.Models.Shop
{
    public class PreviousOrderViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Date")]
        public string DateOfOrder { get; set; }
        [Display(Name = "Type")]
        public string OrderType { get; set; }
        [Display(Name = "Total")]
        public string OrderTotal { get; set; }
        public CartViewModel CartVm { get; set; }

        public async Task InitializeAsync(bool loadCartItems, CustomerOrder customerOrder, DeliveryInfo deliveryInfo, PizzaDatabase pizzaDb, List<int> quantityList)
        {
            Id = customerOrder.Id;
            DateOfOrder = $"{customerOrder.DateOfOrder.ToShortDateString()} {customerOrder.DateOfOrder.ToShortTimeString()}";
            OrderTotal = customerOrder.OrderTotal.ToString("C", CultureInfo.CurrentCulture);
            OrderType = customerOrder.GetOrderType();
            if (loadCartItems)
            {
                CartServices cartServices = new CartServices();
                CartVm = await cartServices.CreateViewModelAsync(customerOrder.CartId, pizzaDb, quantityList);
            }
        }
    }
}