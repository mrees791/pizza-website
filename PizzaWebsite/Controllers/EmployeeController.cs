using DataLibrary.Models;
using DataLibrary.Models.QueryFilters;
using DataLibrary.Models.Tables;
using PizzaWebsite.Models;
using PizzaWebsite.Models.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PizzaWebsite.Controllers
{
    [Authorize(Roles = "Employee")]
    public class EmployeeController : BaseController
    {
        public async Task<ActionResult> Index()
        {
            SiteUser currentUser = await GetCurrentUserAsync();
            EmployeeIndexViewModel model = new EmployeeIndexViewModel();
            await model.InitializeAsync(currentUser, PizzaDb);

            return View(model);
        }

        public async Task<ActionResult> SearchUsers(int? page, int? rowsPerPage, string userId, string email)
        {
            throw new NotImplementedException();
            /*ManagePagedListViewModel<SearchUserViewModel> model = new ManagePagedListViewModel<SearchUserViewModel>();

            SiteUserFilter searchFilter = new SiteUserFilter()
            {
                Id = userId,
                Email = email
            };

            List<SiteUser> userList = await LoadPagedRecordsAsync<SiteUser>(page, rowsPerPage, "Id", SortOrder.Ascending, searchFilter, PizzaDb, Request, model.PaginationVm);

            foreach (SiteUser user in userList)
            {
                SearchUserViewModel userVm = new SearchUserViewModel()
                {
                    Id = user.Id,
                    Email = user.Email,
                    IsBanned = user.IsBanned
                };
                model.ItemViewModelList.Add(userVm);
            }

            return View(model);*/
        }
    }
}