using Dapper;
using DataLibrary.Models.JoinLists.BaseClasses;
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
        public async Task LoadListByEmployeeIdAsync(string employeeId, PizzaDatabase pizzaDb)
        {
            string whereClause = "WHERE l.EmployeeId = @EmployeeId";

            object parameters = new
            {
                EmployeeId = employeeId
            };

            await LoadListAsync(whereClause, parameters, false, "s.Name", SortOrder.Ascending, pizzaDb);
        }

        protected override string GetSqlJoinQuery(bool onlySelectFirst)
        {
            return $@"SELECT {SqlUtility.CreateTopClause(onlySelectFirst)}
                      l.Id, l.EmployeeId, l.StoreId, s.Id, s.Name, s.StreetAddress, s.City, s.State, s.ZipCode, s.PhoneNumber, s.isActiveLocation
                      FROM EmployeeLocation l
                      INNER JOIN StoreLocation s
                      ON l.StoreId = s.Id";
        }
    }
}