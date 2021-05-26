using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models
{
    public class ValidationError
    {
        public string Key { get; set; }
        public string ErrorMessage { get; set; }

        public ValidationError(string key, string errorMessage)
        {
            Key = key;
            ErrorMessage = errorMessage;
        }
    }
}