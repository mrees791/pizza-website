using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Sql
{
    /// <summary>
    /// Used for creating SQL query strings.
    /// </summary>
    internal static class SqlUtility
    {
        internal static string CreateTopClause(bool onlySelectFirst)
        {
            return onlySelectFirst ? "top 1" : "";
        }

        internal static string CreateOrderBy(string orderByColumn, SortOrder sortOrder)
        {
            string conditions = $"order by {orderByColumn} ";

            switch (sortOrder)
            {
                case SortOrder.Ascending:
                    conditions += "asc";
                    break;
                case SortOrder.Descending:
                    conditions += "desc";
                    break;
            }

            return conditions;
        }
    }
}
