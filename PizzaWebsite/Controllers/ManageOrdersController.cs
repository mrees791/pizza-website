using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PizzaWebsite.Controllers.BaseControllers;

namespace PizzaWebsite.Controllers
{
    /// <summary>
    /// Used by employees for managing and updating customer orders.
    /// </summary>
    [Authorize(Roles = "Employee")]
    public class ManageOrdersController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}