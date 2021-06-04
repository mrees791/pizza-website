using DataLibrary.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.JoinLists
{
    public class CartItemOnCartPizzaJoin : JoinListBase<CartItem, CartPizza>
    {
        protected override string GetSqlJoinQuery(bool onlySelectFirst)
        {
            throw new NotImplementedException();
        }
    }
}
