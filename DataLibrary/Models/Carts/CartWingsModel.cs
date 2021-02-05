using DataLibrary.Models.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Carts
{
    public class CartWingsModel : CartItemModel
    {
        public MenuWingsModel MenuWings { get; set; }
        public MenuWingsSauceModel MenuWingsSauce { get; set; }
        public int PieceAmount { get; set; }

        public override object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
