using DataLibrary.Models;
using DataLibrary.Models.Tables;
using Microsoft.AspNet.Identity.Owin;
using PizzaWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PizzaWebsite.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class ManageWebsiteController : BaseController
    {

        // GET: ManageWebsite
        public ActionResult Index()
        {
            return View();
        }


        /*

        public async Task<ActionResult> ManageEmployee(string id)
        {
            List<Employee> employeeRecords = await PizzaDb.GetListAsync<Employee>(new { Id = id });
            Employee employee = employeeRecords.FirstOrDefault();

            ManageEmployeeViewModel manageEmployeeVm = new ManageEmployeeViewModel();
            manageEmployeeVm.FromEntity(employee);
            manageEmployeeVm.IsManager = await UserManager.IsInRoleAsync(employee.UserId, "Manager");
            return View("ManageEmployee", manageEmployeeVm);
        }

        public async Task<ActionResult> ManageEmployees(int? page, int? rowsPerPage, string employeeId)
        {
            var manageEmployeesVm = new PagedListViewModel<ManageEmployeeViewModel, Employee>();

            object searchFilters = new
            {
                Id = employeeId
            };

            await manageEmployeesVm.LoadViewModelRecordsAsync(PizzaDb, Request, page, rowsPerPage, "Id", searchFilters);

            return View(manageEmployeesVm);
        }*/
    }
}