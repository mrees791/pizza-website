using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.QueryFilters
{
    public class SiteUserFilter : QueryFilterBase
    {
        public string Id { get; set; }
        public string Email { get; set; }

        internal override string GetWhereConditions()
        {
            List<ColumnValuePair> filters = new List<ColumnValuePair>()
            {
                new ColumnValuePair("Id", Id),
                new ColumnValuePair("Email", Email)
            };

            return GetWhereConditions(filters);
        }
    }
}