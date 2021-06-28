using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using DataLibrary.Models;
using DataLibrary.Models.Sql;
using PizzaWebsite.Models;
using PizzaWebsite.Models.Employees;
using PizzaWebsite.Models.ManageMenuImages;

namespace PizzaWebsite.Controllers.BaseControllers
{
    public abstract class BaseManageMenuController<TRecord, TViewModel> : BaseManageWebsiteController<TRecord>
        where TRecord : MenuCategoryRecord, new()
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
            int id = await PizzaDb.InsertAsync(ViewModelToRecord(model));
            if (id == 0)
            {
                ModelState.AddModelError("", "Unable to insert record.");
                return View("Manage", model);
            }
            ConfirmationViewModel confirmationModel = new ConfirmationViewModel
            {
                ConfirmationMessage = $"{modelName} has been added to the database.",
                ReturnUrlAction = $"{Url.Action("Index")}?{Request.QueryString}"
            };
            return View("CreateEditConfirmation", confirmationModel);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (!id.HasValue)
            {
                return MissingIdErrorMessage();
            }
            TRecord record = await PizzaDb.GetAsync<TRecord>(id.Value);
            if (record == null)
            {
                return InvalidIdErrorMessage(id.Value);
            }
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
            int rowsAffected = await PizzaDb.UpdateAsync(ViewModelToRecord(model));
            if (rowsAffected == 0)
            {
                ModelState.AddModelError("", "Unable to update record.");
                return View("Manage", model);
            }
            ConfirmationViewModel confirmationModel = new ConfirmationViewModel
            {
                ConfirmationMessage = $"Your changes to {modelName} have been confirmed.",
                ReturnUrlAction = $"{Url.Action("Index")}?{Request.QueryString}"
            };
            return View("CreateEditConfirmation", confirmationModel);
        }

        [HttpPost]
        public async Task<ActionResult> UploadMenuIconAjax(int id)
        {
            Response.StatusCode = (int)HttpStatusCode.OK;

            // todo: Finish
            // Make sure file exists.
            // Validate image file dimensions.

            HttpPostedFileBase file = Request.Files[0];

            // Make sure record exists.
            TRecord record = await PizzaDb.GetAsync<TRecord>(id);

            if (record == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json($"Menu item with ID {id} does not exist.",
                    MediaTypeNames.Text.Plain);
            }

            string filePath = Server.MapPath(DirectoryServices.GetMenuImageUrl(id, record.GetMenuCategoryType(), MenuImageType.MenuIcon));

            try
            {
                file.SaveAs(filePath);
            }
            catch (NotImplementedException e)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json($"NotImplementedException occured for file.",
                    MediaTypeNames.Text.Plain);
            }

            string message = "File uploaded successfully.";
            return Json(message);
        }

        protected ActionResult MissingIdErrorMessage()
        {
            ErrorMessageViewModel model = new ErrorMessageViewModel
            {
                Header = "Error",
                ErrorMessage = "Missing ID.",
                ReturnUrlAction = $"{Url.Action("Index")}?{Request.QueryString}",
                ShowReturnLink = true
            };
            return View("ErrorMessage", model);
        }

        protected ActionResult InvalidIdErrorMessage(int id)
        {
            ErrorMessageViewModel model = new ErrorMessageViewModel
            {
                Header = "Error",
                ErrorMessage = $"Record with ID {id} could not be found.",
                ReturnUrlAction = $"{Url.Action("Index")}?{Request.QueryString}",
                ShowReturnLink = true
            };
            return View("ErrorMessage", model);
        }
        
        protected abstract Task<TViewModel> RecordToViewModelAsync(TRecord record);
        protected abstract TRecord ViewModelToRecord(TViewModel model);
    }
}