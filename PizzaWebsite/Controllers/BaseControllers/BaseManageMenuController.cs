using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
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
        protected MenuImageValidation MenuIconValidation;
        protected MenuImageValidation PizzaBuilderImageValidation;

        protected BaseManageMenuController()
        {
            MenuIconValidation = new MenuImageValidation();
            PizzaBuilderImageValidation = new MenuImageValidation();
        }

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
            return await UploadMenuImageAjax(id, MenuIconValidation, MenuImageType.MenuIcon);
        }

        [HttpPost]
        public async Task<ActionResult> UploadPizzaBuilderImageAjax(int id)
        {
            return await UploadMenuImageAjax(id, PizzaBuilderImageValidation, MenuImageType.PizzaBuilderImage);
        }

        // todo: Finish
        [HttpPost]
        protected async Task<ActionResult> UploadMenuImageAjax(int id, MenuImageValidation validation, MenuImageType imageType)
        {
            Response.StatusCode = (int)HttpStatusCode.OK;
            if (Request.Files.Count == 0)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json($"No files were in the request object.",
                    MediaTypeNames.Text.Plain);
            }
            TRecord record = await PizzaDb.GetAsync<TRecord>(id);
            if (record == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json($"Menu item with ID {id} does not exist.",
                    MediaTypeNames.Text.Plain);
            }
            HttpPostedFileBase file = Request.Files[0];
            if (file == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json($"Request file at index 0 is null.",
                    MediaTypeNames.Text.Plain);
            }
            // Validate image file.
            if (file.ContentLength > 1000000)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json($"File size cannot exceed 1 megabyte.",
                    MediaTypeNames.Text.Plain);
            }
            if (file.ContentType != "image/webp")
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json($"Mime type must be image/webp.",
                    MediaTypeNames.Text.Plain);
            }
            try
            {
                // Validate image file dimensions.
                using (Bitmap bmp = MediaServices.DecodeWebp(file.InputStream))
                {
                    bool validImageDimensions = true;
                    string imageDimensionsErrorMessage = string.Empty;
                    if (bmp.Width != validation.RequiredWidth)
                    {
                        validImageDimensions = false;
                        imageDimensionsErrorMessage += $"Image width must be {validation.RequiredWidth} px. ";
                    }
                    if (bmp.Height != validation.RequiredHeight)
                    {
                        validImageDimensions = false;
                        imageDimensionsErrorMessage += $"Image height must be {validation.RequiredHeight} px. ";
                    }
                    if (!validImageDimensions)
                    {
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        return Json(imageDimensionsErrorMessage,
                            MediaTypeNames.Text.Plain);
                    }
                }
            }
            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json($"An error occured while attempting to decode the webp file.",
                    MediaTypeNames.Text.Plain);
            }

            try
            {
                string filePath =
                    Server.MapPath(DirectoryServices.GetMenuImageUrl(id, record.GetMenuCategoryType(), imageType));
                file.SaveAs(filePath);
            }
            catch (NotImplementedException e)
            {
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return Json($"NotImplementedException occured for webp file. Could not save file.",
                    MediaTypeNames.Text.Plain);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json($"Exception occured for webp file. Could not save file.",
                    MediaTypeNames.Text.Plain);
            }
            return Json("File uploaded successfully.");
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