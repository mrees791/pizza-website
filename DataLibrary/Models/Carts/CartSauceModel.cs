using DataLibrary.Models.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Carts
{
    public class CartSauceModel : CartItemModel
    {
        public MenuSauceModel MenuSauce { get; set; }

        public override object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
