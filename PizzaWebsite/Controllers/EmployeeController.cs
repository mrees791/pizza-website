﻿using DataLibrary.Models;
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
            Employee employee = await PizzaDb.GetEmployeeAsync(await GetCurrentUserAsync());

            EmployeeIndexViewModel viewModel = new EmployeeIndexViewModel()
            {
                EmployeeId = employee.Id
            };

            return View(viewModel);
        }
    }
}