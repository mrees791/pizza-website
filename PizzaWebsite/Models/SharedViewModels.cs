using DataLibrary.Models.Tables;
using DataLibrary.Models.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        [Display(Name = "Crust")]
        public Dictionary<int, MenuPizzaCrust> CrustList { get; set; }
        public int SelectedCrustId { get; set; }
        public List<string> SizeList { get; set; }
        public string SelectedSize { get; set; }
        [Display(Name = "Sauce")]
        public Dictionary<int, MenuPizzaSauce> SauceList { get; set; }
        public int SelectedSauceId { get; set; }
        [Display(Name = "Amount")]
        public List<string> SauceAmountList { get; set; }
        public string SelectedSauceAmount { get; set; }
        [Display(Name = "Cheese")]
        public Dictionary<int, MenuPizzaCheese> CheeseList { get; set; }
        public int SelectedCheeseId { get; set; }
        [Display(Name = "Amount")]
        public List<string> CheeseAmountList { get; set; }
        public string SelectedCheeseAmount { get; set; }
        [Display(Name = "Crust Flavor")]
        public Dictionary<int, MenuPizzaCrustFlavor> CrustFlavorList { get; set; }
        public int SelectedCrustFlavorId { get; set; }
        public Dictionary<int, PizzaToppingViewModel> MeatToppingList { get; set; }
        public Dictionary<int, PizzaToppingViewModel> VeggieToppingList { get; set; }

        public bool ShowPizzaSizes { get; set; }

        public PizzaBuilderViewModel()
        {
        }
    }

    public class PizzaToppingViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public string FormName { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Amount")]
        public List<string> AmountList { get; set; }
        public string SelectedAmount { get; set; }
        public List<string> ToppingHalfList { get; set; }
        public string SelectedToppingHalf { get; set; }
    }
}