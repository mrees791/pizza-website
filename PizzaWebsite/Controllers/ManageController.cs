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
        private List<ToppingType> GetToppingTypes()
        {
            var toppingTypes = new List<ToppingType>();

            toppingTypes.Add(new ToppingType() { Name = "Meats" });
            toppingTypes.Add(new ToppingType() { Name = "Veggies" });

            return toppingTypes;
        }

        private List<IngredientType> GetIngredientTypes()
        {
            var ingredientTypes = new List<IngredientType>();

            ingredientTypes.Add(new IngredientType() { Name = "Cheese" });
            ingredientTypes.Add(new IngredientType() { Name = "Crust" });
            ingredientTypes.Add(new IngredientType() { Name = "Crust Flavor" });
            ingredientTypes.Add(new IngredientType() { Name = "Sauce" });
            ingredientTypes.Add(new IngredientType() { Name = "Topping" });

            return ingredientTypes;
        }

        /*private SelectList GetIngredientTypesSelectList()
        {
            var ingredientTypes = GetIngredientTypes();
            var selectListItems = new List<SelectListItem>();

            foreach (var ingredientType in ingredientTypes)
            {
                selectListItems.Add(new SelectListItem { Text = ingredientType.Name, Value = ingredientType.Name });
            }
            selectListItems[0].Selected = true;

            return new SelectList(selectListItems, "Value", "Text");
        }*/

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
            dynamic model = new ExpandoObject();
            model.IngredientTypes = GetIngredientTypes();

            // Create drop down list linked to model.IngredientTypes

            return View(model);
        }

        // Manage pizza ingredients
        public ActionResult ModifyCrust(Crust crust)
        {
            return View(crust);
        }
    }
}