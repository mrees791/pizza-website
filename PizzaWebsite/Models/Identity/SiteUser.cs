using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace PizzaWebsite.Models.Identity
{
    public class SiteUser : IUser<int>
    {
        private int id;
        private string userName;
        private string passwordHash;
        private string email;
        private bool emailConfirmed;

        // This constructor will be used when users use external logins (UserLogin)
        public SiteUser()
        {
        }

        public SiteUser(string userName, string passwordHash, string email)
        {
            this.userName = userName;
            this.passwordHash = passwordHash;
            this.email = email;
        }

        public bool HasPassword()
        {
            return !(string.IsNullOrEmpty(passwordHash));
        }

        public bool HasEmail()
        {
            return !(string.IsNullOrEmpty(email));
        }

        public int Id { get => id; set => id = value; }

        public string UserName { get => userName; set => userName = value; }
        public string PasswordHash { get => passwordHash; set => passwordHash = value; }
        public string Email { get => email; set => email = value; }
        public bool EmailConfirmed { get => emailConfirmed; set => emailConfirmed = value; }
    }
}