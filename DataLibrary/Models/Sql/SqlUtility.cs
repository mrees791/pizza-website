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
        internal static string CreateWhereClause(List<WhereClauseItem> items)
        {
            string sqlClause = string.Empty;
            bool queriesAdded = false;

            foreach (WhereClauseItem item in items)
            {
                if (!string.IsNullOrEmpty(item.Value))
                {
                    string columnName = item.ColumnName;
                    string placeholderName = item.PlaceholderName;

                    if (!queriesAdded)
                    {
                        sqlClause += "WHERE ";
                    }
                    else
                    {
                        sqlClause += "AND ";
                    }

                    // A placeholder is used to avoid SQL injections.
                    // The column name variable is never set by user input.
                    switch (item.ComparisonType)
                    {
                        case ComparisonType.Equals:
                            sqlClause += $"{columnName} = @{placeholderName} ";
                            break;
                        case ComparisonType.Like:
                            sqlClause += $"{columnName} LIKE '%' + @{placeholderName} + '%' ";
                            break;
                    }
                    queriesAdded = true;
                }
            }

            return sqlClause;
        }

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

        internal static string CreateOffsetClause()
        {
            return @"OFFSET @CurrentOffset ROWS
                     FETCH NEXT @RowsPerPage ROWS ONLY";
        }
    }
}
