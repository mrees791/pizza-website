using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    public class Join<TTable1, TTable2>
        where TTable1 : Record
        where TTable2 : Record
    {
        public TTable1 Table1 { get; set; }
        public TTable2 Table2 { get; set; }

        public async Task MapAsync(PizzaDatabase pizzaDb, IDbTransaction transaction = null)
        {
            if (Table1 != null)
            {
                await Table1.MapEntityAsync(pizzaDb, transaction);
            }
            if (Table2 != null)
            {
                await Table2.MapEntityAsync(pizzaDb, transaction);
            }
        }
    }
}
