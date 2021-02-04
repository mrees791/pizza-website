using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Menus
{
    public class MenuDipModel : MenuItemWithIconModel
    {
        public decimal Price { get; set; }
        public string ItemDetails { get; set; }
    }
}
