using DataLibrary.Models.Tables;
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
    public class ManageEmployeesController : BaseController
    {
        public async Task<ActionResult> Index(int? page, int? rowsPerPage, string employeeId)
        {
            var manageEmployeesVm = new ManagePagedListViewModel<ManageEmployeeViewModel>();

            object searchFilters = new
            {
                Id = employeeId
            };

            List<Employee> employeeEntities = await LoadPagedEntitiesAsync<Employee>(PizzaDb, Request, manageEmployeesVm.PaginationVm, page, rowsPerPage, "Id", searchFilters);

            foreach (Employee employeeEntity in employeeEntities)
            {
                bool isManager = await UserManager.IsInRoleAsync(employeeEntity.UserId, "Manager");

                ManageEmployeeViewModel model = new ManageEmployeeViewModel()
                {
                    Id = employeeEntity.Id,
                    CurrentlyEmployed = employeeEntity.CurrentlyEmployed,
                    IsManager = isManager
                };
                manageEmployeesVm.ItemViewModelList.Add(model);
            }

            return View(manageEmployeesVm);
        }

        public async Task<ActionResult> ManageEmployee(string id)
        {
            Employee employeeEntity = await PizzaDb.GetAsync<Employee>(id);
            bool isManager = await UserManager.IsInRoleAsync(employeeEntity.UserId, "Manager");

            ManageEmployeeViewModel model = new ManageEmployeeViewModel()
            {
                Id = employeeEntity.Id,
                CurrentlyEmployed = employeeEntity.CurrentlyEmployed,
                IsManager = isManager
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ManageEmployee(ManageEmployeeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Employee employeeEntity = await PizzaDb.GetAsync<Employee>(model.Id);
            employeeEntity.CurrentlyEmployed = model.CurrentlyEmployed;
            PizzaDb.Update(employeeEntity);

            if (model.IsManager)
            {
                await UserManager.AddToRoleAsync(employeeEntity.UserId, "Manager");
            }
            else
            {
                await UserManager.RemoveFromRoleAsync(employeeEntity.UserId, "Manager");
            }

            ConfirmationViewModel confirmationModel = new ConfirmationViewModel();
            confirmationModel.ConfirmationMessage = $"Your changes to {model.Id} have been confirmed.";
            confirmationModel.ReturnUrlAction = $"{Url.Action("Index")}?{Request.QueryString}";

            return View("CreateEditConfirmation", confirmationModel);
        }
    }
}