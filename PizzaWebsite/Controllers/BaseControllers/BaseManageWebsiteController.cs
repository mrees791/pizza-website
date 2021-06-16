using System.Collections.Generic;
using System.Threading.Tasks;
using DataLibrary.Models;
using DataLibrary.Models.Sql;
using DataLibrary.Models.Tables;
using PizzaWebsite.Models;

namespace PizzaWebsite.Controllers.BaseControllers
{
    public abstract class BaseManageWebsiteController<TRecord> : BaseController
        where TRecord : Record
    {
        protected async Task<IEnumerable<TRecord>> LoadPagedRecordsAsync(int page, int rowsPerPage,
            string orderByColumn, SortOrder sortOrder,
            WhereClauseBase whereClauseBase, PizzaDatabase pizzaDb, PaginationViewModel paginationVm)
        {
            IEnumerable<TRecord> recordList =
                await pizzaDb.GetPagedListAsync<TRecord>(page, rowsPerPage, orderByColumn, sortOrder, whereClauseBase);
            int totalNumberOfItems = await pizzaDb.GetNumberOfRecordsAsync<TRecord>(whereClauseBase);
            int totalPages = await pizzaDb.GetNumberOfPagesAsync<TRecord>(rowsPerPage, whereClauseBase);

            // Navigation pane
            paginationVm.QueryString = Request.QueryString;
            paginationVm.CurrentPage = page;
            paginationVm.RowsPerPage = rowsPerPage;
            paginationVm.TotalPages = totalPages;
            paginationVm.TotalNumberOfItems = totalNumberOfItems;

            return recordList;
        }

        protected async Task<Employee> GetCurrentEmployeeAsync()
        {
            return await PizzaDb.GetEmployeeAsync(await GetCurrentUserAsync());
        }
    }
}