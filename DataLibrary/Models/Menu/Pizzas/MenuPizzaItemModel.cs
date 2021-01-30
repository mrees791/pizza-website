using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Menu.Pizzas
{
    public abstract class MenuPizzaItemModel : MenuItemWithIconModel
    {
        public bool HasPizzaBuilderImage { get; set; }
        public string PizzaBuilderImageFile { get; set; }
        public string Description { get; set; }
    }
}
