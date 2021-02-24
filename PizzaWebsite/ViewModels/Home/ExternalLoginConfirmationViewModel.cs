using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PizzaWebsite.ViewModels.Home
{
    public class ExternalLoginConfirmationViewModel
    {
        public string ReturnUrl { get; set; }
        public string LoginProvider { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}