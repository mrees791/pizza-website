using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Menu
{
    public class MenuDipModel
    {
        public int MenuDipId { get; set; }
        public bool AvailableForPurchase { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ItemDetails { get; set; }
        public bool HasMenuIcon { get; set; }
        public string MenuIconFile { get; set; }
    }
}
