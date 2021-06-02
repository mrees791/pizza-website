using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.JoinLists
{
    public abstract class JoinListBase<TTable1, TTable2>
        where TTable1 : Record
        where TTable2 : Record
    {
        public IEnumerable<Join2<TTable1, TTable2>> Items { get; protected set; }

        public JoinListBase()
        {
            Items = new List<Join2<TTable1, TTable2>>();
        }

        protected async Task LoadListAsync(string joinQuery, string whereClause, object parameters, bool onlySelectFirst, PizzaDatabase pizzaDb)
        {
            string finalSqlQuery = joinQuery + whereClause;

            Items = await pizzaDb.Connection.QueryAsync<TTable1, TTable2, Join2<TTable1, TTable2>>(
                finalSqlQuery,
                (table1, table2) =>
                {
                    return new Join2<TTable1, TTable2>()
                    {
                        Table1 = table1,
                        Table2 = table2
                    };
                }, param: parameters, splitOn: "Id");

            foreach (var join in Items)
            {
                await join.MapAsync(pizzaDb);
            }
        }
    }
}
