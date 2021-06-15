using PizzaWebsite.Models.Shop;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.Shop.CheckoutAttributes
{
    public class RequiredIfDeliveryAttribute : ValidationAttribute
    {
        public RequiredIfDeliveryAttribute(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            CheckoutViewModel checkoutModel = (CheckoutViewModel)validationContext.ObjectInstance;
            if (checkoutModel.IsDelivery())
            {
                if (value != null && value.ToString().Any())
                {
                    return ValidationResult.Success;
                }
                return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success;
        }
    }
}