using System.Collections.Generic;
using DataLibrary.Models.Sql;

namespace DataLibrary.Models.QueryFilters
{
    public class MenuPizzaFilter : WhereClauseBase
    {
        public string PizzaName { get; set; }

        internal override string GetWhereConditions()
        {
            List<WhereClauseItem> items = new List<WhereClauseItem>
            {
                new WhereClauseItem("PizzaName", nameof(PizzaName), PizzaName, ComparisonType.Like)
            };

            return GetWhereConditions(items);
        }
    }
}