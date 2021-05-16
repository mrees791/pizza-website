using DataLibrary.Models.Joins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PizzaWebsite.Models
{
    public class CostSummary
    {
        public decimal Subtotal { get; private set; }
        public decimal Tax { get; private set; }
        public decimal Total { get; private set; }

        public CostSummary(IEnumerable<CartItemJoin> cartItemJoinList)
        {
            UpdateTotals(cartItemJoinList);
        }

        private void UpdateTotals(IEnumerable<CartItemJoin> cartItemJoinList)
        {
            Subtotal = 0.0m;
            Tax = 0.0m;
            Total = 0.0m;

            foreach (CartItemJoin cartItemJoin in cartItemJoinList)
            {
                Subtotal += cartItemJoin.CartItem.Price;
            }

            Tax = Subtotal * 0.05m;
            Total = Subtotal + Tax;
        }
    }
}