using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Interfaces
{
    public interface IRecord
    {
        dynamic GetId();
        // Used for mapping complex objects and lists from the database.
        void MapEntity(PizzaDatabase pizzaDb);

        void Insert(PizzaDatabase pizzaDb, IDbTransaction transaction = null);
        int Update(PizzaDatabase pizzaDb, IDbTransaction transaction = null);
        bool InsertRequiresTransaction();
        bool UpdateRequiresTransaction();
    }
}