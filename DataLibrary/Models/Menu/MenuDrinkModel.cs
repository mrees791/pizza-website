using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Menu
{
    public class MenuDrinkModel : MenuItemWithIconModel
    {
        public bool AvailableIn20Oz { get; set; }
        public bool AvailableIn2Liter { get; set; }
        public bool AvailableIn2Pack12Oz { get; set; }
        public bool AvailableIn6Pack12Oz { get; set; }
        public decimal Price20Oz { get; set; }
        public decimal Price2Liter { get; set; }
        public decimal Price2Pack12Oz { get; set; }
        public decimal Price6Pack12Oz { get; set; }
        public string Description { get; set; }

        public MenuDrinkModel(int id, bool availableForPurchase, string name, bool hasMenuIcon, string menuIconFile,
            bool availableIn20Oz, bool availableIn2Liter, bool availableIn2Pack12Oz, bool availableIn6Pack12Oz,
            decimal price20Oz, decimal price2Liter, decimal price2Pack12Oz, decimal price6Pack12Oz, string description) :
            base(id, availableForPurchase, name, hasMenuIcon, menuIconFile)
        {
            AvailableIn20Oz = availableIn20Oz;
            AvailableIn2Liter = availableIn2Liter;
            AvailableIn2Pack12Oz = availableIn2Pack12Oz;
            AvailableIn6Pack12Oz = availableIn6Pack12Oz;
            Price20Oz = price20Oz;
            Price2Liter = price2Liter;
            Price2Pack12Oz = price2Pack12Oz;
            Price6Pack12Oz = price6Pack12Oz;
            Description = description;
        }
    }
}
