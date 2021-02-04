using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Menus.Pizzas
{
    public class MenuPizzaCategory
    {
        public const string Popular = "Popular Pizzas";
        public const string Meats = "Meats";
        public const string Chicken = "Chicken";
        public const string Veggie = "Veggie";

        public static string[] GetCategories()
        {
            return new string[]
            {
                Popular, Meats, Chicken, Veggie
            };
        }
    }
}
