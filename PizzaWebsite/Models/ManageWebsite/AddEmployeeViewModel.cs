using DataLibrary.Models;
using DataLibrary.Models.Tables;
using PizzaWebsite.Models.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PizzaWebsite.Models.ManageWebsite
{
    public class AddEmployeeViewModel
    {
        [Display(Name = "Employee ID")]
        [StringLength(256, ErrorMessage = "Employee ID cannot be longer than 256 characters.")]
        [ValidEmployeeId]
        [Required]
        public string Id { get; set; }

        [Display(Name = "User Id")]
        [Required]
        public string UserId { get; set; }

        [Display(Name = "Is Manager")]
        public bool IsManager { get; set; }

        /// <summary>
        /// Validates the view model properties and returns a list of errors found (key, error message.)
        /// </summary>
        /// <returns></returns>
        public async Task ValidateAsync(ModelStateDictionary modelState, PizzaDatabase pizzaDb)
        {
            // Check if user exists
            SiteUser siteUser = await pizzaDb.GetSiteUserByNameAsync(UserId);

            if (siteUser == null)
            {
                modelState.AddModelError(nameof(UserId), "User does not exist.");
            }

            // Make sure employee ID isn't already taken
            Employee employee = await pizzaDb.GetAsync<Employee>(Id);

            if (employee != null)
            {
                modelState.AddModelError(nameof(Id), "Employee ID is already taken.");
            }


            // Make sure user isn't already employed
            if (siteUser != null)
            {
                SiteRole employeeRole = await pizzaDb.GetSiteRoleByNameAsync("Employee");
                bool alreadyEmployed = await pizzaDb.UserIsInRole(siteUser, employeeRole);

                if (alreadyEmployed)
                {
                    modelState.AddModelError(nameof(UserId), "User is already employed.");
                }
            }
        }
    }
}