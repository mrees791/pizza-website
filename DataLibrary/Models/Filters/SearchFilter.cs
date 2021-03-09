using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Filters
{
    /// <summary>
    /// Used to create an SQL where condition clause needed by the Dapper get list methods.
    /// </summary>
    public abstract class SearchFilter
    {
        /// <summary>
        /// Creates a where clause which can be used to run queries with filters using the like operator.
        /// This is used by Simple Dapper's get list methods.
        /// </summary>
        /// <returns>An SQL where clause.</returns>
        internal string GetSqlWhereFilterClause()
        {
            int queriesAdded = 0;
            string sqlWhereClause = string.Empty;

            foreach (PropertyInfo propertyInfo in GetType().GetProperties())
            {
                Type propertyType = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;

                object propertyValue = propertyInfo.GetValue(this);

                if (propertyValue != null)
                {
                    string columnName = propertyInfo.Name;

                    if (queriesAdded == 0)
                    {
                        sqlWhereClause += "where ";
                    }
                    else
                    {
                        sqlWhereClause += "and ";
                    }

                    // Only uses the column name with a placeholder to avoid SQL injections.
                    // The column name variable is never set by user input.
                    sqlWhereClause += $"{columnName} like '%' + @{columnName} + '%'";
                    queriesAdded++;
                }
            }

            return sqlWhereClause;
        }
    }
}