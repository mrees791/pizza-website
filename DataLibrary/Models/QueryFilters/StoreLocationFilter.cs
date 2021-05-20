using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.QueryFilters
{
    public class StoreLocationFilter : QueryFilterBase
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }

        internal override string GetWhereConditions()
        {
            List<ColumnValuePair> filters = new List<ColumnValuePair>()
            {
                new ColumnValuePair("Name", Name),
                new ColumnValuePair("PhoneNumber", PhoneNumber)
            };

            return GetWhereConditions(filters);
        }
    }
}