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
        public string UserId { get; set; }

        internal override string GetWhereConditions()
        {
            List<ColumnValuePair> filters = new List<ColumnValuePair>()
            {
                new ColumnValuePair("Id", Id),
                new ColumnValuePair("UserId", UserId)
            };

            return GetWhereConditions(filters);
        }
    }
}