using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Menu.Pizza
{
    public class MenuPizzaCrustModel : MenuPizzaItemModel
    {
        public decimal PriceSmall { get; set; }
        public decimal PriceMedium { get; set; }
        public decimal PriceLarge { get; set; }

        public MenuPizzaCrustModel(int id, bool availableForPurchase, string name, bool hasMenuIcon, string menuIconFile,
            bool hasPizzaBuilderImage, string pizzaBuilderImageFile, string description, decimal priceSmall, decimal priceMedium, decimal priceLarge) :
            base(id, availableForPurchase, name, hasMenuIcon, menuIconFile, hasPizzaBuilderImage, pizzaBuilderImageFile, description)
        {
            PriceSmall = priceSmall;
            PriceMedium = priceMedium;
            PriceLarge = priceLarge;
        }
    }
}
