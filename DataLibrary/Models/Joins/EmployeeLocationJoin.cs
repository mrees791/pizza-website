using DataLibrary.Models.Tables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Joins
{
    public class EmployeeLocationJoin
    {
        public Employee Employee { get; set; }
        public EmployeeLocation EmployeeLocation { get; set; }

        public async Task MapAsync(PizzaDatabase pizzaDb, IDbTransaction transaction = null)
        {
            await Employee.MapEntityAsync(pizzaDb, transaction);
            await EmployeeLocation.MapEntityAsync(pizzaDb, transaction);
        }
    }
}
