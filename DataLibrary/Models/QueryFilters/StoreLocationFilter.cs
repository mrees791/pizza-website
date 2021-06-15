using System.Collections.Generic;
using DataLibrary.Models.Sql;

namespace DataLibrary.Models.QueryFilters
{
    public class StoreLocationFilter : WhereClauseBase
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }

        internal override string GetWhereConditions()
        {
            List<WhereClauseItem> items = new List<WhereClauseItem>
            {
                new WhereClauseItem("Name", nameof(Name), Name, ComparisonType.Like),
                new WhereClauseItem("PhoneNumber", nameof(PhoneNumber), PhoneNumber, ComparisonType.Like)
            };

            return GetWhereConditions(items);
        }
    }
}