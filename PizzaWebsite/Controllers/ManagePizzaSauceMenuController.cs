﻿using DataLibrary.Models.QueryFilters;
using DataLibrary.Models.Tables;
using PizzaWebsite.Controllers.BaseControllers;
using PizzaWebsite.Models;
using PizzaWebsite.Models.ManageMenus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PizzaWebsite.Controllers
{
    [Authorize(Roles = "Admin,Executive")]
    public class ManagePizzaSauceMenuController : BaseManageMenuController<MenuPizzaSauce, ManageMenuPizzaSauceViewModel>
    {
        public async Task<ActionResult> Index(int? page, int? rowsPerPage, string name)
        {
            ValidatePageQuery(ref page, ref rowsPerPage, 10);
            MenuPizzaSauceFilter searchFilter = new MenuPizzaSauceFilter()
            {
                Name = name
            };
            return await Index(page.Value, rowsPerPage.Value, "Name", searchFilter);
        }

        public override async Task<ActionResult> Add()
        {
            MenuPizzaSauce sauceFlavor = new MenuPizzaSauce()
            {
                AvailableForPurchase = true
            };
            ManageMenuPizzaSauceViewModel model = await RecordToViewModelAsync(sauceFlavor);
            return View("Manage", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(ManageMenuPizzaSauceViewModel model)
        {
            return await Add(model, model.Name);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ManageMenuPizzaSauceViewModel model)
        {
            return await Edit(model, model.Name);
        }

        protected override async Task<ManageMenuPizzaSauceViewModel> RecordToViewModelAsync(MenuPizzaSauce record)
        {
            return await Task.FromResult(new ManageMenuPizzaSauceViewModel
            {
                Id = record.Id,
                AvailableForPurchase = record.AvailableForPurchase,
                Description = record.Description,
                HasMenuIcon = record.HasMenuIcon,
                HasPizzaBuilderImage = record.HasPizzaBuilderImage,
                MenuIconFile = record.MenuIconFile,
                Name = record.Name,
                PizzaBuilderImageFile = record.PizzaBuilderImageFile,
                PriceLight = record.PriceLight,
                PriceRegular = record.PriceRegular,
                PriceExtra = record.PriceExtra,
                SortOrder = record.SortOrder
            });
        }

        protected override MenuPizzaSauce ViewModelToRecord(ManageMenuPizzaSauceViewModel model)
        {
            return new MenuPizzaSauce()
            {
                Id = model.Id,
                AvailableForPurchase = model.AvailableForPurchase,
                Description = model.Description,
                HasMenuIcon = model.HasMenuIcon,
                HasPizzaBuilderImage = model.HasPizzaBuilderImage,
                MenuIconFile = model.MenuIconFile,
                Name = model.Name,
                PizzaBuilderImageFile = model.PizzaBuilderImageFile,
                PriceLight = model.PriceLight,
                PriceRegular = model.PriceRegular,
                PriceExtra = model.PriceExtra,
                SortOrder = model.SortOrder
            };
        }
    }
}