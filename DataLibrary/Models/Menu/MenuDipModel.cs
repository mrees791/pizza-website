using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Menu
{
    public class MenuDipModel : MenuItemWithIconModel
    {
        public decimal Price { get; set; }
        public string ItemDetails { get; set; }

        public MenuDipModel(int id, bool availableForPurchase, string name, bool hasMenuIcon, string menuIconFile, decimal price, string itemDetails) :
            base(id, availableForPurchase, name, hasMenuIcon, menuIconFile)
        {
            Price = price;
            ItemDetails = itemDetails;
        }
    }
}
