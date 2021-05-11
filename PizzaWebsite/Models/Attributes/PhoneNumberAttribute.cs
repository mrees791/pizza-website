using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace PizzaWebsite.Models.Attributes
{
    public class PhoneNumberAttribute : ValidationAttribute
    {
        private const string phoneRegex = @"^\d{10}$";

        public PhoneNumberAttribute()
        {
            ErrorMessage = "Your phone number should have ten digits.";
        }

        public override bool IsValid(object value)
        {
            return value == null ? false : Regex.IsMatch(value.ToString(), phoneRegex);
        }
    }
}