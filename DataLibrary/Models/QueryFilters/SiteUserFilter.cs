using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.QueryFilters
{
    public class SiteUserFilter : QueryFilterBase
    {
        public string UserName { get; set; }
        public string Email { get; set; }

        internal override string GetWhereConditions()
        {
            List<ColumnValuePair> filters = new List<ColumnValuePair>()
            {
                new ColumnValuePair("UserName", UserName),
                new ColumnValuePair("Email", Email)
            };

            return GetWhereConditions(filters);
        }
    }
}