﻿using DataLibrary.Models.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.QueryFilters
{
    public class SiteUserFilter : WhereClauseBase
    {
        public string Id { get; set; }
        public string Email { get; set; }

        internal override string GetWhereConditions()
        {
            List<WhereClauseItem> items = new List<WhereClauseItem>()
            {
                new WhereClauseItem("Id", nameof(Id), Id, ComparisonType.Like),
                new WhereClauseItem("Email", nameof(Email), Email, ComparisonType.Like)
            };

            return GetWhereConditions(items);
        }
    }
}