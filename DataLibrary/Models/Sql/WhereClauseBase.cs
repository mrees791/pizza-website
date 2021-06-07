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

        protected string GetWhereConditions(List<WhereClauseItem> items)
        {
            return SqlUtility.GetWhereClause(items);
        }
    }
}
