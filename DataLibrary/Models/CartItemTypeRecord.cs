using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    public abstract class CartItemTypeRecord : Record
    {
        public abstract void SetCartItemId(int cartItemId);
        public abstract Task<decimal> CalculatePriceAsync(PizzaDatabase pizzaDb);
    }
}