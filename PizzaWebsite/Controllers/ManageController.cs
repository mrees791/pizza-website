﻿using PizzaWebsite.Models.Menu.Pizzas.Ingredients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PizzaWebsite.Controllers
{
    public class ManageController : Controller
    {
        // GET: Manage
        public ActionResult Index()
        {
            return View();
        }

        // Manage pizza ingredients
        public ActionResult ModifyCrust(Crust crust)
        {
        }
    }
}