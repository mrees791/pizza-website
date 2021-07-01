﻿using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DataLibrary.Models;
using DataLibrary.Models.Services;
using DataLibrary.Models.Tables;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PizzaWebsite.Models.Geography;
using PizzaWebsite.Models.Services;

namespace PizzaWebsite.Controllers.BaseControllers
{
    public abstract class BaseController : Controller
    {
        private MediaServices _mediaServices;
        private GeographyServices _geographyServices;
        private ListServices _listServices;
        private DirectoryServices _directoryServices;
        private PizzaDatabase _pizzaDb;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ListServices ListServices
        {
            get => _listServices ?? new ListServices();
            private set => _listServices = value;
        }

        public DirectoryServices DirectoryServices
        {
            get => _directoryServices ?? new DirectoryServices();
            private set => _directoryServices = value;
        }

        public PizzaDatabase PizzaDb
        {
            get => _pizzaDb ?? HttpContext.GetOwinContext().Get<PizzaDatabase>();
            private set => _pizzaDb = value;
        }

        public ApplicationSignInManager SignInManager
        {
            get => _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            private set => _signInManager = value;
        }

        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }

        public GeographyServices GeographyServices
        {
            get => _geographyServices ?? new GeographyServices();
            private set => _geographyServices = value;
        }

        protected MediaServices MediaServices
        {
            get => _mediaServices ?? new MediaServices();
            private set => _mediaServices = value;
        }

        protected async Task<SiteUser> GetCurrentUserAsync()
        {
            return await PizzaDb.GetSiteUserByIdAsync(User.Identity.GetUserId());
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

        [HttpPost]
        protected HttpPostedFileBase GetPostedImageFile()
        {
            HttpFileCollectionBase files = Request.Files;
            if (files.Count == 0)
            {
                throw new Exception("No files found in request.");
            }
            if (files.Count > 1)
            {
                throw new Exception("There can only be one file in the request.");
            }
            HttpPostedFileBase file = files[0];
            if (file == null)
            {
                throw new Exception("files[0] is null.");
            }
            return file;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}