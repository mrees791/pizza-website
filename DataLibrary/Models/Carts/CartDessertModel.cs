using DataLibrary.Models.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Carts
{
    public class CartDessertModel : CartItemModel
    {
        public MenuDessertModel MenuDessert { get; set; }

        public override object Clone()
        {
            throw new NotImplementedException();
        }
    }
}