using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Menu.Pizzas
{
    public class MenuPizzaSauceModel : MenuPizzaItemModel
    {
        public decimal PriceLight { get; set; }
        public decimal PriceRegular { get; set; }
        public decimal PriceExtra { get; set; }
    }
}
