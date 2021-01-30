using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Menu.Pizzas
{
    public class MenuPizzaCrustModel : MenuPizzaItemModel
    {
        public decimal PriceSmall { get; set; }
        public decimal PriceMedium { get; set; }
        public decimal PriceLarge { get; set; }
    }
}
