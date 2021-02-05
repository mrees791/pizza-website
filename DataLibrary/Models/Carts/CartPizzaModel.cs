using DataLibrary.Models.Pizzas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Carts
{
    public class CartPizzaModel : CartItemModel
    {
        public int CartPizzaId { get; set; }
        public PizzaModel Pizza { get; set; }

        public override object Clone()
        {
            PizzaModel pizzaClone = (PizzaModel)Pizza.Clone();

            CartPizzaModel clone = new CartPizzaModel()
            {
                Pizza = pizzaClone,
                PricePerItem = PricePerItem,
                Quantity = Quantity,
                CartId = CartId
            };

            return clone;
        }
    }
}
