using DataLibrary.Models.Tables;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace PizzaWebsite.Models.Identity
{
    public class IdentityUser : IUser<int>
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
        private DateTimeOffset lockoutEndDateUtc;
        private int accessFailedCount;
        private bool lockoutEnabled;

        private string zipCode;
        private int confirmOrderCartId;
        private int currentCartId;
        private bool isBanned;
        private int orderConfirmationId;

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUserManager manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        // This constructor will be used when users use external logins (UserLogin)
        public IdentityUser()
        {
            lockoutEndDateUtc = DateTimeOffset.Now;
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
        public DateTimeOffset LockoutEndDateUtc { get => lockoutEndDateUtc; set => lockoutEndDateUtc = value; }
        public int AccessFailedCount { get => accessFailedCount; set => accessFailedCount = value; }
        public bool LockoutEnabled { get => lockoutEnabled; set => lockoutEnabled = value; }
        public string ZipCode { get => zipCode; set => zipCode = value; }
        public int ConfirmOrderCartId { get => confirmOrderCartId; set => confirmOrderCartId = value; }
        public int CurrentCartId { get => currentCartId; set => currentCartId = value; }
        public bool IsBanned { get => isBanned; set => isBanned = value; }
        public int OrderConfirmationId { get => orderConfirmationId; set => orderConfirmationId = value; }

        public SiteUser ToDbRecord()
        {
            return new SiteUser()
            {
                AccessFailedCount = accessFailedCount,
                ConfirmOrderCartId = ConfirmOrderCartId,
                CurrentCartId = CurrentCartId,
                Email = Email,
                EmailConfirmed = EmailConfirmed,
                Id = Id,
                IsBanned = IsBanned,
                LockoutEnabled = LockoutEnabled,
                LockoutEndDateUtc = LockoutEndDateUtc.Date,
                OrderConfirmationId = OrderConfirmationId,
                PasswordHash = PasswordHash,
                PhoneNumber = PhoneNumber,
                PhoneNumberConfirmed = PhoneNumberConfirmed,
                SecurityStamp = SecurityStamp,
                TwoFactorEnabled = TwoFactorEnabled,
                UserName = UserName,
                ZipCode = ZipCode
            };
        }

        public IdentityUser(SiteUser dbRecord)
        {
            AccessFailedCount = dbRecord.AccessFailedCount;
            ConfirmOrderCartId = dbRecord.ConfirmOrderCartId;
            CurrentCartId = dbRecord.CurrentCartId;
            Email = dbRecord.Email;
            EmailConfirmed = dbRecord.EmailConfirmed;
            Id = dbRecord.Id;
            IsBanned = dbRecord.IsBanned;
            LockoutEnabled = dbRecord.LockoutEnabled;
            LockoutEndDateUtc = dbRecord.LockoutEndDateUtc;
            OrderConfirmationId = dbRecord.OrderConfirmationId;
            PasswordHash = dbRecord.PasswordHash;
            PhoneNumber = dbRecord.PhoneNumber;
            PhoneNumberConfirmed = dbRecord.PhoneNumberConfirmed;
            SecurityStamp = dbRecord.SecurityStamp;
            TwoFactorEnabled = dbRecord.TwoFactorEnabled;
            UserName = dbRecord.UserName;
            ZipCode = dbRecord.ZipCode;
        }
    }
}