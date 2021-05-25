using DataLibrary.Models;
using DataLibrary.Models.QueryFilters;
using PizzaWebsite.Models;
using PizzaWebsite.Models.ManageWebsite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PizzaWebsite.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public abstract class BaseManageMenuController<TRecord, TViewModel> : BaseController where TRecord : Record, new() where TViewModel : class, new()
    {
        protected async Task<ActionResult> Index(int? page, int? rowsPerPage, string orderByColumn, QueryFilterBase searchFilter)
        {
            var viewModelList = new ManagePagedListViewModel<TViewModel>();

            List<TRecord> recordList = await LoadPagedRecordsAsync<TRecord>(page, rowsPerPage, orderByColumn, SortOrder.Ascending, searchFilter, PizzaDb, Request, viewModelList.PaginationVm);

            foreach (TRecord record in recordList)
            {
                viewModelList.ItemViewModelList.Add(await RecordToViewModelAsync(record));
            }

            return View(viewModelList);
        }

        public virtual async Task<ActionResult> Add()
        {
            return View("Manage", await RecordToViewModelAsync(new TRecord()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        protected async Task<ActionResult> Add(TViewModel model, string modelName)
        {
            if (!ModelState.IsValid)
            {
                return View("Manage", model);
            }

            await PizzaDb.InsertAsync(ViewModelToRecord(model));

            ConfirmationViewModel confirmationModel = new ConfirmationViewModel();
            confirmationModel.ConfirmationMessage = $"{modelName} has been added to the database.";
            confirmationModel.ReturnUrlAction = $"{Url.Action("Index")}?{Request.QueryString}";

            return View("CreateEditConfirmation", confirmationModel);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            TRecord record = await PizzaDb.GetAsync<TRecord>(id.Value);
            TViewModel model = await RecordToViewModelAsync(record);

            return View("Manage", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        protected async Task<ActionResult> Edit(TViewModel model, string modelName)
        {
            if (!ModelState.IsValid)
            {
                return View("Manage", model);
            }

            await PizzaDb.UpdateAsync(ViewModelToRecord(model));

            ConfirmationViewModel confirmationModel = new ConfirmationViewModel();
            confirmationModel.ConfirmationMessage = $"Your changes to {modelName} have been confirmed.";
            confirmationModel.ReturnUrlAction = $"{Url.Action("Index")}?{Request.QueryString}";

            return View("CreateEditConfirmation", confirmationModel);
        }

        protected abstract Task<TViewModel> RecordToViewModelAsync(TRecord record);
        protected abstract TRecord ViewModelToRecord(TViewModel model);
    }
}