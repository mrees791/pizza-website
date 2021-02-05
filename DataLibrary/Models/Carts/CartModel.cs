﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Carts
{
    public class CartModel
    {
        public int Id { get; set; }
        public List<CartItemModel> CartItems { get; set; }

        public CartModel()
        {
            CartItems = new List<CartItemModel>();
        }
    }
}
