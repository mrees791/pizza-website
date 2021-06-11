using DataLibrary.Models;
using DataLibrary.Models.QueryFilters;
using DataLibrary.Models.Tables;
using PizzaWebsite.Controllers.BaseControllers;
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
    }
}