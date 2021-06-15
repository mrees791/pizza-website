using System.Threading.Tasks;
using DataLibrary.Models.JoinLists.BaseClasses;
using DataLibrary.Models.Tables;

namespace DataLibrary.Models.JoinLists
{
    public class EmployeeOnEmployeeLocationJoinList : JoinListBase<Employee, EmployeeLocation>
    {
        public async Task LoadListByStoreIdAsync(int storeId, PizzaDatabase pizzaDb)
        {
            string whereClause = "WHERE l.StoreId = @StoreId";

            object parameters = new
            {
                StoreId = storeId
            };

            await LoadListAsync(whereClause, parameters, false, "l.EmployeeId", SortOrder.Ascending, pizzaDb);
        }

        public async Task LoadListByEmployeeIdAsync(string employeeId, PizzaDatabase pizzaDb)
        {
            string whereClause = "WHERE l.EmployeeId = @EmployeeId";

            object parameters = new
            {
                EmployeeId = employeeId
            };

            await LoadListAsync(whereClause, parameters, false, "l.EmployeeId", SortOrder.Ascending, pizzaDb);
        }

        protected override string GetSqlJoinQuery(bool onlySelectFirst)
        {
            return $@"SELECT {SqlServices.CreateTopClause(onlySelectFirst)}
                      e.Id, e.UserId, l.Id, l.EmployeeId, l.StoreId
                      FROM Employee e
                      INNER JOIN EmployeeLocation l
                      ON l.EmployeeId = e.Id";
        }
    }
}