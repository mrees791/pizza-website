using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PizzaWebsite.Controllers
{
    [Authorize(Roles = "Admin,Executive")]
    public class ManageMenuController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}