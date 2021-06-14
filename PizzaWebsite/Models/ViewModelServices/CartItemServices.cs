using DataLibrary.Models;
using DataLibrary.Models.Tables;
using PizzaWebsite.Models.Carts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PizzaWebsite.Models.ViewModelServices
{
    public class CartItemServices
    {
        public async Task<CartItemViewModel> CreateViewModelAsync(CartItemJoin cartItemJoin, PizzaDatabase pizzaDb, List<int> quantityList)
        {
            CartItemViewModel model = CreateDefaultCartItemViewModel(cartItemJoin, quantityList);
            switch (cartItemJoin.CartItem.ProductCategory)
            {
                case "Pizza":
                    await InitializeCartPizzaViewModelAsync(model, (CartPizza)cartItemJoin.CartItemType, pizzaDb);
                    break;
            }
            return model;
        }

        private CartItemViewModel CreateDefaultCartItemViewModel(CartItemJoin cartItemJoin, List<int> quantityList)
        {
            return new CartItemViewModel()
            {
                CartItemJoin = cartItemJoin,
                CartItemId = cartItemJoin.CartItem.Id,
                ProductCategory = cartItemJoin.CartItem.ProductCategory,
                Price = cartItemJoin.CartItem.Price.ToString("C", CultureInfo.CurrentCulture),
                Quantity = cartItemJoin.CartItem.Quantity,
                QuantityList = quantityList,
                CartItemQuantitySelectId = $"cartItemQuantitySelect-{cartItemJoin.CartItem.Id}",
                CartItemDeleteButtonId = $"cartItemDeleteButton-{cartItemJoin.CartItem.Id}",
                CartItemPriceCellId = $"cartItemPriceCell-{cartItemJoin.CartItem.Id}",
                CartItemRowId = $"cartItemRow-{cartItemJoin.CartItem.Id}",
                Name = $"{cartItemJoin.CartItem.ProductCategory}",
                DescriptionHtml = "Description not available."
            };
        }

        private async Task InitializeCartPizzaViewModelAsync(CartItemViewModel model, CartPizza cartPizza, PizzaDatabase pizzaDb)
        {
            MenuPizzaCheese cheese = await pizzaDb.GetAsync<MenuPizzaCheese>(cartPizza.MenuPizzaCheeseId);
            MenuPizzaSauce sauce = await pizzaDb.GetAsync<MenuPizzaSauce>(cartPizza.MenuPizzaSauceId);
            MenuPizzaCrust crust = await pizzaDb.GetAsync<MenuPizzaCrust>(cartPizza.MenuPizzaCrustId);
            MenuPizzaCrustFlavor crustFlavor = await pizzaDb.GetAsync<MenuPizzaCrustFlavor>(cartPizza.MenuPizzaCrustFlavorId);
            model.DescriptionHtml = string.Empty;
            model.Name = $"{cartPizza.Size} Pizza";
            model.DescriptionHtml += $"Size: {cartPizza.Size}<br />";
            model.DescriptionHtml += $"Cheese: {cheese.Name}<br />";
            model.DescriptionHtml += $"Sauce: {sauce.Name}<br />";
            model.DescriptionHtml += $"Crust: {crust.Name}<br />";
            model.DescriptionHtml += $"Crust Flavor: {crustFlavor.Name}<br /><br />";
            if (cartPizza.ToppingList.Any())
            {
                model.DescriptionHtml += $"Toppings<br />";
                foreach (CartPizzaTopping topping in cartPizza.ToppingList)
                {
                    MenuPizzaToppingType toppingType = await pizzaDb.GetAsync<MenuPizzaToppingType>(topping.MenuPizzaToppingTypeId);
                    model.DescriptionHtml += $"{toppingType.Name}: {topping.ToppingAmount}, {topping.ToppingHalf}<br />";
                }
                model.DescriptionHtml += "<br />";
            }
        }
    }
}