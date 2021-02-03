using DataLibrary.Models.Pizzas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Menu.Pizzas
{
    public class MenuPizzaModel
    {
        public int Id { get; set; }
        public PizzaModel Pizza { get; set; }
        public string CategoryName { get; set; }
        public bool AvailableForPurchase { get; set; }
        public string PizzaName { get; set; }
        public string Description { get; set; }
    }
}
