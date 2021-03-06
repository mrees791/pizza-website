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
    public class ManagePizzaCrustFlavorMenuController : BaseManageMenuController<MenuPizzaCrustFlavor, ManageMenuPizzaCrustFlavorViewModel>
    {
        public async Task<ActionResult> Index(int? page, int? rowsPerPage, string name)
        {
            ValidatePageQuery(ref page, ref rowsPerPage, 10);
            MenuPizzaCrustFlavorFilter searchFilter = new MenuPizzaCrustFlavorFilter()
            {
                Name = name
            };
            return await Index(page.Value, rowsPerPage.Value, "Name", searchFilter);
        }

        public override async Task<ActionResult> Add()
        {
            MenuPizzaCrustFlavor crustFlavor = new MenuPizzaCrustFlavor()
            {
                AvailableForPurchase = true
            };
            ManageMenuPizzaCrustFlavorViewModel model = await RecordToViewModelAsync(crustFlavor);
            return View("Manage", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(ManageMenuPizzaCrustFlavorViewModel model)
        {
            return await Add(model, model.Name);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ManageMenuPizzaCrustFlavorViewModel model)
        {
            return await Edit(model, model.Name);
        }

        protected override async Task<ManageMenuPizzaCrustFlavorViewModel> RecordToViewModelAsync(MenuPizzaCrustFlavor record)
        {
            return await Task.FromResult(new ManageMenuPizzaCrustFlavorViewModel
            {
                Id = record.Id,
                AvailableForPurchase = record.AvailableForPurchase,
                Description = record.Description,
                HasMenuIcon = record.HasMenuIcon,
                HasPizzaBuilderImage = record.HasPizzaBuilderImage,
                Name = record.Name,
                MenuIconFile = record.MenuIconFile,
                PizzaBuilderImageFile = record.PizzaBuilderImageFile,
                SortOrder = record.SortOrder
            });
        }

        protected override MenuPizzaCrustFlavor ViewModelToRecord(ManageMenuPizzaCrustFlavorViewModel model)
        {
            return new MenuPizzaCrustFlavor()
            {
                Id = model.Id,
                AvailableForPurchase = model.AvailableForPurchase,
                Description = model.Description,
                HasMenuIcon = model.HasMenuIcon,
                HasPizzaBuilderImage = model.HasPizzaBuilderImage,
                Name = model.Name,
                MenuIconFile = model.MenuIconFile,
                PizzaBuilderImageFile = model.PizzaBuilderImageFile,
                SortOrder = model.SortOrder
            };
        }
    }
}