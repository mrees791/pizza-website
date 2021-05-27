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
        private const string employeeRegex = @"^([A-Z]|\d)*$";

        public ValidEmployeeIdAttribute()
        {
            ErrorMessage = "Employee ID's can only have numeric digits and upper-case letters.";
        }

        public override bool IsValid(object value)
        {
            return value == null ? false : Regex.IsMatch(value.ToString(), employeeRegex);
        }
    }
}