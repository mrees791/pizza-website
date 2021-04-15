using Dapper;
using DataLibrary.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Tables
{
    public class CartPizza : IRecordCartItemType
    {
        [Key]
        public int CartItemId { get; set; }
        public string Size { get; set; }
        public int MenuPizzaCrustId { get; set; }
        public int MenuPizzaSauceId { get; set; }
        public string SauceAmount { get; set; }
        public int MenuPizzaCheeseId { get; set; }
        public string CheeseAmount { get; set; }
        public int MenuPizzaCrustFlavorId { get; set; }
        public List<CartPizzaTopping> Toppings { get; set; }
        public CartItem CartItem { get; set; }

        public CartPizza()
        {
            Toppings = new List<CartPizzaTopping>();
        }

        public void Insert(PizzaDatabase pizzaDb, IDbTransaction transaction = null)
        {
            CartItemId = CartItem.Id;

            pizzaDb.Connection.Query(@"INSERT INTO
                                   CartPizza (CartItemId, Size, MenuPizzaCrustId, MenuPizzaSauceId, SauceAmount, MenuPizzaCheeseId, CheeseAmount, MenuPizzaCrustFlavorId)
                                   VALUES (@CartItemId, @Size, @MenuPizzaCrustId, @MenuPizzaSauceId, @SauceAmount, @MenuPizzaCheeseId, @CheeseAmount, @MenuPizzaCrustFlavorId)",
                                      this, transaction);

            foreach (CartPizzaTopping topping in Toppings)
            {
                topping.CartItemId = CartItemId;
                topping.Insert(pizzaDb, transaction);
            }
        }

        public dynamic GetId()
        {
            return CartItemId;
        }

        public void MapEntity(PizzaDatabase pizzaDb)
        {
            Toppings = pizzaDb.GetList<CartPizzaTopping>(new { CartItemId = CartItemId }, "Id");
        }

        public int Update(PizzaDatabase pizzaDb, IDbTransaction transaction)
        {
            // Delete previous toppings
            pizzaDb.Connection.DeleteList<CartPizzaTopping>(new { CartItemId = CartItemId }, transaction);

            // Insert new toppings
            foreach (CartPizzaTopping topping in Toppings)
            {
                pizzaDb.Connection.Insert(topping, transaction);
            }

            // Update pizza record
            int rowsAffected = pizzaDb.Connection.Update(this, transaction);

            return rowsAffected;
        }

        public bool InsertRequiresTransaction()
        {
            return true;
        }

        public bool UpdateRequiresTransaction()
        {
            return true;
        }

        public decimal CalculatePrice(PizzaDatabase pizzaDb)
        {
            decimal total = 0.0m;

            MenuPizzaCheese cheese = pizzaDb.Get<MenuPizzaCheese>(MenuPizzaCheeseId);
            MenuPizzaSauce sauce = pizzaDb.Get<MenuPizzaSauce>(MenuPizzaSauceId);
            MenuPizzaCrust crust = pizzaDb.Get<MenuPizzaCrust>(MenuPizzaCrustId);

            switch (CheeseAmount)
            {
                case "Light":
                    total += cheese.PriceLight;
                    break;
                case "Regular":
                    total += cheese.PriceRegular;
                    break;
                case "Extra":
                    total += cheese.PriceExtra;
                    break;
            }

            switch (SauceAmount)
            {
                case "Light":
                    total += sauce.PriceLight;
                    break;
                case "Regular":
                    total += sauce.PriceRegular;
                    break;
                case "Extra":
                    total += sauce.PriceExtra;
                    break;
            }

            switch (Size)
            {
                case "Small":
                    total += crust.PriceSmall;
                    break;
                case "Medium":
                    total += crust.PriceMedium;
                    break;
                case "Large":
                    total += crust.PriceLarge;
                    break;
            }

            foreach (CartPizzaTopping topping in Toppings)
            {
                MenuPizzaToppingType toppingType = pizzaDb.Get<MenuPizzaToppingType>(topping.MenuPizzaToppingTypeId);

                decimal toppingAmount = 0.0m;

                switch (topping.ToppingAmount)
                {
                    case "Light":
                        toppingAmount = toppingType.PriceLight;
                        break;
                    case "Regular":
                        toppingAmount = toppingType.PriceRegular;
                        break;
                    case "Extra":
                        toppingAmount = toppingType.PriceExtra;
                        break;
                }

                if (topping.ToppingHalf != "Whole")
                {
                    toppingAmount /= 2.0m;
                }

                total += toppingAmount;
            }

            return total;
        }

        public string GetDescriptionHtml(PizzaDatabase pizzaDb)
        {
            MenuPizzaCheese cheese = pizzaDb.Get<MenuPizzaCheese>(MenuPizzaCheeseId);
            MenuPizzaSauce sauce = pizzaDb.Get<MenuPizzaSauce>(MenuPizzaSauceId);
            MenuPizzaCrust crust = pizzaDb.Get<MenuPizzaCrust>(MenuPizzaCrustId);
            MenuPizzaCrustFlavor crustFlavor = pizzaDb.Get<MenuPizzaCrustFlavor>(MenuPizzaCrustFlavorId);

            string details = string.Empty;

            if (Toppings.Any())
            {
                details += $"Toppings<br />";

                foreach (CartPizzaTopping topping in Toppings)
                {
                    MenuPizzaToppingType toppingType = pizzaDb.Get<MenuPizzaToppingType>(topping.MenuPizzaToppingTypeId);
                    details += $"{toppingType.Name}: {topping.ToppingAmount}, {topping.ToppingHalf}<br />";
                }

                details += "<br />";
            }

            details += $"Size: {Size}<br />";
            details += $"Cheese: {cheese.Name}<br />";
            details += $"Sauce: {sauce.Name}<br />";
            details += $"Crust: {crust.Name}<br />";
            details += $"Crust Flavor: {crustFlavor.Name}<br />";

            return details;
        }

        public string GetName(PizzaDatabase pizzaDb)
        {
            return $"{Size} Pizza";
        }

        public int CompareTo(IRecordCartItemType other)
        {
            return CartItemId.CompareTo(other.GetId());
        }
    }
}