using System.Data;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    public abstract class Record
    {
        public abstract dynamic GetId();
        internal abstract bool InsertRequiresTransaction();
        internal abstract bool UpdateRequiresTransaction();
        internal abstract Task MapEntityAsync(PizzaDatabase pizzaDb, IDbTransaction transaction = null);
        internal abstract Task<dynamic> InsertAsync(PizzaDatabase pizzaDb, IDbTransaction transaction = null);
        internal abstract Task<int> UpdateAsync(PizzaDatabase pizzaDb, IDbTransaction transaction = null);
    }
}