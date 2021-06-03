using DataLibrary.Models.Sql;
using DataLibrary.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.JoinLists
{
    public class EmployeeOnEmployeeLocationJoin : JoinListBase<Employee, EmployeeLocation>
    {
        public async Task LoadListByStoreIdAsync(int storeId, PizzaDatabase pizzaDb)
        {
            string whereClause = "where l.StoreId = @StoreId";

            object parameters = new
            {
                StoreId = storeId
            };

            await LoadListAsync(whereClause, parameters, false, "l.EmployeeId", SortOrder.Ascending, pizzaDb);
        }

        public async Task LoadListByEmployeeIdAsync(string employeeId, PizzaDatabase pizzaDb)
        {
            string whereClause = "where l.EmployeeId = @EmployeeId";

            object parameters = new
            {
                EmployeeId = employeeId
            };

            await LoadListAsync(whereClause, parameters, false, "l.EmployeeId", SortOrder.Ascending, pizzaDb);
        }

        protected override string GetSqlJoinQuery(bool onlySelectFirst)
        {
            return $"select {SqlUtility.CreateTopClause(onlySelectFirst)}" +
                @"e.Id, e.UserId, l.Id, l.EmployeeId, l.StoreId from Employee e inner join EmployeeLocation l on l.EmployeeId = e.Id";
        }
    }
}
