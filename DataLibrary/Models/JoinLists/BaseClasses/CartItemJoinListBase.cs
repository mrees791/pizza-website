using Dapper;
using DataLibrary.Models.Services;
using DataLibrary.Models.Sql;
using DataLibrary.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.JoinLists.BaseClasses
{
    /// <summary>
    /// Provides a base join class for each cart item category table..
    /// </summary>
    /// <typeparam name="TCategoryTable"></typeparam>
    public abstract class CartItemJoinListBase<TCategoryTable>
        where TCategoryTable : CartItemCategory
    {
        public IEnumerable<CartItemJoin> Items { get; protected set; }
        public abstract Task LoadListByCartIdAsync(int cartId, PizzaDatabase pizzaDb);
        protected abstract string GetSqlJoinQuery(bool onlySelectFirst);
        internal SqlServices sqlServices;

        public CartItemJoinListBase()
        {
            Items = new List<CartItemJoin>();
            sqlServices = new SqlServices();
        }

        protected async Task LoadListAsync(string whereClause, object parameters, bool onlySelectFirst, string orderByColumn, SortOrder sortOrder, PizzaDatabase pizzaDb)
        {
            string sqlJoinQuery = $"{GetSqlJoinQuery(onlySelectFirst)} {whereClause} {sqlServices.CreateOrderByClause(orderByColumn, sortOrder)}";

            Items = await pizzaDb.Connection.QueryAsync<CartItem, TCategoryTable, CartItemJoin>(
                sqlJoinQuery,
                (table1, table2) =>
                {
                    return new CartItemJoin(table1, table2);
                }, param: parameters, splitOn: "CartItemId");

            foreach (var join in Items)
            {
                await join.MapAsync(pizzaDb);
            }
        }
    }
}
