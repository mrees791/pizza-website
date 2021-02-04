using DataLibrary.Models.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Carts
{
    public class CartDrinkModel : CartItemModel
    {
        public MenuDrinkModel MenuDrink { get; set; }
        public string Size { get; set; }
    }
}