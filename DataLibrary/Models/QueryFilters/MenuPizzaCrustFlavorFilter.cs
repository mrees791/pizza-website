using DataLibrary.Models.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.QueryFilters
{
    public class MenuPizzaCrustFlavorFilter : WhereClauseBase
    {
        public string Name { get; set; }

        internal override string GetWhereConditions()
        {
            List<WhereClauseItem> items = new List<WhereClauseItem>()
            {
                new WhereClauseItem("Name", nameof(Name), Name, ComparisonType.Like)
            };

            return GetWhereConditions(items);
        }
    }
}