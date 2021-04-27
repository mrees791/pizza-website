using DataLibrary.Models.Interfaces;
using DataLibrary.Models.Tables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Joins
{
    public class CartItemJoin : IRecord
    {
        public CartItem CartItem { get; set; }
        public IRecordCartItemType CartItemType { get; set; }

        public dynamic GetId()
        {
            return CartItem.GetId();
        }

        public void Insert(PizzaDatabase pizzaDb, IDbTransaction transaction = null)
        {
            CartItem.Insert(pizzaDb, transaction);
            CartItemType.SetCartItemId(CartItem.GetId());
            CartItemType.Insert(pizzaDb, transaction);
        }

        public bool InsertRequiresTransaction()
        {
            return true;
        }

        public void MapEntity(PizzaDatabase pizzaDb)
        {
            CartItem.MapEntity(pizzaDb);
            CartItemType.MapEntity(pizzaDb);
        }

        public int Update(PizzaDatabase pizzaDb, IDbTransaction transaction = null)
        {
            int rowsUpdated = 0;

            rowsUpdated += CartItem.Update(pizzaDb, transaction);
            rowsUpdated += CartItemType.Update(pizzaDb, transaction);

            return rowsUpdated;
        }

        public bool UpdateRequiresTransaction()
        {
            return true;
        }
    }
}
