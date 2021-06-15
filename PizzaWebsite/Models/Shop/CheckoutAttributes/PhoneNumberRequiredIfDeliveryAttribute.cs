﻿using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using PizzaWebsite.Models.Attributes;

namespace PizzaWebsite.Models.Shop.CheckoutAttributes
{
    public class PhoneNumberRequiredIfDeliveryAttribute : PhoneNumberAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            CheckoutViewModel checkoutModel = (CheckoutViewModel) validationContext.ObjectInstance;
            if (checkoutModel.IsDelivery())
            {
                if (value != null && value.ToString().Any())
                {
                    if (Regex.IsMatch(value.ToString(), PhoneRegex))
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