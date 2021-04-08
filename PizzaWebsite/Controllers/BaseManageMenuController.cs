using DataLibrary.Models.Interfaces;
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
    public abstract class BaseManageMenuController<TEntity, TViewModel> : BaseController where TEntity : class, ITable, new() where TViewModel : class, new()
    {
        protected async Task<ActionResult> Index(int? page, int? rowsPerPage, object searchFilters, string sortColumn)
        {
            var viewModelList = new ManagePagedListViewModel<TViewModel>();

            List<TEntity> entityList = await LoadPagedEntitiesAsync<TEntity>(PizzaDb, Request, viewModelList.PaginationVm, page, rowsPerPage, sortColumn, searchFilters);

            foreach (TEntity entity in entityList)
            {
                viewModelList.ItemViewModelList.Add(EntityToViewModel(entity));
            }

            return View(viewModelList);
        }

        public virtual ActionResult Add()
        {
            return View("Manage", EntityToViewModel(new TEntity()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        protected ActionResult Add(TViewModel model, string modelName)
        {
            if (!ModelState.IsValid)
            {
                return View("Manage", model);
            }

            PizzaDb.Insert(ViewModelToEntity(model));

            ConfirmationViewModel confirmationModel = new ConfirmationViewModel();
            confirmationModel.ConfirmationMessage = $"{modelName} has been added to the database.";
            confirmationModel.ReturnUrlAction = $"{Url.Action("Index")}?{Request.QueryString}";

            return View("CreateEditConfirmation", confirmationModel);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            TEntity entity = await PizzaDb.GetAsync<TEntity>(id.Value);
            TViewModel model = EntityToViewModel(entity);

            return View("Manage", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        protected ActionResult Edit(TViewModel model, string modelName)
        {
            if (!ModelState.IsValid)
            {
                return View("Manage", model);
            }

            PizzaDb.Update(ViewModelToEntity(model));

            ConfirmationViewModel confirmationModel = new ConfirmationViewModel();
            confirmationModel.ConfirmationMessage = $"Your changes to {modelName} have been confirmed.";
            confirmationModel.ReturnUrlAction = $"{Url.Action("Index")}?{Request.QueryString}";

            return View("CreateEditConfirmation", confirmationModel);
        }

        protected abstract TViewModel EntityToViewModel(TEntity entity);
        protected abstract TEntity ViewModelToEntity(TViewModel model);
    }
}