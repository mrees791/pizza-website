using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Carts
{
    public class CartModel
    {
        public int Id { get; set; }
        public List<CartDessertModel> CartDesserts { get; set; }
        public List<CartDipModel> CartDips { get; set; }
        public List<CartDrinkModel> CartDrinks { get; set; }
        public List<CartPastaModel> CartPastas { get; set; }
        public List<CartPizzaModel> CartPizzas { get; set; }
        public List<CartSauceModel> CartSauces { get; set; }
        public List<CartSideModel> CartSides { get; set; }
        public List<CartWingsModel> CartWings { get; set; }
    }
}
