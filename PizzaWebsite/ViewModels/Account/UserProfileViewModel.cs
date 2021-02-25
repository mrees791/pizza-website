using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWebsite.ViewModels.Account
{
    public class UserProfileViewModel
    {
        private bool userIsSignedIn;
        private string message1;

        public string Message1 { get => message1; set => message1 = value; }
        public bool UserIsSignedIn { get => userIsSignedIn; set => userIsSignedIn = value; }
    }
}