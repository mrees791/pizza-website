using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace PizzaWebsite.Models.Attributes
{
    public class ValidEmployeeIdAttribute : ValidationAttribute
    {
        private const string EmployeeIdRegex = @"^([A-Z]|\d)*$";
        private const int CharacterLimit = 256;

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                ErrorMessage = "Employee ID is required.";
                return false;
            }

            string employeeId = value.ToString();

            if (employeeId.Length > CharacterLimit)
            {
                ErrorMessage = $"Employee ID's can only have up to {CharacterLimit} characters.";
                return false;
            }

            if (!Regex.IsMatch(employeeId, EmployeeIdRegex))
            {
                ErrorMessage = $"Employee ID's can only have numeric digits and upper-case letters.";
                return false;
            }

            return true;
        }
    }
}