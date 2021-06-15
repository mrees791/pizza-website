using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using DataLibrary.Models.Services;

namespace DataLibrary.Models.JoinLists.BaseClasses
{
    public abstract class JoinListBase<TTable1, TTable2>
        where TTable1 : Record
        where TTable2 : Record
    {
        protected PagedListServices PagedListServices;
        internal SqlServices SqlServices;

        public JoinListBase()
        {
            Items = new List<Join<TTable1, TTable2>>();
            PagedListServices = new PagedListServices();
            SqlServices = new SqlServices();
        }

        public IEnumerable<Join<TTable1, TTable2>> Items { get; protected set; }

        protected abstract string GetSqlJoinQuery(bool onlySelectFirst);

        protected async Task LoadListAsync(string whereClause, object parameters, bool onlySelectFirst,
            string orderByColumn, SortOrder sortOrder, PizzaDatabase pizzaDb, string splitOn = "Id",
            string offsetClause = "")
        {
            string sqlJoinQuery = $@"{GetSqlJoinQuery(onlySelectFirst)}
                                     {whereClause}
                                     {SqlServices.CreateOrderByClause(orderByColumn, sortOrder)}
                                     {offsetClause}";

            Items = await pizzaDb.Connection.QueryAsync<TTable1, TTable2, Join<TTable1, TTable2>>(
                sqlJoinQuery,
                (table1, table2) =>
                {
                    return new Join<TTable1, TTable2>
                    {
                        Table1 = table1,
                        Table2 = table2
                    };
                }, parameters, splitOn: splitOn);

            foreach (Join<TTable1, TTable2> join in Items)
            {
                await join.MapAsync(pizzaDb);
            }
        }
    }
}