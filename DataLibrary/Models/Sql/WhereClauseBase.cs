using System.Collections.Generic;
using DataLibrary.Models.Services;

namespace DataLibrary.Models.Sql
{
    public abstract class WhereClauseBase
    {
        private readonly SqlServices _sqlServices;

        public WhereClauseBase()
        {
            _sqlServices = new SqlServices();
        }

        internal abstract string GetWhereConditions();

        protected string GetWhereConditions(List<WhereClauseItem> items)
        {
            return _sqlServices.CreateWhereClause(items);
        }
    }
}