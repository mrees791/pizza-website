using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    /// <summary>
    /// Serves as a generic pizza topping model for either menu pizza toppings or cart pizza toppings.
    /// </summary>
    public class PizzaTopping
    {
        public int ToppingTypeId { get; set; }
        public string ToppingAmount { get; set; }
        public string ToppingHalf { get; set; }
    }
}
