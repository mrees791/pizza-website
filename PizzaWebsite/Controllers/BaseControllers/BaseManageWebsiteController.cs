using System.Collections.Generic;
using System.Threading.Tasks;
using DataLibrary.Models;
using DataLibrary.Models.Sql;
using DataLibrary.Models.Tables;
using PizzaWebsite.Models;
using PizzaWebsite.Models.Services;

namespace PizzaWebsite.Controllers.BaseControllers
{
    public abstract class BaseManageWebsiteController<TRecord> : BaseController
        where TRecord : Record
    {
        private MediaServices _mediaServices;

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

        protected MediaServices ImageServices
        {
            get => _mediaServices ?? new MediaServices();
            private set => _mediaServices = value;
        }
    }
}