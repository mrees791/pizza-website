using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Identity
{
    public class IdentityUser : IUser
    {
        private string id;
        private string userName;
        private string email;
        private string passwordHash;
        private string phoneNumber;
        private string zipCode;

        public string Id => id;
        public string UserName { get => userName; set => userName = value; }
        public string Email { get => email; set => email = value; }
        public string PasswordHash { get => passwordHash; set => passwordHash = value; }
        public string PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
        public string ZipCode { get => zipCode; set => zipCode = value; }
    }
}