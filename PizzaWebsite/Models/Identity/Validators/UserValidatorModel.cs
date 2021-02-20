using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PizzaWebsite.Models.Identity.Validators
{
    /// <summary>
    /// Provides server-side validation for user registration. Used by the UserManager UserValidator property.
    /// </summary>
    public class UserValidatorModel : IIdentityValidator<IdentityUser>
    {
        private UserStoreModelOLD userStore;

        public UserValidatorModel(UserStoreModelOLD userStore)
        {
            this.userStore = userStore;
        }

        public Task<IdentityResult> ValidateAsync(IdentityUser item)
        {
            List<string> errors = new List<string>();

            ValidateUserName(item, errors);
            ValidateEmail(item, errors);
            ValidatePhoneNumber(item, errors);

            if (errors.Any())
            {
                return Task.FromResult(new IdentityResult(errors));
            }
            return Task.FromResult(IdentityResult.Success);
        }

        private void ValidateUserName(IdentityUser item, List<string> errors)
        {
            var previousUser = userStore.FindByNameAsync(item.UserName).Result;
            bool nameAlreadyInUse = previousUser.Id != null;

            if (nameAlreadyInUse)
            {
                errors.Add($"Name '{item.UserName}' has already been taken.");
            }
        }

        private void ValidateEmail(IdentityUser item, List<string> errors)
        {
            var previousUser = userStore.FindByEmailAsync(item.Email).Result;
            bool emailAlreadyInUse = previousUser.Id != null;

            if (emailAlreadyInUse)
            {
                errors.Add($"Email '{item.Email}' is already in use.");
            }
        }

        private void ValidatePhoneNumber(IdentityUser item, List<string> errors)
        {
            var previousUser = userStore.FindByPhoneNumberAsync(item.PhoneNumber).Result;
            bool phoneNumberAlreadyInUse = previousUser.Id != null;

            if (phoneNumberAlreadyInUse)
            {
                errors.Add($"Phone number is already in use.");
            }
        }
    }
}