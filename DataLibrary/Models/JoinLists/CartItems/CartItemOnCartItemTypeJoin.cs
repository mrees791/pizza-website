using DataLibrary.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.JoinLists.CartItems
{
    /// <summary>
    /// Loads a join on the CartItem table and all other cart item type tables (CartPizza, etc)
    /// </summary>
    public class CartItemOnCartItemTypeJoin
    {
        public List<CartItemJoin> Items { get; protected set; }

        public CartItemOnCartItemTypeJoin()
        {
            Items = new List<CartItemJoin>();
        }

        public async Task LoadListByCartIdAsync(int cartId, PizzaDatabase pizzaDb)
        {
            // One join is needed for each cart item category table.
            await LoadCartItemTypeListByCartIdAsync<CartItemOnCartPizzaJoin, CartPizza>(cartId, pizzaDb);
            Items.Sort();
        }

        private async Task LoadCartItemTypeListByCartIdAsync<TJoinList, TCartItemType>(int cartId, PizzaDatabase pizzaDb)
            where TJoinList : CartItemJoinListBase<TCartItemType>, new()
            where TCartItemType : CartItemType
        {
            TJoinList joinList = new TJoinList();
            await joinList.LoadListByCartIdAsync(cartId, pizzaDb);
            Items.AddRange(joinList.Items);
        }
    }
}
