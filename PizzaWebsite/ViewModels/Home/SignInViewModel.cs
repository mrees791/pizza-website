using Microsoft.Owin.Security;
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
        private string returnUrl;
        private List<AuthenticationDescription> loginProviders;

        public SignInViewModel()
        {
            loginProviders = new List<AuthenticationDescription>();
        }

        [Display(Name = "Username")]
        [Required(ErrorMessage = "You must enter your username.")]
        public string UserName { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "You must enter your password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool UserIsSignedIn { get => userIsSignedIn; set => userIsSignedIn = value; }
        public string AlreadySignedInMessage { get => alreadySignedInMessage; set => alreadySignedInMessage = value; }
        public List<AuthenticationDescription> LoginProviders { get => loginProviders; }
        public string ReturnUrl { get => returnUrl; set => returnUrl = value; }
    }
}