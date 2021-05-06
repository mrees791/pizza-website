using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.QueryFilters
{
    public abstract class QueryFilterBase
    {
        internal abstract string GetWhereConditions(string orderByColumn = null);

        internal string GetWhereConditions(List<ColumnValuePair> filters, string orderByColumn = null)
        {
            string sqlWhereConditions = string.Empty;
            bool queriesAdded = false;

            foreach (ColumnValuePair searchFilter in filters)
            {
                if (!string.IsNullOrEmpty(searchFilter.Value))
                {
                    string columnName = searchFilter.Column;

                    if (!queriesAdded)
                    {
                        sqlWhereConditions += "where ";
                    }
                    else
                    {
                        sqlWhereConditions += " and ";
                    }

                    // Only uses the column name with a placeholder to avoid SQL injections.
                    // The column name variable is never set by user input.
                    sqlWhereConditions += $"{columnName} like '%' + @{columnName} + '%'";
                    queriesAdded = true;
                }
            }

            if (!string.IsNullOrEmpty(orderByColumn))
            {
                // Order by column is never set by user input to avoid SQL injections.
                sqlWhereConditions += $"order by {orderByColumn}";
            }

            return sqlWhereConditions;
        }
    }
}