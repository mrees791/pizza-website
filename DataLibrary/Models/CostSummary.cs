using System.Collections.Generic;

namespace DataLibrary.Models
{
    public class CostSummary
    {
        public CostSummary(IEnumerable<CartItemJoin> cartItemJoinList)
        {
            UpdateTotals(cartItemJoinList);
        }

        public decimal Subtotal { get; private set; }
        public decimal Tax { get; private set; }
        public decimal Total { get; private set; }

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