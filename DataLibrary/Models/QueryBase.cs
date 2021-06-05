using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    /// <summary>
    /// Used as a base class for QueryFilterBase and QuerySearchBase for creating SQL where clauses.
    /// </summary>
    public abstract class QueryBase
    {
        internal abstract string GetWhereConditions();

    }
}
