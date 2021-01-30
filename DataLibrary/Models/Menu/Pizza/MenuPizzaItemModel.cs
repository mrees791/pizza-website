using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Menu.Pizza
{
    public abstract class MenuPizzaItemModel : MenuItemWithIconModel
    {
        public bool HasPizzaBuilderImage { get; set; }
        public string PizzaBuilderImageFile { get; set; }
        public string Description { get; set; }

        public MenuPizzaItemModel(int id, bool availableForPurchase, string name, bool hasMenuIcon, string menuIconFile,
            bool hasPizzaBuilderImage, string pizzaBuilderImageFile, string description) :
            base(id, availableForPurchase, name, hasMenuIcon, menuIconFile)
        {
            HasPizzaBuilderImage = hasPizzaBuilderImage;
            PizzaBuilderImageFile = pizzaBuilderImageFile;
            Description = description;
        }
    }
}
