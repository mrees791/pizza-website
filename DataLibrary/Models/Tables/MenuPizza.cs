using Dapper;
using DataLibrary.Models.Joins;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Tables
{
    [Table("MenuPizza")]
    public class MenuPizza : Record
    {
        [Key]
        public int Id { get; set; }
        public int SortOrder { get; set; }
        public string CategoryName { get; set; }
        public bool AvailableForPurchase { get; set; }
        public string PizzaName { get; set; }
        public string Description { get; set; }
        public int MenuPizzaSauceId { get; set; }
        public string SauceAmount { get; set; }
        public int MenuPizzaCheeseId { get; set; }
        public string CheeseAmount { get; set; }
        public int MenuPizzaCrustFlavorId { get; set; }
        public List<MenuPizzaTopping> Toppings { get; set; }

        public MenuPizza()
        {
            Toppings = new List<MenuPizzaTopping>();
        }

        public override dynamic GetId()
        {
            return Id;
        }

        internal override async Task MapEntityAsync(PizzaDatabase pizzaDb, IDbTransaction transaction = null)
        {
            Toppings.AddRange(await pizzaDb.GetListAsync<MenuPizzaTopping>(new { MenuPizzaId = Id }));
        }

        internal override async Task<dynamic> InsertAsync(PizzaDatabase pizzaDb, IDbTransaction transaction = null)
        {
            int? id = await pizzaDb.Connection.InsertAsync(this, transaction);
            Id = id.Value;

            // Insert toppings
            foreach (MenuPizzaTopping topping in Toppings)
            {
                topping.MenuPizzaId = Id;
                await topping.InsertAsync(pizzaDb, transaction);
            }

            return Id;
        }

        internal override async Task<int> UpdateAsync(PizzaDatabase pizzaDb, IDbTransaction transaction = null)
        {
            // Delete previous toppings
            await pizzaDb.Connection.DeleteListAsync<MenuPizzaTopping>(new { MenuPizzaId = Id }, transaction);

            // Insert new toppings
            foreach (MenuPizzaTopping topping in Toppings)
            {
                await pizzaDb.Connection.InsertAsync(topping, transaction);
            }

            // Update pizza record
            return await pizzaDb.Connection.UpdateAsync(this, transaction);
        }

        internal override bool InsertRequiresTransaction()
        {
            return true;
        }

        internal override bool UpdateRequiresTransaction()
        {
            return true;
        }

        public async Task<CartItemJoin> CreateCartRecordsAsync(PizzaDatabase pizzaDb, int cartId, int quantity, string size, int menuCrustId)
        {
            CartPizza cartPizza = new CartPizza()
            {
                CheeseAmount = CheeseAmount,
                MenuPizzaCheeseId = MenuPizzaCheeseId,
                MenuPizzaCrustFlavorId = MenuPizzaCrustFlavorId,
                MenuPizzaCrustId = menuCrustId,
                MenuPizzaSauceId = MenuPizzaSauceId,
                SauceAmount = SauceAmount,
                Size = size
            };

            foreach (MenuPizzaTopping menuTopping in Toppings)
            {
                cartPizza.Toppings.Add(menuTopping.CreateCartTopping());
            }

            CartItem cartItem = new CartItem()
            {
                CartId = cartId,
                Quantity = quantity,
                ProductCategory = ProductCategory.Pizza.ToString(),
                PricePerItem = await cartPizza.CalculatePriceAsync(pizzaDb)
            };

            CartItemJoin cartItemJoin = new CartItemJoin()
            {
                CartItem = cartItem,
                CartItemType = cartPizza
            };

            return cartItemJoin;
        }
    }
}