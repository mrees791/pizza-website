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
    public class IdentityUser : IUser<int>, IRecordConverter<SiteUser>
    {
        public int Id { get; set; }
        public string UserName { get; set; }
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
        public int ConfirmOrderCardId { get; set; }
        public bool IsBanned { get; set; }
        public int OrderConfirmationId { get; set; }

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
            LockoutEndDateUtc = DateTimeOffset.Now;
        }

        public bool HasPassword()
        {
            return !(string.IsNullOrEmpty(PasswordHash));
        }

        public bool HasEmail()
        {
            return !(string.IsNullOrEmpty(Email));
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
            return new SiteUser()
            {
                AccessFailedCount = AccessFailedCount,
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

        public void FromRecord(SiteUser dbModel)
        {
            AccessFailedCount = dbModel.AccessFailedCount;
            CurrentCartId = dbModel.CurrentCartId;
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
            UserName = dbModel.UserName;
            ZipCode = dbModel.ZipCode;
        }

        public IdentityUser(SiteUser dbModel)
        {
            FromRecord(dbModel);
        }
    }
}