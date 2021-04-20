﻿using DataLibrary.Models.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PizzaWebsite.Models
{
    public class CartPizzaBuilderViewModel : PizzaBuilderViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Quantity")]
        public int SelectedQuantity { get; set; }
        public List<int> QuantityList { get; set; }
        public List<string> SizeList { get; set; }
        [Required]
        [Display(Name = "Size")]
        public string SelectedSize { get; set; }
        [Display(Name = "Crust")]
        public Dictionary<int, string> CrustList { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "You must select a crust.")]
        public int SelectedCrustId { get; set; }

        public bool IsNewRecord()
        {
            return Id == 0;
        }
    }

    public class ManageMenuPizzaViewModel : PizzaBuilderViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int SortOrder { get; set; }
        [Required]
        [Display(Name = "Name")]
        [MaxLength(100)]
        public string Name { get; set; }
        [Display(Name = "Available for Purchase")]
        public bool AvailableForPurchase { get; set; }
        public List<string> CategoryList { get; set; }
        [Required]
        [Display(Name = "Category")]
        public string SelectedCategory { get; set; }
        [Required]
        [Display(Name = "Description")]
        [MaxLength(512)]
        public string Description { get; set; }

        public bool IsNewRecord()
        {
            return Id == 0;
        }
    }

    public class ManageMenuPizzaCheeseViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int SortOrder { get; set; }
        [Required]
        [Display(Name = "Name")]
        [MaxLength(100)]
        public string Name { get; set; }
        [Display(Name = "Available for Purchase")]
        public bool AvailableForPurchase { get; set; }
        [Display(Name = "Price (Light Amount)")]
        [Range(0.01, 100.0, ErrorMessage = "Price must be between $0.01 and $100.00.")]
        public decimal PriceLight { get; set; }
        [Display(Name = "Price (Regular Amount)")]
        [Range(0.01, 100.0, ErrorMessage = "Price must be between $0.01 and $100.00.")]
        public decimal PriceRegular { get; set; }
        [Display(Name = "Price (Extra Amount)")]
        [Range(0.01, 100.0, ErrorMessage = "Price must be between $0.01 and $100.00.")]
        public decimal PriceExtra { get; set; }
        [Required]
        [Display(Name = "Description")]
        [MaxLength(512)]
        public string Description { get; set; }
        public bool HasMenuIcon { get; set; }
        public string MenuIconFile { get; set; }
        public bool HasPizzaBuilderImage { get; set; }
        public string PizzaBuilderImageFile { get; set; }

        public bool IsNewRecord()
        {
            return Id == 0;
        }
    }

    public class ManageMenuPizzaCrustViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int SortOrder { get; set; }
        [Required]
        [Display(Name = "Name")]
        [MaxLength(100)]
        public string Name { get; set; }
        [Display(Name = "Available for Purchase")]
        public bool AvailableForPurchase { get; set; }
        [Display(Name = "Price (Small)")]
        [Range(0.01, 100.0, ErrorMessage = "Price must be between $0.01 and $100.00.")]
        public decimal PriceSmall { get; set; }
        [Display(Name = "Price (Medium)")]
        [Range(0.01, 100.0, ErrorMessage = "Price must be between $0.01 and $100.00.")]
        public decimal PriceMedium { get; set; }
        [Display(Name = "Price (Large)")]
        [Range(0.01, 100.0, ErrorMessage = "Price must be between $0.01 and $100.00.")]
        public decimal PriceLarge { get; set; }
        [Required]
        [Display(Name = "Description")]
        [MaxLength(512)]
        public string Description { get; set; }
        public bool HasMenuIcon { get; set; }
        public string MenuIconFile { get; set; }
        public bool HasPizzaBuilderImage { get; set; }
        public string PizzaBuilderImageFile { get; set; }

        public bool IsNewRecord()
        {
            return Id == 0;
        }
    }

    public class ManageMenuPizzaCrustFlavorViewModel
    {

        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int SortOrder { get; set; }
        [Required]
        [Display(Name = "Name")]
        [MaxLength(100)]
        public string Name { get; set; }
        [Display(Name = "Available for Purchase")]
        public bool AvailableForPurchase { get; set; }
        [Required]
        [Display(Name = "Description")]
        [MaxLength(512)]
        public string Description { get; set; }
        public bool HasMenuIcon { get; set; }
        public string MenuIconFile { get; set; }
        public bool HasPizzaBuilderImage { get; set; }
        public string PizzaBuilderImageFile { get; set; }

        public bool IsNewRecord()
        {
            return Id == 0;
        }
    }

    public class ManageMenuPizzaSauceViewModel
    {

        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int SortOrder { get; set; }
        [Required]
        [Display(Name = "Name")]
        [MaxLength(100)]
        public string Name { get; set; }
        [Display(Name = "Available for Purchase")]
        public bool AvailableForPurchase { get; set; }
        [Display(Name = "Price (Light Amount)")]
        [Range(0.01, 100.0, ErrorMessage = "Price must be between $0.01 and $100.00.")]
        public decimal PriceLight { get; set; }
        [Display(Name = "Price (Regular Amount)")]
        [Range(0.01, 100.0, ErrorMessage = "Price must be between $0.01 and $100.00.")]
        public decimal PriceRegular { get; set; }
        [Display(Name = "Price (Extra Amount)")]
        [Range(0.01, 100.0, ErrorMessage = "Price must be between $0.01 and $100.00.")]
        public decimal PriceExtra { get; set; }
        [Required]
        [Display(Name = "Description")]
        [MaxLength(512)]
        public string Description { get; set; }
        public bool HasMenuIcon { get; set; }
        public string MenuIconFile { get; set; }
        public bool HasPizzaBuilderImage { get; set; }
        public string PizzaBuilderImageFile { get; set; }

        public bool IsNewRecord()
        {
            return Id == 0;
        }
    }

    public class ManageMenuPizzaToppingTypeViewModel
    {
        public List<string> ToppingCategoryList { get; set; }

        public ManageMenuPizzaToppingTypeViewModel()
        {
            ToppingCategoryList = new List<string>();
        }

        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int SortOrder { get; set; }
        [Required]
        [Display(Name = "Name")]
        [MaxLength(100)]
        public string Name { get; set; }
        [Display(Name = "Available for Purchase")]
        public bool AvailableForPurchase { get; set; }
        [Display(Name = "Price (Light Amount)")]
        [Range(0.01, 100.0, ErrorMessage = "Price must be between $0.01 and $100.00.")]
        public decimal PriceLight { get; set; }
        [Display(Name = "Price (Regular Amount)")]
        [Range(0.01, 100.0, ErrorMessage = "Price must be between $0.01 and $100.00.")]
        public decimal PriceRegular { get; set; }
        [Display(Name = "Price (Extra Amount)")]
        [Range(0.01, 100.0, ErrorMessage = "Price must be between $0.01 and $100.00.")]
        public decimal PriceExtra { get; set; }
        [Required]
        [Display(Name = "Category")]
        public string CategoryName { get; set; }
        [Required]
        [Display(Name = "Description")]
        [MaxLength(512)]
        public string Description { get; set; }
        public bool HasMenuIcon { get; set; }
        public string MenuIconFile { get; set; }
        public bool HasPizzaBuilderImage { get; set; }
        public string PizzaBuilderImageFile { get; set; }

        public bool IsNewRecord()
        {
            return Id == 0;
        }
    }
}