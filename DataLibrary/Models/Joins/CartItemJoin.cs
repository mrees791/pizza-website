using DataLibrary.Models.Tables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Joins
{
    public class CartItemJoin : JoinRecord, IComparable<CartItemJoin>
    {
        public CartItem CartItem { get; set; }
        public CartItemTypeRecord CartItemType { get; set; }

        public override dynamic GetId()
        {
            return CartItem.Id;
        }

        internal override async Task<dynamic> InsertAsync(PizzaDatabase pizzaDb, IDbTransaction transaction = null)
        {
            await CartItem.InsertAsync(pizzaDb, transaction);
            CartItemType.SetCartItemId(CartItem.Id);
            await CartItemType.InsertAsync(pizzaDb, transaction);

            return CartItem.Id;
        }

        internal override bool InsertRequiresTransaction()
        {
            return true;
        }

        internal override async Task MapEntityAsync(PizzaDatabase pizzaDb, IDbTransaction transaction = null)
        {
            await CartItem.MapEntityAsync(pizzaDb, transaction);
            await CartItemType.MapEntityAsync(pizzaDb, transaction);
        }

        internal override async Task<int> UpdateAsync(PizzaDatabase pizzaDb, IDbTransaction transaction = null)
        {
            int rowsUpdated = 0;

            rowsUpdated += await CartItem.UpdateAsync(pizzaDb, transaction);
            rowsUpdated += await CartItemType.UpdateAsync(pizzaDb, transaction);

            return rowsUpdated;
        }

        internal override bool UpdateRequiresTransaction()
        {
            return true;
        }

        public int CompareTo(CartItemJoin other)
        {
            return CartItem.Id.CompareTo(other.CartItem.Id);
        }
    }
}