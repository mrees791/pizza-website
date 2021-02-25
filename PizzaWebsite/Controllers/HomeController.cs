using Microsoft.AspNet.Identity;
using PizzaWebsite.Models.Identity;
using PizzaWebsite.Models.Identity.Validators;
using PizzaWebsite.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using System.Security.Claims;
using PizzaWebsite.Models.Tests;
using System.Net.Mail;
using System.Net;
using System.Diagnostics;
using System.Configuration;
using Microsoft.AspNet.Identity.Owin;
using PizzaWebsite.App_Start;

namespace PizzaWebsite.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private DummyDatabase dbContext;

        public HomeController()
        {
            dbContext = new DummyDatabase();
        }

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
    }
}