using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace DataLibrary.Models.Tables
{
    [Table("MenuPizza")]
    public class MenuPizza : MenuCategoryRecord
    {
        public MenuPizza()
        {
            ToppingList = new List<MenuPizzaTopping>();
        }

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
        public List<MenuPizzaTopping> ToppingList { get; set; }

        public override dynamic GetId()
        {
            return Id;
        }

        public override MenuCategory GetMenuCategoryType()
        {
            return MenuCategory.Pizza;
        }

        internal override async Task MapEntityAsync(PizzaDatabase pizzaDb, IDbTransaction transaction = null)
        {
            ToppingList.AddRange(await pizzaDb.GetListAsync<MenuPizzaTopping>(new {MenuPizzaId = Id}));
        }

        internal override async Task<dynamic> InsertAsync(PizzaDatabase pizzaDb, IDbTransaction transaction = null)
        {
            int? id = await pizzaDb.Connection.InsertAsync(this, transaction);
            Id = id.Value;

            // Insert toppings
            foreach (MenuPizzaTopping topping in ToppingList)
            {
                topping.MenuPizzaId = Id;
                await topping.InsertAsync(pizzaDb, transaction);
            }

            return Id;
        }

        internal override async Task<int> UpdateAsync(PizzaDatabase pizzaDb, IDbTransaction transaction = null)
        {
            // Delete previous toppings
            await pizzaDb.Connection.DeleteListAsync<MenuPizzaTopping>(new {MenuPizzaId = Id}, transaction);

            // Insert new toppings
            foreach (MenuPizzaTopping topping in ToppingList)
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

        public async Task<Tuple<CartItem, CartPizza>> CreateCartRecordsAsync(int quantity, string size, int menuCrustId,
            SiteUser siteUser, PizzaDatabase pizzaDb)
        {
            CartPizza cartPizza = new CartPizza
            {
                CheeseAmount = CheeseAmount,
                MenuPizzaCheeseId = MenuPizzaCheeseId,
                MenuPizzaCrustFlavorId = MenuPizzaCrustFlavorId,
                MenuPizzaCrustId = menuCrustId,
                MenuPizzaSauceId = MenuPizzaSauceId,
                SauceAmount = SauceAmount,
                Size = size
            };

            foreach (MenuPizzaTopping menuTopping in ToppingList)
            {
                cartPizza.ToppingList.Add(menuTopping.CreateCartTopping());
            }

            decimal pricePerItem = await cartPizza.CalculateItemPriceAsync(pizzaDb);

            CartItem cartItem = new CartItem
            {
                CartId = siteUser.CurrentCartId,
                UserId = siteUser.Id,
                Quantity = quantity,
                ProductCategory = ProductCategory.Pizza.ToString(),
                PricePerItem = pricePerItem,
                Price = pricePerItem * quantity
            };

            return new Tuple<CartItem, CartPizza>(cartItem, cartPizza);
        }
    }
}