using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.QueryFilters
{
    public class EmployeeFilter : QueryFilterBase
    {
        public string Id { get; set; }

        internal override string GetWhereConditions(string orderByColumn = null)
        {
            List<ColumnValuePair> filters = new List<ColumnValuePair>()
            {
                new ColumnValuePair("Id", Id)
            };

            return GetWhereConditions(filters, orderByColumn);
        }
    }
}