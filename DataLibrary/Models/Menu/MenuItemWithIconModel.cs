using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Menu
{
    public abstract class MenuItemWithIconModel : MenuItemModel
    {
        public bool HasMenuIcon { get; set; }
        public string MenuIconFile { get; set; }

        public MenuItemWithIconModel(int id, bool availableForPurchase, string name, bool hasMenuIcon, string menuIconFile) : base(id, availableForPurchase, name)
        {
            HasMenuIcon = hasMenuIcon;
            MenuIconFile = menuIconFile;
        }
    }
}
