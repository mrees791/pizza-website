﻿using DataLibrary.Models;
using DataLibrary.Models.QueryFilters;
using DataLibrary.Models.Sql;
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
        protected async Task<IEnumerable<TRecord>> LoadPagedRecordsAsync(int page, int rowsPerPage, string orderByColumn, SortOrder sortOrder,
            WhereClauseBase whereClauseBase, PizzaDatabase database, PaginationViewModel paginationVm)
        {
            List<TRecord> recordList = new List<TRecord>();
            int totalNumberOfItems = await database.GetNumberOfRecordsAsync<TRecord>(whereClauseBase);
            int totalPages = await database.GetNumberOfPagesAsync<TRecord>(rowsPerPage, whereClauseBase);
            recordList.AddRange(await database.GetPagedListAsync<TRecord>(page, rowsPerPage, orderByColumn, sortOrder, whereClauseBase));

            // Navigation pane
            paginationVm.QueryString = Request.QueryString;
            paginationVm.CurrentPage = page;
            paginationVm.RowsPerPage = rowsPerPage;
            paginationVm.TotalPages = totalPages;
            paginationVm.TotalNumberOfItems = totalNumberOfItems;

            return recordList;
        }

        protected void ValidatePageQuery(ref int? page, ref int? rowsPerPage, int defaultRowsPerPage)
        {
            if (!page.HasValue)
            {
                page = 1;
            }

            if (!rowsPerPage.HasValue)
            {
                rowsPerPage = defaultRowsPerPage;
            }

            if (page < 1)
            {
                page = 1;
            }

            if (rowsPerPage < 1)
            {
                rowsPerPage = defaultRowsPerPage;
            }
        }
    }
}