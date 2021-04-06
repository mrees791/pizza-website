using DataLibrary.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Joins
{
    // Used for when you need to join the CartItem table with the CartPizza table.
    public class JoinedCartPizza
    {
        public CartItem CartItem { get; set; }
        public CartPizza CartPizza { get; set; }

        /* todo: Remove
        // CartItem columns.
        public int CartId { get; set; }
        public decimal PricePerItem { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public string ProductCategory { get; set; }

        // CartPizza columns.
        public int CartItemId { get; set; }
        public string Size { get; set; }
        public int MenuPizzaCrustId { get; set; }
        public int MenuPizzaSauceId { get; set; }
        public string SauceAmount { get; set; }
        public int MenuPizzaCheeseId { get; set; }
        public string CheeseAmount { get; set; }
        public int MenuPizzaCrustFlavorId { get; set; }
        public List<CartPizzaTopping> Toppings { get; set; }*/
    }
}
