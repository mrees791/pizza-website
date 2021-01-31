using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Carts
{
    public abstract class CartItemModel
    {
        public int Id { get; set; }
        public CartModel Cart { get; set; }
        public decimal PricePerItem { get; set; }
        public int Quantity { get; set; }
    }
}
