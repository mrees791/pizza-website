using DataLibrary.Models.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Carts
{
    public class CartPastaModel : CartItemModel
    {
        public MenuPastaModel MenuPasta { get; set; }

        public override object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
