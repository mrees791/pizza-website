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
        private SqlServices _sqlServices;
        internal abstract string GetWhereConditions();

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
