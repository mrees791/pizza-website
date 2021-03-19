using DataLibrary.Models.Tables;
using DataLibrary.Models.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
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
    }

    public abstract class PagedListViewModel
    {
        public PaginationViewModel PaginationVm { get; set; }

        public PagedListViewModel()
        {
            PaginationVm = new PaginationViewModel();
        }
    }

    public class PizzaBuilderViewModel
    {
        public List<string> PizzaSizeList { get; set; }
        public List<MenuPizzaCrust> PizzaCrustList { get; set; }

        public int SelectedPizzaCrustId { get; set; }

        public PizzaBuilderViewModel()
        {
            PizzaSizeList = new List<string>();
            PizzaCrustList = new List<MenuPizzaCrust>();
        }
    }
}