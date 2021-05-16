using PizzaWebsite.Models.Shop;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.Attributes
{
    public class RequiredIfDeliveryAttribute : ValidationAttribute
    {
        CheckoutViewModel checkoutViewModel;

        public RequiredIfDeliveryAttribute(CheckoutViewModel checkoutViewModel)
        {
            this.checkoutViewModel = checkoutViewModel;
        }

        public override bool IsValid(object value)
        {
            // todo: Finish
            return false;
            //return value == null ? false : Regex.IsMatch(value.ToString(), zipRegex);
        }
    }
}