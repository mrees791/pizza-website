using DataLibrary.BusinessLogic.Users;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace PizzaWebsite.Identity
{
    public class UserStore : IUserStore<IdentityUser>//, IUserLoginStore<IdentityUser>, IUserClaimStore<IdentityUser>, IUserRoleStore<IdentityUser>
    {
        public Task CreateAsync(IdentityUser user)
        {
            throw new NotImplementedException();

            /*return new Task(() =>
            {
                int id = DatabaseUserProcessor.AddNewUser(user.Email, user.PasswordHash, user.PhoneNumber, user.ZipCode);
            });*/
        }

        public Task DeleteAsync(IdentityUser user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<IdentityUser> FindByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityUser> FindByNameAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(IdentityUser user)
        {
            throw new NotImplementedException();
        }
    }
}