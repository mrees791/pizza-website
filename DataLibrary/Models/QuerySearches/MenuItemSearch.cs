using DataLibrary.Models.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.QuerySearches
{
    public class MenuItemSearch : WhereClauseBase
    {
        public bool AvailableForPurchase { get; set; }

        internal override string GetWhereConditions()
        {
            List<WhereClauseItem> items = new List<WhereClauseItem>()
            {
                new WhereClauseItem("AvailableForPurchase", nameof(AvailableForPurchase), AvailableForPurchase.ToString(), ComparisonType.Equals)
            };

            return GetWhereConditions(items);
        }
    }
}