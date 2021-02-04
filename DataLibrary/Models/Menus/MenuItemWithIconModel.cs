using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Menus
{
    public abstract class MenuItemWithIconModel : MenuItemModel
    {
        public bool HasMenuIcon { get; set; }
        public string MenuIconFile { get; set; }
    }
}
