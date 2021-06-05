using DataLibrary.Models;
using DataLibrary.Models.QueryFilters;
using DataLibrary.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PizzaWebsite.Models.ManageEmployees
{
    public class ManageEmployeesIndexViewModel : PagedListViewModel<ManageEmployeeViewModel>
    {
        public async Task InitializeAsync(int page, int rowsPerPage, string employeeId, string userId, PizzaDatabase pizzaDb, HttpRequestBase request)
        {
            EmployeeFilter searchFilter = new EmployeeFilter()
            {
                Id = employeeId,
                UserId = userId
            };

            int totalNumberOfItems = await pizzaDb.GetNumberOfRecordsAsync<Employee>(searchFilter);
            int totalPages = await pizzaDb.GetNumberOfPagesAsync<Employee>(rowsPerPage, searchFilter);
            PaginationVm.Initialize(page, rowsPerPage, totalPages, totalNumberOfItems, request.QueryString);

            SiteRole managerRole = await pizzaDb.GetSiteRoleByNameAsync("Manager");
            IEnumerable<Employee> employeeList = await pizzaDb.GetPagedListAsync<Employee>(page, rowsPerPage, "Id", SortOrder.Ascending, searchFilter);

            foreach (Employee employee in employeeList)
            {
                bool isManager = await pizzaDb.UserIsInRole(employee.UserId, managerRole);

                ManageEmployeeViewModel employeeVm = new ManageEmployeeViewModel()
                {
                    Id = employee.Id,
                    UserId = employee.UserId,
                    IsManager = isManager
                };
                ItemViewModelList.Add(employeeVm);
            }

        }
    }
}