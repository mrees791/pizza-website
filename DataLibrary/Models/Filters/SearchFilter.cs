using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Filters
{
    /// <summary>
    /// Used to create an SQL where condition needed by the Dapper get list methods.
    /// It only uses
    /// </summary>
    public class SearchFilter
    {
        private List<FilterPair> filterPairs;

        public SearchFilter()
        {
            filterPairs = new List<FilterPair>();
        }

        public void AddFilter(string columnName, string columnValue)
        {
            filterPairs.Add(new FilterPair(columnName, columnValue));
        }

        internal List<FilterPair> FilterPairs { get => filterPairs; }

        /// <summary>
        /// Creates a where clause used by Dapper's get list conditions parameter.
        /// </summary>
        /// <returns></returns>
        internal string GetSqlConditions()
        {
            int queriesAdded = 0;
            string conditions = string.Empty;

            for (int i = 0; i < filterPairs.Count; i++)
            {
                FilterPair currentPair = filterPairs[i];

                if (currentPair.ColumnValue != null)
                {
                    if (queriesAdded == 0)
                    {
                        conditions += "where ";
                    }
                    else
                    {
                        conditions += "and ";
                    }

                    // Only uses the column name with a placeholder to avoid SQL injections.
                    // The column name variable is never set by user input.
                    conditions += $"{currentPair.ColumnName} like '%' + @{currentPair.ColumnName} + '%'";
                    queriesAdded++;
                }
            }

            return conditions;
        }
    }
}
