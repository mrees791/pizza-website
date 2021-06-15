using System.Collections.Generic;
using DataLibrary.Models.Sql;

namespace DataLibrary.Models.QueryFilters
{
    public class MenuPizzaCrustFilter : WhereClauseBase
    {
        public string Name { get; set; }

        internal override string GetWhereConditions()
        {
            List<WhereClauseItem> items = new List<WhereClauseItem>
            {
                new WhereClauseItem("Name", nameof(Name), Name, ComparisonType.Like)
            };

            return GetWhereConditions(items);
        }
    }
}