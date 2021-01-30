using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Menu
{
    public class MenuPastaModel : MenuItemWithIconModel
    {
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string ItemDetails { get; set; }

        public MenuPastaModel(int id, bool availableForPurchase, string name, bool hasMenuIcon, string menuIconFile, decimal price, string description, string itemDetails) :
            base(id, availableForPurchase, name, hasMenuIcon, menuIconFile)
        {
            Price = price;
            Description = description;
            ItemDetails = itemDetails;
        }
    }
}
