using DataLibrary.Models;
using DataLibrary.Models.Sql;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PizzaWebsite.Models
{
    public class PaginationViewModel
    {
        public int TotalPages { get; set; }
        public int RowsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalNumberOfItems { get; set; }
        public NameValueCollection QueryString { get; set; }

        public async Task InitializeAsync<TRecord>(int pageNumber, int rowsPerPage, WhereClauseBase whereClauseBase, HttpRequestBase request, PizzaDatabase pizzaDb) where TRecord : Record
        {
            int totalNumberOfItems = await pizzaDb.GetNumberOfRecordsAsync<TRecord>(whereClauseBase);
            int numberOfPages = await pizzaDb.GetNumberOfPagesAsync<TRecord>(rowsPerPage, whereClauseBase);

            CurrentPage = pageNumber;
            RowsPerPage = rowsPerPage;
            TotalPages = numberOfPages;
            TotalNumberOfItems = totalNumberOfItems;
            QueryString = request.QueryString;
        }
    }
}