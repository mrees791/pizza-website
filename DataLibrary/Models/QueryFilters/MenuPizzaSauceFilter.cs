﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.QueryFilters
{
    public class MenuPizzaSauceFilter : QueryFilterBase
    {
        public string Name { get; set; }

        internal override string GetWhereConditions()
        {
            List<ColumnValuePair> filters = new List<ColumnValuePair>()
            {
                new ColumnValuePair("Name", Name)
            };

            return GetWhereConditions(filters);
        }
    }
}