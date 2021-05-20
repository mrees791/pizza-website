using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.QuerySearches
{
    public class MenuItemSearch : QuerySearchBase
    {
        public bool AvailableForPurchase { get; set; }

        internal override string GetWhereConditions()
        {
            List<ColumnValuePair> searchValues = new List<ColumnValuePair>()
            {
                new ColumnValuePair("AvailableForPurchase", AvailableForPurchase.ToString())
            };

            return GetWhereConditions(searchValues);
        }
    }
}