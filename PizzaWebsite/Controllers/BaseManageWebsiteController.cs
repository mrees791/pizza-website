using DataLibrary.Models;
using DataLibrary.Models.QueryFilters;
using PizzaWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PizzaWebsite.Controllers
{
    public abstract class BaseManageWebsiteController<TRecord> : BaseController
        where TRecord : Record
    {
        protected async Task<List<TRecord>> LoadPagedRecordsAsync(int? page, int? rowsPerPage, string orderByColumn, SortOrder sortOrder,
            QueryBase queryBase, PizzaDatabase database, HttpRequestBase request, PaginationViewModel paginationVm)
        {
            // Set default values
            if (!page.HasValue)
            {
                page = 1;
            }
            if (!rowsPerPage.HasValue)
            {
                rowsPerPage = 10;
            }

            List<TRecord> recordList = new List<TRecord>();
            int totalNumberOfItems = await database.GetNumberOfRecordsAsync<TRecord>(queryBase);
            int totalPages = await database.GetNumberOfPagesAsync<TRecord>(rowsPerPage.Value, queryBase);
            recordList.AddRange(await database.GetPagedListAsync<TRecord>(page.Value, rowsPerPage.Value, orderByColumn, sortOrder, queryBase));

            // Navigation pane
            paginationVm.QueryString = request.QueryString;
            paginationVm.CurrentPage = page.Value;
            paginationVm.RowsPerPage = rowsPerPage.Value;
            paginationVm.TotalPages = totalPages;
            paginationVm.TotalNumberOfItems = totalNumberOfItems;

            return recordList;
        }
    }
}