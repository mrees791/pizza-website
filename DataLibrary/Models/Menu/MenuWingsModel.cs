using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Menu
{
    public class MenuWingsModel : MenuItemModel
    {
        public decimal Price6Piece { get; set; }
        public decimal Price12Piece { get; set; }
        public decimal Price18Piece { get; set; }
        public string Description { get; set; }
    }
}
