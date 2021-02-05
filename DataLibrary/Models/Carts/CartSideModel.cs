using DataLibrary.Models.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Carts
{
    public class CartSideModel : CartItemModel
    {
        public MenuSideModel MenuSide { get; set; }

        public override object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
