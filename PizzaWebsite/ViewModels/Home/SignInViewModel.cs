using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PizzaWebsite.ViewModels.Home
{
    public class SignInViewModel
    {
        private bool userIsSignedIn;
        private string alreadySignedInMessage;

        [Display(Name = "Username")]
        [Required(ErrorMessage = "You must enter your username.")]
        public string UserName { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "You must enter your password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool UserIsSignedIn { get => userIsSignedIn; set => userIsSignedIn = value; }
        public string AlreadySignedInMessage { get => alreadySignedInMessage; set => alreadySignedInMessage = value; }
    }
}