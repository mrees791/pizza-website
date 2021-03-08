using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Filters
{
    public class FilterPair
    {
        private string columnName;
        private string columnValue;

        public FilterPair(string columnName, string columnValue)
        {
            this.columnName = columnName;
            this.columnValue = columnValue;
        }

        public string ColumnName { get => columnName; }
        public string ColumnValue { get => columnValue; }
    }
}