using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models
{
    public class ConfirmationViewModel
    {
        public string ConfirmationMessage { get; set; }
        public string ReturnUrlAction { get; set; }
    }
}