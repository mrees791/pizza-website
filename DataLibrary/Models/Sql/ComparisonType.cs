using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Sql
{
    /// <summary>
    /// Used by the WhereClauseItem to determine what kind of comparison should be performed.
    /// </summary>
    public enum ComparisonType
    {
        Equals, Like
    }
}
