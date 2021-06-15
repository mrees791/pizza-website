using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PizzaWebsite.Models.Attributes
{
    public class ZipCodeAttribute : ValidationAttribute
    {
        protected const string ZipRegex = @"^\d{5}$";

        public ZipCodeAttribute()
        {
            ErrorMessage = "Your zip code should have five digits.";
        }

        public override bool IsValid(object value)
        {
            return value == null ? false : Regex.IsMatch(value.ToString(), ZipRegex);
        }
    }
}