using DataLibrary.Models.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.QueryFilters
{
    public class MenuPizzaFilter : WhereClauseBase
    {
        public string PizzaName { get; set; }

        internal override string GetWhereConditions()
        {
            List<WhereClauseItem> items = new List<WhereClauseItem>()
            {
                new WhereClauseItem("PizzaName", nameof(PizzaName), PizzaName, ComparisonType.Like)
            };

            return GetWhereConditions(items);
        }
    }
}