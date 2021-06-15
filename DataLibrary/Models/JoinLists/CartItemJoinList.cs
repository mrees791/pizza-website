﻿using System.Collections.Generic;
using System.Threading.Tasks;
using DataLibrary.Models.JoinLists.BaseClasses;
using DataLibrary.Models.JoinLists.CartItemCategories;
using DataLibrary.Models.Tables;

namespace DataLibrary.Models.JoinLists
{
    /// <summary>
    ///     Loads a join on the CartItem table and all other cart item category tables (CartPizza, etc)
    /// </summary>
    public class CartItemJoinList
    {
        public CartItemJoinList()
        {
            Items = new List<CartItemJoin>();
        }

        public List<CartItemJoin> Items { get; protected set; }

        public async Task LoadListByCartIdAsync(int cartId, PizzaDatabase pizzaDb)
        {
            // One join is needed for each cart item category table.
            await LoadCartItemTypeListByCartIdAsync<CartItemOnCartPizzaJoinList, CartPizza>(cartId, pizzaDb);
            Items.Sort();
        }

        private async Task LoadCartItemTypeListByCartIdAsync<TJoinList, TCategoryTable>(int cartId,
            PizzaDatabase pizzaDb)
            where TJoinList : CartItemJoinListBase<TCategoryTable>, new()
            where TCategoryTable : CartItemCategory
        {
            TJoinList joinList = new TJoinList();
            await joinList.LoadListByCartIdAsync(cartId, pizzaDb);
            Items.AddRange(joinList.Items);
        }
    }
}