using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Menu
{
    public class MenuWingsSauceModel : MenuItemModel
    {
        public string Description { get; set; }

        public MenuWingsSauceModel(int id, bool availableForPurchase, string name, string description) :
            base(id, availableForPurchase, name)
        {
            Description = description;
        }
    }
}
