using DataLibrary.BusinessLogic.Users;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PizzaWebsite.Models.Identity.Validators
{
    public class UserValidatorModel : IIdentityValidator<IdentityUserModel>
    {
        public Task<IdentityResult> ValidateAsync(IdentityUserModel item)
        {
            List<string> errors = new List<string>();

            // todo: Finish implementing validation
            ValidateUserName(item, errors);
            ValidateEmail(item, errors);
            ValidatePhoneNumber(item, errors);

            return Task.FromResult(new IdentityResult(errors));
        }

        private void ValidateUserName(IdentityUserModel item, List<string> errors)
        {
            var previousUser = DatabaseUserProcessor.FindUserByName(item.UserName);
            bool nameAlreadyInUse = previousUser != null;

            if (nameAlreadyInUse)
            {
                errors.Add($"Name '{item.UserName}' has already been taken.");
            }
        }

        private void ValidateEmail(IdentityUserModel item, List<string> errors)
        {

        }

        private void ValidatePhoneNumber(IdentityUserModel item, List<string> errors)
        {

        }
    }
}