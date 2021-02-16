using DataLibrary.BusinessLogic.Users;
using DataLibrary.Models.Users;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PizzaWebsite.Models.Identity
{
    /// <summary>
    /// A synchronous implementation of Microsoft's user store interfaces with role-based authorization.
    /// </summary>
    public class UserStoreModel : IUserStore<IdentityUserModel>, IUserPasswordStore<IdentityUserModel>, IUserRoleStore<IdentityUserModel>, IUserEmailStore<IdentityUserModel>
    {
        public Task CreateAsync(IdentityUserModel user)
        {
            int userId = DatabaseUserProcessor.AddNewUser(user.ToDbModel());
            user.Id = userId.ToString();
            return Task.FromResult(0);
        }

        public Task DeleteAsync(IdentityUserModel user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }

        public Task<IdentityUserModel> FindByIdAsync(string userId)
        {
            UserModel user = DatabaseUserProcessor.FindUserById(int.Parse(userId));

            if (user != null)
            {
                return Task.FromResult(new IdentityUserModel(user));
            }
            return Task.FromResult(new IdentityUserModel());
        }

        public Task<IdentityUserModel> FindByNameAsync(string userName)
        {
            UserModel user = DatabaseUserProcessor.FindUserByName(userName);

            if (user != null)
            {
                return Task.FromResult(new IdentityUserModel(user));
            }
            return Task.FromResult(new IdentityUserModel());
        }

        public Task<string> GetPasswordHashAsync(IdentityUserModel user)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(IdentityUserModel user)
        {
            return Task.FromResult(user.PasswordHash.Any());
        }

        public Task SetPasswordHashAsync(IdentityUserModel user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task UpdateAsync(IdentityUserModel user)
        {
            int rowsUpdated = DatabaseUserProcessor.UpdateUser(user.ToDbModel());

            if (rowsUpdated > 0)
            {
                return Task.FromResult(0);
            }
            throw new Exception($"Unable to update user record for user ID {user.Id}.");
        }
        
        public Task<IList<string>> GetRolesAsync(IdentityUserModel user)
        {
            return Task.FromResult(user.Roles);
        }

        public Task AddToRoleAsync(IdentityUserModel user, string roleName)
        {
            user.Roles.Add(roleName);
            return Task.FromResult(0);
        }

        public Task<bool> IsInRoleAsync(IdentityUserModel user, string roleName)
        {
            return Task.FromResult(user.Roles.Contains(roleName));
        }

        public Task RemoveFromRoleAsync(IdentityUserModel user, string roleName)
        {
            return Task.FromResult(user.Roles.Remove(roleName));
        }

        public Task SetEmailAsync(IdentityUserModel user, string email)
        {
            user.Email = email;
            return Task.FromResult(0);
        }

        public Task<string> GetEmailAsync(IdentityUserModel user)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(IdentityUserModel user)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(IdentityUserModel user, bool confirmed)
        {
            user.EmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task<IdentityUserModel> FindByEmailAsync(string email)
        {
            UserModel user = DatabaseUserProcessor.FindUserByEmail(email);

            if (user != null)
            {
                return Task.FromResult(new IdentityUserModel(user));
            }
            return Task.FromResult(new IdentityUserModel());
        }
    }
}