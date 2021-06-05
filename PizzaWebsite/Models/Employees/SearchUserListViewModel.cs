using DataLibrary.Models;
using DataLibrary.Models.QueryFilters;
using DataLibrary.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PizzaWebsite.Models.Employees
{
    public class SearchUserListViewModel : PagedListViewModel<SearchUserViewModel>
    {
        public string EmployeeId { get; set; }

        public async Task InitializeAsync(int page, int rowsPerPage, string userId, string email, Employee currentEmployee, PizzaDatabase pizzaDb, HttpRequestBase request)
        {
            EmployeeId = currentEmployee.Id;

            SiteUserFilter searchFilter = new SiteUserFilter()
            {
                Id = userId,
                Email = email
            };

            int totalNumberOfItems = await pizzaDb.GetNumberOfRecordsAsync<SiteUser>(searchFilter);
            int totalPages = await pizzaDb.GetNumberOfPagesAsync<SiteUser>(rowsPerPage, searchFilter);
            PaginationVm.Initialize(page, rowsPerPage, totalPages, totalNumberOfItems, request.QueryString);

            IEnumerable<SiteUser> userList = await pizzaDb.GetPagedListAsync<SiteUser>(page, rowsPerPage, "Id", SortOrder.Ascending, searchFilter);

            foreach (SiteUser user in userList)
            {
                SearchUserViewModel userVm = new SearchUserViewModel()
                {
                    Id = user.Id,
                    Email = user.Email,
                    IsBanned = user.IsBanned
                };
                ItemViewModelList.Add(userVm);
            }
        }
    }
}