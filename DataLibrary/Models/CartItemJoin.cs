using DataLibrary.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    public class CartItemJoin : IComparable<CartItemJoin>
    {
        public CartItem CartItem { get; set; }
        public CartItemType CartItemType { get; set; }

        public CartItemJoin(CartItem cartItem, CartItemType cartItemType)
        {
            CartItem = cartItem;
            CartItemType = cartItemType;
        }

        public int CompareTo(CartItemJoin other)
        {
            return CartItem.Id.CompareTo(other.CartItem.Id);
        }

        public async Task MapAsync(PizzaDatabase pizzaDb)
        {
            await CartItem.MapEntityAsync(pizzaDb);
            await CartItemType.MapEntityAsync(pizzaDb);
        }
    }
}
