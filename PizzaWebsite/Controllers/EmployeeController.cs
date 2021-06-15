using System.Threading.Tasks;
using System.Web.Mvc;
using DataLibrary.Models.Tables;
using PizzaWebsite.Controllers.BaseControllers;
using PizzaWebsite.Models.Employees;

namespace PizzaWebsite.Controllers
{
    [Authorize(Roles = "Employee")]
    public class EmployeeController : BaseController
    {
        public async Task<ActionResult> Index()
        {
            SiteUser user = await GetCurrentUserAsync();
            Employee employee = await PizzaDb.GetEmployeeAsync(user);
            bool isManager = await UserManager.IsInRoleAsync(user.Id, "Manager");
            bool isExecutive = await UserManager.IsInRoleAsync(user.Id, "Executive");
            bool isAdmin = await UserManager.IsInRoleAsync(user.Id, "Admin");
            EmployeeIndexViewModel model = new EmployeeIndexViewModel
            {
                EmployeeId = employee.Id,
                AuthorizedToManageMenu = isExecutive || isAdmin,
                AuthorizedToManageEmployees = isManager || isExecutive || isAdmin,
                AuthorizedToManageStores = isManager || isExecutive || isAdmin,
                AuthorizedToManageUsers = isManager || isExecutive || isAdmin
            };
            return View(model);
        }
    }
}