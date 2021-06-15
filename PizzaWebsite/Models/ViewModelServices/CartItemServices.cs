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
        public async Task<CartItemViewModel> CreateViewModelAsync(CartItemJoin cartItemJoin, PizzaDatabase pizzaDb, IEnumerable<int> quantityList)
        {
            CartItemViewModel model = CreateDefaultCartItemViewModel(cartItemJoin, quantityList);
            switch (cartItemJoin.CartItem.ProductCategory)
            {
                case "Pizza":
                    await InitializeViewModelAsync((CartPizza)cartItemJoin.CartItemType, model, pizzaDb);
                    break;
            }
            return model;
        }

        private CartItemViewModel CreateDefaultCartItemViewModel(CartItemJoin cartItemJoin, IEnumerable<int> quantityList)
        {
            return new CartItemViewModel()
            {
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

        private async Task InitializeViewModelAsync(CartPizza cartPizza, CartItemViewModel model, PizzaDatabase pizzaDb)
        {
            MenuPizzaCheese cheese = await pizzaDb.GetAsync<MenuPizzaCheese>(cartPizza.MenuPizzaCheeseId);
            MenuPizzaSauce sauce = await pizzaDb.GetAsync<MenuPizzaSauce>(cartPizza.MenuPizzaSauceId);
            MenuPizzaCrust crust = await pizzaDb.GetAsync<MenuPizzaCrust>(cartPizza.MenuPizzaCrustId);
            MenuPizzaCrustFlavor crustFlavor = await pizzaDb.GetAsync<MenuPizzaCrustFlavor>(cartPizza.MenuPizzaCrustFlavorId);
            string name = $"{cartPizza.Size} Pizza";
            string descriptionHtml = string.Empty;
            descriptionHtml = string.Empty;
            descriptionHtml += $"Size: {cartPizza.Size}<br />";
            descriptionHtml += $"Cheese: {cheese.Name}<br />";
            descriptionHtml += $"Sauce: {sauce.Name}<br />";
            descriptionHtml += $"Crust: {crust.Name}<br />";
            descriptionHtml += $"Crust Flavor: {crustFlavor.Name}<br /><br />";
            if (cartPizza.ToppingList.Any())
            {
                descriptionHtml += $"Toppings<br />";
                foreach (CartPizzaTopping topping in cartPizza.ToppingList)
                {
                    MenuPizzaToppingType toppingType = await pizzaDb.GetAsync<MenuPizzaToppingType>(topping.MenuPizzaToppingTypeId);
                    descriptionHtml += $"{toppingType.Name}: {topping.ToppingAmount}, {topping.ToppingHalf}<br />";
                }
                descriptionHtml += "<br />";
            }
            InitializeViewModel(model, name, descriptionHtml, "ModifyCartPizza", "PizzaMenu");
        }

        private void InitializeViewModel(CartItemViewModel model, string name, string descriptionHtml, string modifyActionName, string modifyControllerName)
        {
            model.Name = name;
            model.DescriptionHtml = descriptionHtml;
            model.ControlsEnabled = true;
            model.ModifyActionName = modifyActionName;
            model.ModifyControllerName = modifyControllerName;
        }
    }
}