using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Sql
{
    public class WhereClauseItem
    {
        public string ColumnName { get; set; }
        public string PlaceholderName { get; set; }
        public string Value { get; set; }
        public ComparisonType ComparisonType { get; set; }

        public WhereClauseItem(string columnName, string placeholderName, string value, ComparisonType comparisonType)
        {
            ColumnName = columnName;
            PlaceholderName = placeholderName;
            Value = value;
            ComparisonType = comparisonType;
        }
    }
}
