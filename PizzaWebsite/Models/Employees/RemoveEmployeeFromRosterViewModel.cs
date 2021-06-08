using DataLibrary.Models;
using DataLibrary.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PizzaWebsite.Models.Employees
{
    public class RemoveEmployeeFromRosterViewModel
    {
        public int EmployeeLocationId { get; set; }
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public string EmployeeId { get; set; }

        public async Task InitializeAsync(int employeeLocationId, PizzaDatabase pizzaDb)
        {
            EmployeeLocation employeeLocation = await pizzaDb.GetAsync<EmployeeLocation>(employeeLocationId);
            StoreLocation storeLocation = await pizzaDb.GetAsync<StoreLocation>(employeeLocation.StoreId);
            Employee employee = await pizzaDb.GetAsync<Employee>(employeeLocation.EmployeeId);

            EmployeeLocationId = employeeLocationId;
            StoreId = storeLocation.Id;
            StoreName = storeLocation.Name;
            EmployeeId = employee.Id;
        }

        public async Task ValidateAsync(ModelStateDictionary modelState, PizzaDatabase pizzaDb)
        {
            // Make sure employee exists.
            Employee employee = await pizzaDb.GetAsync<Employee>(EmployeeId);

            if (employee == null)
            {
                modelState.AddModelError(nameof(EmployeeId), $"Employee with ID {EmployeeId} does not exist.");
            }
            else
            {
                // Make sure store exists.
                StoreLocation storeLocation = await pizzaDb.GetAsync<StoreLocation>(StoreId);

                if (storeLocation == null)
                {
                    modelState.AddModelError("", $"Store with ID {StoreId} does not exist.");
                }
                else
                {
                    // Make sure employee isn't already employed at this location.
                    bool alreadyEmployedAtLocation = await pizzaDb.Commands.IsEmployedAtLocation(employee, storeLocation);

                    if (!alreadyEmployedAtLocation)
                    {
                        modelState.AddModelError(nameof(EmployeeId), $"Employee with ID {EmployeeId} is not employed at {StoreName}.");
                    }
                }
            }
        }
    }
}