using DataLibrary.Models.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.QuerySearches
{
    public class DeliveryAddressSearch : WhereClauseBase
    {
        public string UserId { get; set; }

        internal override string GetWhereConditions()
        {
            List<WhereClauseItem> items = new List<WhereClauseItem>()
            {
                new WhereClauseItem("UserId", nameof(UserId), UserId, ComparisonType.Equals)
            };

            return GetWhereConditions(items);
        }
    }
}