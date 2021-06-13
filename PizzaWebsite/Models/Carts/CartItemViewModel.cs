using DataLibrary.Models;
using DataLibrary.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PizzaWebsite.Models.Carts
{

    public class CartItemViewModel
    {
        public int CartItemId { get; set; }
        public string Price { get; set; }
        public int Quantity { get; set; }
        public string ProductCategory { get; set; }
        public string CartItemQuantitySelectId { get; set; }
        public string CartItemDeleteButtonId { get; set; }
        public string CartItemRowId { get; set; }
        public string CartItemPriceCellId { get; set; }
        public List<int> QuantityList { get; set; }
        public string Name { get; set; }
        public string DescriptionHtml { get; set; }
        public CartItemJoin CartItemJoin { get; set; }

        public async Task UpdateAsync(PizzaDatabase pizzaDb)
        {
            switch (CartItemJoin.CartItem.ProductCategory)
            {
                case "Pizza":
                    await UpdateCartPizza(pizzaDb, ((CartPizza)CartItemJoin.CartItemType));
                    break;
                default:
                    Name = $"{CartItemJoin.CartItem.ProductCategory}";
                    DescriptionHtml = "Description not available.";
                    break;
            }
        }

        private async Task UpdateCartPizza(PizzaDatabase pizzaDb, CartPizza cartPizza)
        {
            MenuPizzaCheese cheese = await pizzaDb.GetAsync<MenuPizzaCheese>(cartPizza.MenuPizzaCheeseId);
            MenuPizzaSauce sauce = await pizzaDb.GetAsync<MenuPizzaSauce>(cartPizza.MenuPizzaSauceId);
            MenuPizzaCrust crust = await pizzaDb.GetAsync<MenuPizzaCrust>(cartPizza.MenuPizzaCrustId);
            MenuPizzaCrustFlavor crustFlavor = await pizzaDb.GetAsync<MenuPizzaCrustFlavor>(cartPizza.MenuPizzaCrustFlavorId);

            Name = $"{cartPizza.Size} Pizza";

            DescriptionHtml += $"Size: {cartPizza.Size}<br />";
            DescriptionHtml += $"Cheese: {cheese.Name}<br />";
            DescriptionHtml += $"Sauce: {sauce.Name}<br />";
            DescriptionHtml += $"Crust: {crust.Name}<br />";
            DescriptionHtml += $"Crust Flavor: {crustFlavor.Name}<br /><br />";

            if (cartPizza.ToppingList.Any())
            {
                DescriptionHtml += $"Toppings<br />";

                foreach (CartPizzaTopping topping in cartPizza.ToppingList)
                {
                    MenuPizzaToppingType toppingType = await pizzaDb.GetAsync<MenuPizzaToppingType>(topping.MenuPizzaToppingTypeId);
                    DescriptionHtml += $"{toppingType.Name}: {topping.ToppingAmount}, {topping.ToppingHalf}<br />";
                }

                DescriptionHtml += "<br />";
            }
        }
    }
}