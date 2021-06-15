using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PizzaWebsite.Models.Attributes
{
    public class PhoneNumberAttribute : ValidationAttribute
    {
        protected const string PhoneRegex = @"^\d{10}$";

        public PhoneNumberAttribute()
        {
            ErrorMessage = "Your phone number should have ten digits.";
        }

        public override bool IsValid(object value)
        {
            return value == null ? false : Regex.IsMatch(value.ToString(), PhoneRegex);
        }
    }
}