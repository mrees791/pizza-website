﻿using DataLibrary.Models.Pizzas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Carts
{
    public class CartPizzaModel : CartItemModel
    {
        public PizzaModel Pizza { get; set; }
    }
}
