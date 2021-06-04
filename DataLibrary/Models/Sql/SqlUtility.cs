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
            return onlySelectFirst ? "TOP 1" : "";
        }

        internal static string CreateOrderByClause(string orderByColumn, SortOrder sortOrder)
        {
            string conditions = $"ORDER BY {orderByColumn} ";

            switch (sortOrder)
            {
                case SortOrder.Ascending:
                    conditions += "ASC";
                    break;
                case SortOrder.Descending:
                    conditions += "DESC";
                    break;
            }

            return conditions;
        }
    }
}
