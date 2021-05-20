using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.QuerySearches
{
    /// <summary>
    /// Used to create a where clause from a list of search values.
    /// </summary>
    public abstract class QuerySearchBase
    {
        internal abstract string GetWhereConditions();

        protected string GetWhereConditions(List<ColumnValuePair> searchValues)
        {
            string sqlWhereConditions = string.Empty;
            bool queriesAdded = false;

            foreach (ColumnValuePair searchValue in searchValues)
            {
                if (!string.IsNullOrEmpty(searchValue.Value))
                {
                    string columnName = searchValue.Column;

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
                    sqlWhereConditions += $"{columnName} = @{columnName} ";
                    queriesAdded = true;
                }
            }

            return sqlWhereConditions;
        }
    }
}