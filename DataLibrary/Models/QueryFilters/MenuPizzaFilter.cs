using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.QueryFilters
{
    public class MenuPizzaFilter : QueryFilterBase
    {
        public string PizzaName { get; set; }

        internal override string GetWhereConditions()
        {
            List<ColumnValuePair> filters = new List<ColumnValuePair>()
            {
                new ColumnValuePair("PizzaName", PizzaName)
            };

            return GetWhereConditions(filters);
        }
    }
}