using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    /// <summary>
    /// Used to represent basic database entities: records (Record class) and joins (Join class.)
    /// </summary>
    public abstract class EntityBase
    {
        abstract public dynamic GetId();
        abstract internal bool InsertRequiresTransaction();
        abstract internal bool UpdateRequiresTransaction();
        abstract internal Task MapEntityAsync(PizzaDatabase pizzaDb, IDbTransaction transaction = null);
        abstract internal Task<dynamic> InsertAsync(PizzaDatabase pizzaDb, IDbTransaction transaction = null);
        abstract internal Task<int> UpdateAsync(PizzaDatabase pizzaDb, IDbTransaction transaction = null);
    }
}
