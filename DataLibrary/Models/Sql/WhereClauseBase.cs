using DataLibrary.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Sql
{
    public abstract class WhereClauseBase
    {
        internal abstract string GetWhereConditions();
        private SqlServices _sqlServices;

        public WhereClauseBase()
        {
            _sqlServices = new SqlServices();
        }

        protected string GetWhereConditions(List<WhereClauseItem> items)
        {
            return _sqlServices.CreateWhereClause(items);
        }
    }
}
