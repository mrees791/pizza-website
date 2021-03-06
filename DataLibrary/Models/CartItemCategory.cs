﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    public abstract class CartItemCategory : Record
    {
        [Key]
        public int CartItemId { get; set; }

        public abstract void SetCartItemId(int cartItemId);
        public abstract Task<decimal> CalculateItemPriceAsync(PizzaDatabase pizzaDb);
    }
}