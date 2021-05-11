﻿using DataLibrary.Models.QueryFilters;
using DataLibrary.Models.Tables;
using DataLibrary.Models.Utility;
using PizzaWebsite.Models;
using PizzaWebsite.Models.ManageWebsite.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PizzaWebsite.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class ManagePizzaToppingTypeMenuController : BaseManageMenuController<MenuPizzaToppingType, ManageMenuPizzaToppingTypeViewModel>
    {
        public async Task<ActionResult> Index(int? page, int? rowsPerPage, string name)
        {
            MenuPizzaToppingTypeFilter searchFilter = new MenuPizzaToppingTypeFilter()
            {
                Name = name
            };

            return await Index(page, rowsPerPage, "Name", searchFilter);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(ManageMenuPizzaToppingTypeViewModel model)
        {
            return await Add(model, model.Name);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ManageMenuPizzaToppingTypeViewModel model)
        {
            return await Edit(model, model.Name);
        }

        protected override async Task<ManageMenuPizzaToppingTypeViewModel> RecordToViewModelAsync(MenuPizzaToppingType record)
        {
            return await Task.FromResult(new ManageMenuPizzaToppingTypeViewModel
            {
                Id = record.Id,
                AvailableForPurchase = record.AvailableForPurchase,
                CategoryName = record.CategoryName,
                Description = record.Description,
                HasMenuIcon = record.HasMenuIcon,
                HasPizzaBuilderImage = record.HasPizzaBuilderImage,
                MenuIconFile = record.MenuIconFile,
                Name = record.Name,
                PizzaBuilderImageFile = record.PizzaBuilderImageFile,
                SortOrder = record.SortOrder,
                PriceLight = record.PriceLight,
                PriceRegular = record.PriceRegular,
                PriceExtra = record.PriceExtra,
                ToppingCategoryList = ListUtility.GetToppingCategoryList()
            });
        }

        protected override MenuPizzaToppingType ViewModelToRecord(ManageMenuPizzaToppingTypeViewModel model)
        {
            return new MenuPizzaToppingType()
            {
                Id = model.Id,
                AvailableForPurchase = model.AvailableForPurchase,
                CategoryName = model.CategoryName,
                Description = model.Description,
                HasMenuIcon = model.HasMenuIcon,
                HasPizzaBuilderImage = model.HasPizzaBuilderImage,
                MenuIconFile = model.MenuIconFile,
                Name = model.Name,
                PizzaBuilderImageFile = model.PizzaBuilderImageFile,
                SortOrder = model.SortOrder,
                PriceLight = model.PriceLight,
                PriceRegular = model.PriceRegular,
                PriceExtra = model.PriceExtra
            };
        }
    }
}