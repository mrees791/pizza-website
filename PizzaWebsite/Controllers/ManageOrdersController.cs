using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PizzaWebsite.Controllers.BaseControllers;
using PizzaWebsite.Models.ManageOrders;
using PizzaWebsite.Models.ViewModelServices;

namespace PizzaWebsite.Controllers
{
    /// <summary>
    /// Used by employees for managing and updating customer orders.
    /// </summary>
    [Authorize(Roles = "Employee")]
    public class ManageOrdersController : BaseController
    {
        private readonly CustomerOrderServices _customerOrderServices;

        public ManageOrdersController()
        {
            _customerOrderServices = new CustomerOrderServices();
        }

        public ActionResult Index(int? page, int? rowsPerPage)
        {
            ValidatePageQuery(ref page, ref rowsPerPage, 10);

            // Load authorized store list.

            ManageOrdersIndexViewModel model = new ManageOrdersIndexViewModel()
            {

            };

            return View(model);
        }
    }
}