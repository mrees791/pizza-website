using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.QuerySearches
{
    public class StoreLocationSearch : QuerySearchBase
    {
        public bool IsActiveLocation { get; set; }

        internal override string GetWhereConditions(string orderByColumn = null)
        {
            List<ColumnValuePair> searchValues = new List<ColumnValuePair>()
            {
                new ColumnValuePair("IsActiveLocation", IsActiveLocation.ToString())
            };

            return GetWhereConditions(searchValues, orderByColumn);
        }
    }
}
