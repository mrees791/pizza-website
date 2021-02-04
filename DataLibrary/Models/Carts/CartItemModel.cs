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
        public int CartId { get; set; }
        public decimal PricePerItem { get; set; }
        public int Quantity { get; set; }
        public DateTime DateAddedToCart { get; set; }
    }
}
