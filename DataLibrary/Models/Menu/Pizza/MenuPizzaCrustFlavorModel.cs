using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Menu.Pizza
{
    public class MenuPizzaCrustFlavorModel : MenuPizzaItemModel
    {
        public MenuPizzaCrustFlavorModel(int id, bool availableForPurchase, string name, bool hasMenuIcon, string menuIconFile,
            bool hasPizzaBuilderImage, string pizzaBuilderImageFile, string description) :
            base(id, availableForPurchase, name, hasMenuIcon, menuIconFile, hasPizzaBuilderImage, pizzaBuilderImageFile, description)
        {
        }
    }
}
