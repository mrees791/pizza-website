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
    [Table("MenuPizza")]
    public class MenuPizza : ITable
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

        public void AddInsertItems(List<IInsertable> itemsList)
        {
            itemsList.Add(this);
            foreach (MenuPizzaTopping topping in Toppings)
            {
                topping.AddInsertItems(itemsList);
            }
        }

        public void Insert(IDbConnection connection, IDbTransaction transaction = null)
        {
            Id = connection.Insert(this, transaction).Value;

            foreach (MenuPizzaTopping topping in Toppings)
            {
                topping.MenuPizzaId = Id;
            }
        }

        public dynamic GetId()
        {
            return Id;
        }

        public void MapEntity(PizzaDatabase pizzaDb)
        {
            Toppings = pizzaDb.GetList<MenuPizzaTopping>(new { MenuPizzaId = Id }, "Id");
        }

        public int Update(PizzaDatabase pizzaDb, IDbTransaction transaction)
        {
            // Delete previous toppings
            pizzaDb.Connection.DeleteList<MenuPizzaTopping>(new { MenuPizzaId = Id }, transaction);

            // Insert new toppings
            foreach (MenuPizzaTopping topping in Toppings)
            {
                pizzaDb.Connection.Insert(topping, transaction);
            }

            // Update pizza record
            int rowsAffected = pizzaDb.Connection.Update(this, transaction);

            return rowsAffected;
        }

        public CartPizza CreateCartPizza(PizzaDatabase pizzaDb, int cartId, int quantity, string size, int menuCrustId)
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

            decimal pricePerItem = cartPizza.CalculatePrice(pizzaDb);

            CartItem cartItem = new CartItem()
            {
                CartId = cartId,
                PricePerItem = pricePerItem,
                ProductCategory = ProductCategory.Pizza,
                Quantity = quantity
            };

            cartPizza.CartItem = cartItem;

            return cartPizza;
        }

        public bool InsertRequiresTransaction()
        {
            return false;
        }

        public bool UpdateRequiresTransaction()
        {
            return true;
        }
    }
}