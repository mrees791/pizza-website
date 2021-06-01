using Dapper;
using DataLibrary.Models.Sql;
using DataLibrary.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Joins
{
    public class EmployeeLocationOnStoreLocationJoin
    {
        public EmployeeLocation EmployeeLocation { get; set; }
        public StoreLocation StoreLocation { get; set; }

        public async Task MapAsync(PizzaDatabase pizzaDb)
        {
            await EmployeeLocation.MapEntityAsync(pizzaDb);
            await StoreLocation.MapEntityAsync(pizzaDb);
        }

        public static async Task<IEnumerable<EmployeeLocationOnStoreLocationJoin>> GetListByEmployeeId(string employeeId, PizzaDatabase pizzaDb)
        {
            string whereClause = "where l.EmployeeId = @EmployeeId";

            object parameters = new
            {
                EmployeeId = employeeId
            };

            return await GetListAsync(whereClause, parameters, false, pizzaDb);
        }

        private static async Task<IEnumerable<EmployeeLocationOnStoreLocationJoin>> GetListAsync(string whereClause, object parameters, bool onlySelectFirst, PizzaDatabase pizzaDb)
        {
            string joinQuery = SelectQueries.GetEmployeeLocationOnStoreLocationJoin(onlySelectFirst) + whereClause;

            IEnumerable<EmployeeLocationOnStoreLocationJoin> joinList = await pizzaDb.Connection.QueryAsync<EmployeeLocation, StoreLocation, EmployeeLocationOnStoreLocationJoin>(
                joinQuery,
                (employeeLocation, storeLocation) =>
                {
                    return new EmployeeLocationOnStoreLocationJoin()
                    {
                        EmployeeLocation = employeeLocation,
                        StoreLocation = storeLocation
                    };
                }, param: parameters, splitOn: "Id");

            foreach (EmployeeLocationOnStoreLocationJoin join in joinList)
            {
                await join.MapAsync(pizzaDb);
            }

            return joinList;
        }
    }
}
