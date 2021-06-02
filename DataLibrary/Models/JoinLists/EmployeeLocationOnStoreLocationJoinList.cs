using Dapper;
using DataLibrary.Models.Sql;
using DataLibrary.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.JoinLists
{
    public class EmployeeLocationOnStoreLocationJoinList : JoinListBase<EmployeeLocation, StoreLocation>
    {
        public EmployeeLocationOnStoreLocationJoinList() : base()
        {
        }

        public async Task LoadListByEmployeeIdAsync(string employeeId, PizzaDatabase pizzaDb)
        {
            string whereClause = "where l.EmployeeId = @EmployeeId";

            object parameters = new
            {
                EmployeeId = employeeId
            };

            await LoadListAsync(whereClause, parameters, false, pizzaDb);
        }

        private async Task LoadListAsync(string whereClause, object parameters, bool onlySelectFirst, PizzaDatabase pizzaDb)
        {
            string joinQuery = SelectQueries.GetEmployeeLocationOnStoreLocationJoin(onlySelectFirst);
            await LoadListAsync(joinQuery, whereClause, parameters, onlySelectFirst, pizzaDb);
        }
    }
}