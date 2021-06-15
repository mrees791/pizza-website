using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using DataLibrary.Models;
using DataLibrary.Models.Sql;
using PizzaWebsite.Models;
using PizzaWebsite.Models.Employees;

namespace PizzaWebsite.Controllers.BaseControllers
{
    public abstract class BaseManageMenuController<TRecord, TViewModel> : BaseManageWebsiteController<TRecord>
        where TRecord : Record, new()
        where TViewModel : class, new()
    {
        protected async Task<ActionResult> Index(int page, int rowsPerPage, string orderByColumn,
            WhereClauseBase whereClauseBase)
        {
            PaginationViewModel paginationVm = new PaginationViewModel();
            IEnumerable<TRecord> recordList = await LoadPagedRecordsAsync(page, rowsPerPage, orderByColumn,
                SortOrder.Ascending, whereClauseBase, PizzaDb, paginationVm);
            List<TViewModel> itemViewModelList = new List<TViewModel>();
            foreach (TRecord record in recordList)
            {
                itemViewModelList.Add(await RecordToViewModelAsync(record));
            }

            ManagePagedListViewModel<TViewModel> model = new ManagePagedListViewModel<TViewModel>
            {
                ItemViewModelList = itemViewModelList,
                PaginationVm = paginationVm
            };
            return View(model);
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
            ConfirmationViewModel confirmationModel = new ConfirmationViewModel
            {
                ConfirmationMessage = $"{modelName} has been added to the database.",
                ReturnUrlAction = $"{Url.Action("Index")}?{Request.QueryString}"
            };
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
            ConfirmationViewModel confirmationModel = new ConfirmationViewModel
            {
                ConfirmationMessage = $"Your changes to {modelName} have been confirmed.",
                ReturnUrlAction = $"{Url.Action("Index")}?{Request.QueryString}"
            };
            return View("CreateEditConfirmation", confirmationModel);
        }

        protected abstract Task<TViewModel> RecordToViewModelAsync(TRecord record);
        protected abstract TRecord ViewModelToRecord(TViewModel model);
    }
}