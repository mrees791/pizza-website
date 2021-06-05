using DataLibrary.Models;
using DataLibrary.Models.JoinLists;
using DataLibrary.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PizzaWebsite.Models.Employees
{
    public class YourStoresViewModel
    {
        public string EmployeeId { get; set; }

        public async Task InitializeAsync(int? page, int? rowsPerPage, Employee currentEmployee, PizzaDatabase pizzaDb, HttpRequestBase request)
        {
            throw new NotImplementedException();
        }
    }
}