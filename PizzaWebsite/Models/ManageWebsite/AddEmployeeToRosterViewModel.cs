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
    public class AddEmployeeToRosterViewModel
    {
        public int StoreId { get; set; }
        public string StoreName { get; set; }

        [Display(Name = "Employee ID")]
        [StringLength(256, ErrorMessage = "Employee ID cannot be longer than 256 characters.")]
        [ValidEmployeeId]
        [Required]
        public string EmployeeId { get; set; }

        public async Task InitializeAsync(int storeId, PizzaDatabase pizzaDb)
        {
            StoreLocation storeLocation = await pizzaDb.GetAsync<StoreLocation>(storeId);

            StoreId = storeId;
            StoreName = storeLocation.Name;
        }

        public async Task ValidateAsync(ModelStateDictionary modelState, PizzaDatabase pizzaDb)
        {
            // Make sure employee exists.
            Employee employee = await pizzaDb.GetAsync<Employee>(EmployeeId);

            if (employee == null)
            {
                modelState.AddModelError(nameof(EmployeeId), $"Employee with ID {EmployeeId} does not exist.");
            }
            else
            {
                // Make sure store exists.
                StoreLocation storeLocation = await pizzaDb.GetAsync<StoreLocation>(StoreId);

                if (storeLocation == null)
                {
                    modelState.AddModelError("", $"Store with ID {StoreId} does not exist.");
                }
                else
                {
                    // Make sure employee isn't already employed at this location.
                    bool alreadyEmployedAtLocation = await pizzaDb.Commands.IsEmployedAtLocation(employee, storeLocation);

                    if (alreadyEmployedAtLocation)
                    {
                        modelState.AddModelError(nameof(EmployeeId), $"Employee with ID {EmployeeId} is already employed at this location.");
                    }
                }
            }
        }
    }
}