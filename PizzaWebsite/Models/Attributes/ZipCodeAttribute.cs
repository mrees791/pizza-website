using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace PizzaWebsite.Models.Attributes
{
    public class ZipCodeAttribute : ValidationAttribute
    {
        protected const string zipRegex = @"^\d{5}$";

        public ZipCodeAttribute() : base()
        {
            ErrorMessage = "Your zip code should have five digits.";
        }

        public override bool IsValid(object value)
        {
            return value == null ? false : Regex.IsMatch(value.ToString(), zipRegex);
        }
    }
}