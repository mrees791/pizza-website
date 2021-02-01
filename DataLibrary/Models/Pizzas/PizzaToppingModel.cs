using DataLibrary.Models.Menu.Pizzas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Pizzas
{
    public class PizzaToppingModel
    {
        public int Id { get; set; }
        public string ToppingHalf { get; set; }
        public MenuPizzaToppingModel MenuPizzaTopping { get; set; }
    }
}
