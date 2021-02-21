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
        private string securityStamp;
        private string phoneNumber;
        private bool phoneNumberConfirmed;
        private bool twoFactorEnabled;

        private DateTimeOffset lockoutEndDate;
        private int accessFailedCount;
        private bool lockoutEnabled;

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

        public int IncrementAccessFailedCount()
        {
            return ++accessFailedCount;
        }

        public void ResetAccessFailedCount()
        {
            accessFailedCount = 0;
        }

        public int Id { get => id; set => id = value; }

        public string UserName { get => userName; set => userName = value; }
        public string PasswordHash { get => passwordHash; set => passwordHash = value; }
        public string Email { get => email; set => email = value; }
        public bool EmailConfirmed { get => emailConfirmed; set => emailConfirmed = value; }
        public string SecurityStamp { get => securityStamp; set => securityStamp = value; }
        public string PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
        public bool PhoneNumberConfirmed { get => phoneNumberConfirmed; set => phoneNumberConfirmed = value; }
        public bool TwoFactorEnabled { get => twoFactorEnabled; set => twoFactorEnabled = value; }
        public DateTimeOffset LockoutEndDate { get => lockoutEndDate; set => lockoutEndDate = value; }
        public int AccessFailedCount { get => accessFailedCount; set => accessFailedCount = value; }
        public bool LockoutEnabled { get => lockoutEnabled; set => lockoutEnabled = value; }
    }
}