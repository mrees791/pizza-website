using DataLibrary.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Utility
{
    // todo: Will move somewhere else.
    public static class CartItemUtility
    {
        public static ProductCategory FindProductCategory(string name)
        {
            switch (name)
            {
                case "Pizza":
                    return ProductCategory.Pizza;
            }
            throw new Exception($"Product category not found: {name}");
        }

        // CartPizza

        public static string CreateHtmlItemDetails(CartPizza cartPizza, PizzaDatabase pizzaDb)
        {
            MenuPizzaCheese cheese = pizzaDb.Get<MenuPizzaCheese>(cartPizza.MenuPizzaCheeseId);
            MenuPizzaSauce sauce = pizzaDb.Get<MenuPizzaSauce>(cartPizza.MenuPizzaSauceId);
            MenuPizzaCrust crust = pizzaDb.Get<MenuPizzaCrust>(cartPizza.MenuPizzaCrustId);
            MenuPizzaCrustFlavor crustFlavor = pizzaDb.Get<MenuPizzaCrustFlavor>(cartPizza.MenuPizzaCrustFlavorId);

            string details = string.Empty;
            details += $"Size: {cartPizza.Size}<br />";
            details += $"Cheese: {cheese.Name}<br />";
            details += $"Sauce: {sauce.Name}<br />";
            details += $"Crust: {crust.Name}<br />";
            details += $"Crust Flavor: {crustFlavor.Name}<br />";

            if (cartPizza.Toppings.Any())
            {
                details += $"<br />Toppings<br />";

                foreach (CartPizzaTopping topping in cartPizza.Toppings)
                {
                    MenuPizzaToppingType toppingType = pizzaDb.Get<MenuPizzaToppingType>(topping.MenuPizzaToppingTypeId);
                    details += $"{toppingType.Name}: {topping.ToppingAmount}, {topping.ToppingHalf}";
                }
            }

            return details;
        }

        public static string CreateItemName(CartPizza cartPizza)
        {
            string name = $"{cartPizza.Size} Pizza";

            return name;
        }
    }
}
