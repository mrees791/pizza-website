using System;
using System.Web.Mvc;
using PizzaWebsite.Models;

namespace PizzaWebsite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        // todo: Remove after pizza builder is complete.
        public ActionResult PizzaBuilderTest()
        {
            return View();
        }

        // todo: Remove
        public ActionResult StyleTest()
        {
            return View();
        }
    }
}