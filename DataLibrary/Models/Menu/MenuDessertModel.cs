using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Menu
{
    public class MenuDessertModel : MenuItemModel
    {
        public decimal Price { get; set; }
        public string Decimal { get; set; }
        public string ItemDetails { get; set; }
    }
}
