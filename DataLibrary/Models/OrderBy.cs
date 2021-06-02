using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    // todo: Remove. SqlUtility will create order by keywords.
    internal class OrderBy
    {
        public string OrderByColumn { get; set; }
        public SortOrder SortOrder { get; set; }

        public OrderBy()
        {
            SortOrder = SortOrder.Ascending;
        }

        public string GetConditions()
        {
            string conditions = $"{OrderByColumn} ";

            switch (SortOrder)
            {
                case SortOrder.Ascending:
                    conditions += "asc";
                    break;
                case SortOrder.Descending:
                    conditions += "desc";
                    break;
            }

            return conditions;
        }
    }
}
