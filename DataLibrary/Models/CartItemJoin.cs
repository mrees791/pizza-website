using System;
using System.Threading.Tasks;
using DataLibrary.Models.Tables;

namespace DataLibrary.Models
{
    public class CartItemJoin : IComparable<CartItemJoin>
    {
        public CartItemJoin(CartItem cartItem, CartItemCategory cartItemType)
        {
            CartItem = cartItem;
            CartItemType = cartItemType;
        }

        public CartItem CartItem { get; set; }
        public CartItemCategory CartItemType { get; set; }

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