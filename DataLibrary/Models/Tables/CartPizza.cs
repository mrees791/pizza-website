using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Tables
{
    [Table("CartPizza")]
    public class CartPizza : CartItemTypeRecord
    {
        public string Size { get; set; }
        public int MenuPizzaCrustId { get; set; }
        public int MenuPizzaSauceId { get; set; }
        public string SauceAmount { get; set; }
        public int MenuPizzaCheeseId { get; set; }
        public string CheeseAmount { get; set; }
        public int MenuPizzaCrustFlavorId { get; set; }
        public List<CartPizzaTopping> Toppings { get; set; }

        public CartPizza()
        {
            Toppings = new List<CartPizzaTopping>();
        }

        public override dynamic GetId()
        {
            return CartItemId;
        }

        internal override bool InsertRequiresTransaction()
        {
            return true;
        }

        internal override bool UpdateRequiresTransaction()
        {
            return true;
        }

        internal override async Task MapEntityAsync(PizzaDatabase pizzaDb, IDbTransaction transaction = null)
        {
            Toppings.AddRange(await pizzaDb.GetListAsync<CartPizzaTopping>(new { CartItemId = CartItemId }));
        }

        internal override async Task<dynamic> InsertAsync(PizzaDatabase pizzaDb, IDbTransaction transaction = null)
        {
            await pizzaDb.Connection.QueryAsync(@"INSERT INTO
                                   CartPizza (CartItemId, Size, MenuPizzaCrustId, MenuPizzaSauceId, SauceAmount, MenuPizzaCheeseId, CheeseAmount, MenuPizzaCrustFlavorId)
                                   VALUES (@CartItemId, @Size, @MenuPizzaCrustId, @MenuPizzaSauceId, @SauceAmount, @MenuPizzaCheeseId, @CheeseAmount, @MenuPizzaCrustFlavorId)",
                                      this, transaction);

            foreach (CartPizzaTopping topping in Toppings)
            {
                topping.CartItemId = CartItemId;
                await topping.InsertAsync(pizzaDb, transaction);
            }

            return CartItemId;
        }

        internal override async Task<int> UpdateAsync(PizzaDatabase pizzaDb, IDbTransaction transaction = null)
        {
            // Delete previous toppings
            await pizzaDb.Connection.DeleteListAsync<CartPizzaTopping>(new { CartItemId = CartItemId }, transaction);

            // Insert new toppings
            foreach (CartPizzaTopping topping in Toppings)
            {
                topping.CartItemId = CartItemId;
                await topping.InsertAsync(pizzaDb, transaction);
            }

            // Update pizza record
            return await pizzaDb.Connection.UpdateAsync(this, transaction);
        }

        public override async Task<decimal> CalculatePriceAsync(PizzaDatabase pizzaDb)
        {
            decimal total = 0.0m;

            MenuPizzaCheese cheese = await pizzaDb.GetAsync<MenuPizzaCheese>(MenuPizzaCheeseId);
            MenuPizzaSauce sauce = await pizzaDb.GetAsync<MenuPizzaSauce>(MenuPizzaSauceId);
            MenuPizzaCrust crust = await pizzaDb.GetAsync<MenuPizzaCrust>(MenuPizzaCrustId);

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
                MenuPizzaToppingType toppingType = await pizzaDb.GetAsync<MenuPizzaToppingType>(topping.MenuPizzaToppingTypeId);

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

        public override void SetCartItemId(int cartItemId)
        {
            CartItemId = cartItemId;
        }
    }
}