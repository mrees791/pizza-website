﻿using Dapper;
using DataLibrary.Models.Sql;
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

        protected abstract string GetSqlJoinQuery(bool onlySelectFirst);

        protected async Task LoadListAsync(string whereClause, object parameters, bool onlySelectFirst, string orderByColumn, SortOrder sortOrder, PizzaDatabase pizzaDb, string splitOn = "Id")
        {
            string sqlJoinQuery = $"{GetSqlJoinQuery(onlySelectFirst)} {whereClause} {SqlUtility.CreateOrderBy(orderByColumn, sortOrder)}";

            Items = await pizzaDb.Connection.QueryAsync<TTable1, TTable2, Join2<TTable1, TTable2>>(
                sqlJoinQuery,
                (table1, table2) =>
                {
                    return new Join2<TTable1, TTable2>()
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
