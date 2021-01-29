using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Menu.Pizza
{
    public abstract class MenuPizzaItemModel : MenuItemModel
    {
        public bool HasPizzaBuilderImage { get; set; }
        public string PizzaBuilderImageFile { get; set; }
        public string Description { get; set; }
    }
}
