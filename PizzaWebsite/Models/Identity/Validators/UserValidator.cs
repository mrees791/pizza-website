using DataLibrary.BusinessLogic.Users;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PizzaWebsite.Models.Identity.Validators
{
    public class UserValidator : IIdentityValidator<IdentityUserModel>
    {
        public Task<IdentityResult> ValidateAsync(IdentityUserModel item)
        {
            List<string> errors = new List<string>();
            IdentityResult result = new IdentityResult();

            // Validate username already exists
            var previousUserName = DatabaseUserProcessor.FindUserByName(item.UserName);

            if (previousUserName != null)
            {
                errors.Add($"Name '{item.UserName}' has already been taken.");
            }

            // todo: Finish implementing validation
            // Validate email
            // Validate phone number

            return Task.FromResult(new IdentityResult(errors));
        }
    }
}