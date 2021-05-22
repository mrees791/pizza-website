using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.QueryFilters
{
    /// <summary>
    /// Used to create a where clause using the SQL like operator to filter query results.
    /// </summary>
    public abstract class QueryFilterBase : QueryBase
    {
        protected string GetWhereConditions(List<ColumnValuePair> filters)
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
                        sqlWhereConditions += "and ";
                    }

                    // Only uses the column name with a placeholder to avoid SQL injections.
                    // The column name variable is never set by user input.
                    sqlWhereConditions += $"{columnName} like '%' + @{columnName} + '%' ";
                    queriesAdded = true;
                }
            }

            return sqlWhereConditions;
        }
    }
}