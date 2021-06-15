using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using DataLibrary.Models.Services;
using DataLibrary.Models.Tables;

namespace DataLibrary.Models.JoinLists.BaseClasses
{
    /// <summary>
    ///     Provides a base join class for each cart item category table..
    /// </summary>
    /// <typeparam name="TCategoryTable"></typeparam>
    public abstract class CartItemJoinListBase<TCategoryTable>
        where TCategoryTable : CartItemCategory
    {
        internal SqlServices SqlServices;

        public CartItemJoinListBase()
        {
            Items = new List<CartItemJoin>();
            SqlServices = new SqlServices();
        }

        public IEnumerable<CartItemJoin> Items { get; protected set; }
        public abstract Task LoadListByCartIdAsync(int cartId, PizzaDatabase pizzaDb);
        protected abstract string GetSqlJoinQuery(bool onlySelectFirst);

        protected async Task LoadListAsync(string whereClause, object parameters, bool onlySelectFirst,
            string orderByColumn, SortOrder sortOrder, PizzaDatabase pizzaDb)
        {
            string sqlJoinQuery =
                $"{GetSqlJoinQuery(onlySelectFirst)} {whereClause} {SqlServices.CreateOrderByClause(orderByColumn, sortOrder)}";

            Items = await pizzaDb.Connection.QueryAsync<CartItem, TCategoryTable, CartItemJoin>(
                sqlJoinQuery,
                (table1, table2) => { return new CartItemJoin(table1, table2); }, parameters,
                splitOn: "CartItemId");

            foreach (CartItemJoin join in Items)
            {
                await join.MapAsync(pizzaDb);
            }
        }
    }
}