using DataLibrary.Models.Menu.Pizzas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Pizzas
{
    public class PizzaModel
    {
        public int Id { get; set; }
        public string Size { get; set; }
        public MenuPizzaCrustModel MenuPizzaCrust { get; set; }
        public MenuPizzaSauceModel MenuPizzaSauce { get; set; }
        public string SauceAmount { get; set; }
        public MenuPizzaCheeseModel MenuPizzaCheese { get; set; }
        public string CheeseAmount { get; set; }
        public MenuPizzaCrustFlavorModel MenuPizzaCrustFlavor { get; set; }
        public List<PizzaToppingModel> PizzaToppings { get; set; }

        public PizzaModel()
        {
            PizzaToppings = new List<PizzaToppingModel>();
        }
    }
}
