using System.Collections.Generic;
using DataLibrary.Models.Sql;

namespace DataLibrary.Models.QuerySearches
{
    public class MenuPizzaSearch : WhereClauseBase
    {
        public bool AvailableForPurchase { get; set; }
        public string CategoryName { get; set; }

        internal override string GetWhereConditions()
        {
            List<WhereClauseItem> items = new List<WhereClauseItem>
            {
                new WhereClauseItem("AvailableForPurchase", nameof(AvailableForPurchase),
                    AvailableForPurchase.ToString(), ComparisonType.Equals),
                new WhereClauseItem("CategoryName", nameof(CategoryName), CategoryName, ComparisonType.Equals)
            };

            return GetWhereConditions(items);
        }
    }
}