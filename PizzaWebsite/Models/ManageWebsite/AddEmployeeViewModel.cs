using DataLibrary.Models;
using DataLibrary.Models.Tables;
using PizzaWebsite.Models.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PizzaWebsite.Models.ManageWebsite
{
    public class AddEmployeeViewModel
    {
        [Display(Name = "Employee ID")]
        [StringLength(256, ErrorMessage = "Employee ID cannot be longer than 256 characters.")]
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
        public async Task<IEnumerable<ValidationError>> ValidateAsync(PizzaDatabase pizzaDb)
        {
            List<ValidationError> errorList = new List<ValidationError>();

            // Check if user exists
            SiteUser user = await pizzaDb.GetSiteUserByNameAsync(UserId);

            if (user == null)
            {
                errorList.Add(new ValidationError(nameof(UserId), "User does not exist."));
            }

            // Make sure employee ID isn't already taken
            Employee employee = await pizzaDb.GetAsync<Employee>(Id);

            if (employee != null)
            {
                errorList.Add(new ValidationError(nameof(Id), "Employee ID is already taken."));
            }


            // Make sure user isn't already employed
            if (user != null)
            {
                bool alreadyEmployed = await pizzaDb.UserIsInRole(user.Id, "Employee");

                if (alreadyEmployed)
                {
                    errorList.Add(new ValidationError(nameof(UserId), "User is already employed."));
                }
            }

            return errorList;
        }
    }
}