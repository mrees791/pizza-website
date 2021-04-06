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
    public class CartPizza : ITableBase
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

        public CartPizza()
        {
            Toppings = new List<CartPizzaTopping>();
        }

        public void AddInsertItems(List<IInsertable> itemsList)
        {
            itemsList.Add(this);
            foreach (var topping in Toppings)
            {
                topping.AddInsertItems(itemsList);
            }
        }

        public void Insert(IDbConnection connection, IDbTransaction transaction = null)
        {
            connection.Query(@"INSERT INTO
                                   CartPizza (CartItemId, Size, MenuPizzaCrustId, MenuPizzaSauceId, SauceAmount, MenuPizzaCheeseId, CheeseAmount, MenuPizzaCrustFlavorId)
                                   VALUES (@CartItemId, @Size, @MenuPizzaCrustId, @MenuPizzaSauceId, @SauceAmount, @MenuPizzaCheeseId, @CheeseAmount, @MenuPizzaCrustFlavorId)",
                                      this, transaction);

            foreach (CartPizzaTopping topping in Toppings)
            {
                topping.CartItemId = CartItemId;
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

        public int Update(PizzaDatabase pizzaDb)
        {
            using (var transaction = pizzaDb.Connection.BeginTransaction())
            {
                // Delete previous toppings
                pizzaDb.Connection.DeleteList<CartPizzaTopping>(new { CartItemId = CartItemId }, transaction);

                // Insert new toppings
                foreach (CartPizzaTopping topping in Toppings)
                {
                    pizzaDb.Connection.Insert(topping, transaction);
                }

                // Update pizza record
                int rowsAffected = pizzaDb.Connection.Update(this);

                transaction.Commit();

                return rowsAffected;
            }
        }
    }
}