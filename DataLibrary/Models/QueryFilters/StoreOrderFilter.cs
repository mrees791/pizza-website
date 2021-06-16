using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using DataLibrary.Models.Sql;

namespace DataLibrary.Models.QueryFilters
{
    public class StoreOrderFilter : WhereClauseBase
    {
        public int StoreId { get; set; }
        public string UserId { get; set; }

        internal override string GetWhereConditions()
        {
            List<WhereClauseItem> items = new List<WhereClauseItem>
            {
                new WhereClauseItem("StoreId", nameof(StoreId), StoreId.ToString(), ComparisonType.Equals),
                new WhereClauseItem("UserId", nameof(UserId), UserId, ComparisonType.Like)
            };

            return GetWhereConditions(items);
        }
    }
}
