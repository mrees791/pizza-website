using System;
using System.Web.Mvc;
using PizzaWebsite.Controllers.BaseControllers;
using PizzaWebsite.Models;

namespace PizzaWebsite.Controllers
{
    public class HomeController : BaseController
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


    }
}