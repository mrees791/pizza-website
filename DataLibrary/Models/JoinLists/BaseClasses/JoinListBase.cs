using Dapper;
using DataLibrary.Models.Services;
using DataLibrary.Models.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.JoinLists.BaseClasses
{
    public abstract class JoinListBase<TTable1, TTable2>
        where TTable1 : Record
        where TTable2 : Record
    {
        protected PagedListServices pagedListServices;
        public IEnumerable<Join<TTable1, TTable2>> Items { get; protected set; }

        public JoinListBase()
        {
            Items = new List<Join<TTable1, TTable2>>();
            pagedListServices = new PagedListServices();
        }

        protected abstract string GetSqlJoinQuery(bool onlySelectFirst);

        protected async Task LoadListAsync(string whereClause, object parameters, bool onlySelectFirst, string orderByColumn, SortOrder sortOrder, PizzaDatabase pizzaDb, string splitOn = "Id", string offsetClause = "")
        {
            string sqlJoinQuery = $@"{GetSqlJoinQuery(onlySelectFirst)}
                                     {whereClause}
                                     {SqlUtility.CreateOrderByClause(orderByColumn, sortOrder)}
                                     {offsetClause}";

            Items = await pizzaDb.Connection.QueryAsync<TTable1, TTable2, Join<TTable1, TTable2>>(
                sqlJoinQuery,
                (table1, table2) =>
                {
                    return new Join<TTable1, TTable2>()
                    {
                        Table1 = table1,
                        Table2 = table2
                    };
                }, param: parameters, splitOn: splitOn);

            foreach (var join in Items)
            {
                await join.MapAsync(pizzaDb);
            }
        }
    }
}
