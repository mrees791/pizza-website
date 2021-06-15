using System.Collections.Generic;
using DataLibrary.Models.Sql;

namespace DataLibrary.Models.QueryFilters
{
    public class EmployeeFilter : WhereClauseBase
    {
        public string Id { get; set; }
        public string UserId { get; set; }

        internal override string GetWhereConditions()
        {
            List<WhereClauseItem> items = new List<WhereClauseItem>
            {
                new WhereClauseItem("Id", nameof(Id), Id, ComparisonType.Like),
                new WhereClauseItem("UserId", nameof(UserId), UserId, ComparisonType.Like)
            };

            return GetWhereConditions(items);
        }
    }
}