using DataLibrary.Models.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.QuerySearches
{
    public class StoreLocationSearch : WhereClauseBase
    {
        public bool IsActiveLocation { get; set; }

        internal override string GetWhereConditions()
        {
            List<WhereClauseItem> items = new List<WhereClauseItem>()
            {
                new WhereClauseItem("IsActiveLocation", nameof(IsActiveLocation), IsActiveLocation.ToString(), ComparisonType.Equals)
            };

            return GetWhereConditions(items);
        }
    }
}
