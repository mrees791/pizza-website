using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PizzaWebsite.Models.Identity
{
    public class UserStore : IUserStore<SiteUser, int>
    {
        public Task CreateAsync(SiteUser user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(SiteUser user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<SiteUser> FindByIdAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<SiteUser> FindByNameAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(SiteUser user)
        {
            throw new NotImplementedException();
        }
    }
}