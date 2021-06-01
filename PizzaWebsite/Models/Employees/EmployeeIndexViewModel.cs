using DataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PizzaWebsite.Models.Employees
{
    public class EmployeeIndexViewModel
    {
        public string EmployeeId { get; set; }

        public async Task InitializeAsync(string userId, PizzaDatabase pizzaDb)
        {
            //Employee employee = await pizzaDb.GetEmployeeLocationAsync
        }
    }
}