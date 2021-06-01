﻿using DataLibrary.Models.Tables;
using PizzaWebsite.Models.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PizzaWebsite.Controllers
{
    [Authorize(Roles = "Admin,Executive,Employee")]
    public class EmployeeController : BaseController
    {
        public async Task<ActionResult> Index()
        {
            string userId = User.Identity.Name;
            EmployeeIndexViewModel model = new EmployeeIndexViewModel();

            return View();
        }
    }
}