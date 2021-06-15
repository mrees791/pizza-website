using System;
using System.Security.Claims;
using System.Threading.Tasks;
using DataLibrary.Models.Tables;
using Microsoft.AspNet.Identity;

namespace PizzaWebsite.Models.Identity
{
    public class IdentityUser : IUser<string>
    {
        // This constructor will be used when users use external logins (UserLogin)
        public IdentityUser()
        {
            LockoutEndDateUtc = DateTimeOffset.Now;
        }

        public IdentityUser(SiteUser dbModel)
        {
            FromRecord(dbModel);
        }

        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset LockoutEndDateUtc { get; set; }
        public int AccessFailedCount { get; set; }
        public bool LockoutEnabled { get; set; }
        public string ZipCode { get; set; }
        public int CurrentCartId { get; set; }
        public int ConfirmOrderCartId { get; set; }
        public bool IsBanned { get; set; }
        public int OrderConfirmationId { get; set; }
        public string Id { get; set; }

        public string UserName
        {
            get => Id;
            set => Id = value;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUserManager manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            ClaimsIdentity userIdentity =
                await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public bool HasPassword()
        {
            return !string.IsNullOrEmpty(PasswordHash);
        }

        public bool HasEmail()
        {
            return !string.IsNullOrEmpty(Email);
        }

        public int IncrementAccessFailedCount()
        {
            return ++AccessFailedCount;
        }

        public void ResetAccessFailedCount()
        {
            AccessFailedCount = 0;
        }

        public SiteUser ToRecord()
        {
            return new SiteUser
            {
                AccessFailedCount = AccessFailedCount,
                CurrentCartId = CurrentCartId,
                ConfirmOrderCartId = ConfirmOrderCartId,
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
                ZipCode = ZipCode
            };
        }

        public void FromRecord(SiteUser dbModel)
        {
            AccessFailedCount = dbModel.AccessFailedCount;
            CurrentCartId = dbModel.CurrentCartId;
            ConfirmOrderCartId = dbModel.ConfirmOrderCartId;
            Email = dbModel.Email;
            EmailConfirmed = dbModel.EmailConfirmed;
            Id = dbModel.Id;
            IsBanned = dbModel.IsBanned;
            LockoutEnabled = dbModel.LockoutEnabled;
            LockoutEndDateUtc = dbModel.LockoutEndDateUtc;
            OrderConfirmationId = dbModel.OrderConfirmationId;
            PasswordHash = dbModel.PasswordHash;
            PhoneNumber = dbModel.PhoneNumber;
            PhoneNumberConfirmed = dbModel.PhoneNumberConfirmed;
            SecurityStamp = dbModel.SecurityStamp;
            TwoFactorEnabled = dbModel.TwoFactorEnabled;
            ZipCode = dbModel.ZipCode;
        }
    }
}