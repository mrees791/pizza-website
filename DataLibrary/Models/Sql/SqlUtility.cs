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
        /// <summary>
        /// Creates the start of a select SQL query and includes only the top record if needed.
        /// </summary>
        /// <param name="selectOnlyTopRecord"></param>
        /// <returns></returns>
        internal static string CreateSelectQueryStart(bool selectOnlyTopRecord)
        {
            string topClause = "";

            if (selectOnlyTopRecord)
            {
                topClause = "top 1 ";
            }

            return $"select {topClause} ";
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
