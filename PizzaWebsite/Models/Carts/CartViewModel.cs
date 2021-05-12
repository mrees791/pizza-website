﻿using DataLibrary.Models;
using DataLibrary.Models.Joins;
using DataLibrary.Models.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PizzaWebsite.Models.Carts
{

    public class CartViewModel
    {
        public List<CartItemViewModel> CartItemList { get; set; }

        public async Task LoadCartItems(int cartId, PizzaDatabase pizzaDb)
        {
            List<int> quantityList = ListUtility.CreateQuantityList();
            IEnumerable<CartItemJoin> cartItemList = await pizzaDb.GetJoinedCartItemListAsync(cartId);

            foreach (CartItemJoin cartItemJoin in cartItemList)
            {
                CartItemViewModel cartItemVm = new CartItemViewModel()
                {
                    CartItemJoin = cartItemJoin,
                    CartItemId = cartItemJoin.CartItem.Id,
                    ProductCategory = cartItemJoin.CartItem.ProductCategory,
                    Price = cartItemJoin.CartItem.PricePerItem.ToString("C", CultureInfo.CurrentCulture),
                    Quantity = cartItemJoin.CartItem.Quantity,
                    QuantityList = quantityList,
                    CartItemQuantitySelectId = $"cartItemQuantitySelect-{cartItemJoin.CartItem.Id}",
                    CartItemDeleteButtonId = $"cartItemDeleteButton-{cartItemJoin.CartItem.Id}",
                    CartItemRowId = $"cartItemRow-{cartItemJoin.CartItem.Id}"
                };

                await cartItemVm.UpdateAsync(pizzaDb);

                CartItemList.Add(cartItemVm);
            }
        }

        public bool IsEmpty()
        {
            return !CartItemList.Any();
        }
    }
}