using DataLibrary.Models.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.QueryFilters
{
    public class EmployeeFilter : WhereClauseBase
    {
        public string Id { get; set; }
        public string UserId { get; set; }

        internal override string GetWhereConditions()
        {
            List<WhereClauseItem> items = new List<WhereClauseItem>()
            {
                new WhereClauseItem("Id", nameof(Id), Id, ComparisonType.Like),
                new WhereClauseItem("UserId", nameof(UserId), UserId, ComparisonType.Like)
            };

            return GetWhereConditions(items);
        }
    }
}