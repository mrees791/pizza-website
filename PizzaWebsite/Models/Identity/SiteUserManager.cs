using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PizzaWebsite.App_Start;
using PizzaWebsite.Models.Identity.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.Identity
{
    public class SiteUserManager : UserManager<SiteUser, int>
    {
        public SiteUserManager(UserStore store) : base(store)
        {

        }

        public static SiteUserManager Create(IdentityFactoryOptions<SiteUserManager> options)
        {
            var userStore = new UserStore();
            var userManager = new SiteUserManager(userStore);

            // Validation logic for usernames
            userManager.UserValidator = new UserValidator(userStore);
            // Validation logic for passwords
            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 10
            };

            // Configure user lockout defaults
            userManager.UserLockoutEnabledByDefault = true;
            userManager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            userManager.MaxFailedAccessAttemptsBeforeLockout = 5;

            /*userManager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<SiteUser, int>
            {
                MessageFormat = "Your security code is {0}"
            });*/
            userManager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<SiteUser, int>
            {
                Subject = "Little Brutus Security Code",
                BodyFormat = "Your security code is {0}."
            });
            userManager.EmailService = new EmailService();
            //userManager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                userManager.UserTokenProvider = new DataProtectorTokenProvider<SiteUser, int>(dataProtectionProvider.Create("ASP.NET Identity"));
            }

            return userManager;
        }
    }
}