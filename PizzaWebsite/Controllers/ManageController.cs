using PizzaWebsite.Models.Manage;
using PizzaWebsite.Models.Menu.Pizzas.Ingredients;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PizzaWebsite.Controllers
{
    public class ManageController : Controller
    {
        private IEnumerable<string> GetIngredientTypes()
        {
            /*return new List<string>()
            {
                "Cheese",
                "Crust",
                "Crust Flavor",
                "Sauce",
                "Topping"
            };*/
            return new List<string>()
            {
                "Crust"
            };
        }

        private IEnumerable<SelectListItem> GetSelectListItems(IEnumerable<string> elements)
        {
            var selectList = new List<SelectListItem>();

            foreach (var element in elements)
            {
                selectList.Add(new SelectListItem
                {
                    Value = element,
                    Text = element
                });
            }

            return selectList;
        }

        // GET: Manage
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ManageMenu()
        {
            return View();
        }

        public ActionResult ManagePizzaMenu()
        {
            return View();
        }

        public ActionResult ManagePizzaIngredients()
        {
            var ingredients = GetIngredientTypes();
            var model = new ManagePizzaIngredientsModel();
            model.Ingredients = GetSelectListItems(ingredients);

            return View(model);
        }

        [HttpPost]
        public ActionResult AddPizzaIngredient(ManagePizzaIngredientsModel model)
        {
            switch (model.SelectedIngredient)
            {
                case "Crust":
                    return View("ModifyCrust", new Crust { IsNewRecord = true });
            }
            throw new Exception($"ActionResult needed for {model.SelectedIngredient}.");
        }

        public ActionResult ModifyCrust(Crust crust)
        {
            return View("ModifyCrust", crust);
        }

        [HttpPost]
        public ActionResult AddCrustRecord(Crust crust)
        {
            if (ModelState.IsValid)
            {
                // Add crust record to database.

                RedirectToAction("ManagePizzaIngredients");
            }
            return View("ModifyCrust", crust);
        }

        [HttpPost]
        public ActionResult ModifyCrustRecord(Crust crust)
        {
            if (ModelState.IsValid)
            {
                RedirectToAction("AddCrustRecord", crust);
            }
            return View("ModifyCrust", crust);
        }
    }
}