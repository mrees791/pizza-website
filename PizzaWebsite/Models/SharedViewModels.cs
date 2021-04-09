﻿using DataLibrary.Models;
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
        public PizzaBuilderViewModel()
        {
            SauceList = new Dictionary<int, string>();
            CheeseList = new Dictionary<int, string>();
            CrustFlavorList = new Dictionary<int, string>();
            ToppingList = new List<PizzaToppingViewModel>();
        }
        [Display(Name = "Sauce")]
        public Dictionary<int, string> SauceList { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "You must select a sauce.")]
        public int SelectedSauceId { get; set; }
        [Display(Name = "Amount")]
        public List<string> SauceAmountList { get; set; }
        public string SelectedSauceAmount { get; set; }
        [Display(Name = "Cheese")]
        public Dictionary<int, string> CheeseList { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "You must select a cheese.")]
        public int SelectedCheeseId { get; set; }
        [Display(Name = "Amount")]
        public List<string> CheeseAmountList { get; set; }
        public string SelectedCheeseAmount { get; set; }
        [Display(Name = "Crust Flavor")]
        public Dictionary<int, string> CrustFlavorList { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "You must select a crust flavor.")]
        public int SelectedCrustFlavorId { get; set; }
        public List<PizzaToppingViewModel> ToppingList { get; set; }
    }

    public class PizzaToppingViewModel
    {
        public int ListIndex { get; set; }
        public int Id { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public List<string> AmountList { get; set; }
        public string SelectedAmount { get; set; }
        public List<string> ToppingHalfList { get; set; }
        public string SelectedToppingHalf { get; set; }
    }

    public class CartItemViewModel
    {
        public int CartItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public int Quantity { get; set; }
        public ProductCategory ProductCategory { get; set; }
    }

    public class CartViewModel
    {
        public List<CartItemViewModel> CartItemList { get; set; }

        public CartViewModel()
        {
            CartItemList = new List<CartItemViewModel>();
        }

        public bool IsEmpty()
        {
            return !CartItemList.Any();
        }
    }
}