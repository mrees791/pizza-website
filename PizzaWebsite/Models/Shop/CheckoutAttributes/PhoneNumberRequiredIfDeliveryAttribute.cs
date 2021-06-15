using PizzaWebsite.Models.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace PizzaWebsite.Models.Shop.CheckoutAttributes
{
    public class PhoneNumberRequiredIfDeliveryAttribute : PhoneNumberAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            CheckoutViewModel checkoutModel = (CheckoutViewModel)validationContext.ObjectInstance;
            if (checkoutModel.IsDelivery())
            {
                if (value != null && value.ToString().Any())
                {
                    if (Regex.IsMatch(value.ToString(), phoneRegex))
                    {
                        return ValidationResult.Success;
                    }
                }
                return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success;
        }
    }
}