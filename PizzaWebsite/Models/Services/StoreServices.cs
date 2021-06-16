using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using DataLibrary.Models;
using DataLibrary.Models.JoinLists;
using DataLibrary.Models.QueryFilters;
using DataLibrary.Models.Tables;

namespace PizzaWebsite.Models.Services
{
    /// <summary>
    /// Provides utility methods for accessing store location records.
    /// </summary>
    public class StoreServices
    {
        /// <summary>
        /// Only select stores that the current user is authorized to see.
        /// If they are an admin or an executive, it will load all stores.
        /// Otherwise, it will only load stores the current user is employed at.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rowsPerPage"></param>
        /// <param name="userRoleList"></param>
        /// <param name="searchFilter"></param>
        /// <param name="paginationVm"></param>
        /// <param name="employee"></param>
        /// <param name="request"></param>
        /// <param name="pizzaDb"></param>
        /// <returns></returns>
        public async Task<IEnumerable<StoreLocation>> LoadAuthorizedStoreLocationListAsync(int page, int rowsPerPage, IList<string> userRoleList,
            StoreLocationFilter searchFilter, PaginationViewModel paginationVm, Employee employee, HttpRequestBase request, PizzaDatabase pizzaDb)
        {
            int totalPages = 0;
            int totalNumberOfItems = 0;
            bool isAdmin = userRoleList.Contains("Admin");
            bool isExecutive = userRoleList.Contains("Executive");
            bool authorizedToSeeAllStores = isAdmin || isExecutive;
            IEnumerable<StoreLocation> storeList = null;

            if (authorizedToSeeAllStores)
            {
                storeList = await pizzaDb.GetPagedListAsync<StoreLocation>(page, rowsPerPage, "Name", SortOrder.Ascending, searchFilter);
                totalNumberOfItems = await pizzaDb.GetNumberOfRecordsAsync<StoreLocation>(searchFilter);
                totalPages = await pizzaDb.GetNumberOfPagesAsync<StoreLocation>(rowsPerPage, searchFilter);
            }
            else
            {
                // Uses a join to select only stores that the current user is employed at.
                EmployeeLocationOnStoreLocationJoinList joinList = new EmployeeLocationOnStoreLocationJoinList();
                await joinList.LoadPagedListByEmployeeIdAsync(employee.Id, searchFilter, page, rowsPerPage, pizzaDb);
                storeList = joinList.Items.Select(j => j.Table2);
                totalNumberOfItems =
                    await joinList.GetNumberOfResultsByEmployeeIdAsync(employee.Id, searchFilter, rowsPerPage,
                        pizzaDb);
                totalPages =
                    await joinList.GetNumberOfPagesByEmployeeIdAsync(employee.Id, searchFilter, rowsPerPage,
                        pizzaDb);
            }

            paginationVm.QueryString = request.QueryString;
            paginationVm.CurrentPage = page;
            paginationVm.RowsPerPage = rowsPerPage;
            paginationVm.TotalPages = totalPages;
            paginationVm.TotalNumberOfItems = totalNumberOfItems;
            return storeList;
        }
    }
}