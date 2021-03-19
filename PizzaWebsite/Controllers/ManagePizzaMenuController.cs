using DataLibrary.Models.Tables;
using PizzaWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PizzaWebsite.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class ManagePizzaMenuController : BaseManageMenuController<MenuPizza, ManageMenuPizzaViewModel>
    {
        public async Task<ActionResult> Index(int? page, int? rowsPerPage, string name)
        {
            object searchFilters = new
            {
                Name = name
            };

            return await Index(page, rowsPerPage, searchFilters, "PizzaName");
        }

        public override ActionResult Add()
        {
            ManageMenuPizzaViewModel model = new ManageMenuPizzaViewModel();
            PizzaBuilderUtility.LoadPizzaBuilderLists(PizzaDb, model);
            return View("Manage", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ManageMenuPizzaViewModel model)
        {
            PizzaBuilderUtility.LoadPizzaBuilderLists(PizzaDb, model);
            return Add(model, model.Name);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ManageMenuPizzaViewModel model)
        {
            return Edit(model, model.Name);
        }

        protected override ManageMenuPizzaViewModel EntityToViewModel(MenuPizza entity)
        {
            throw new NotImplementedException();
            /*ManageMenuPizzaViewModel model = new ManageMenuPizzaViewModel();
            PizzaBuilderUtility.LoadPizzaBuilderLists(PizzaDb, model);

            return model;*/
        }

        protected override MenuPizza ViewModelToEntity(ManageMenuPizzaViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}