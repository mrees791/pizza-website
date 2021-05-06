using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    public class ColumnValuePair
    {
        private string column;
        private string value;

        public ColumnValuePair(string column, string value)
        {
            this.column = column;
            this.value = value;
        }

        public string Column { get => column; }
        public string Value { get => value; }
    }
}