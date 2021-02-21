using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PizzaWebsite.Models.Identity
{
    public class UserStore :
        IUserStore<SiteUser, int>,
        IUserRoleStore<SiteUser, int>
    {
        // User, role definitions
        private List<SiteRole> roles;
        private List<SiteUser> users;

        // Links users to roles
        private List<UserRole> userRoles;

        public UserStore()
        {
            users = new List<SiteUser>();
            roles = new List<SiteRole>();
            userRoles = new List<UserRole>();

            CreateDummyDatabase();
        }

        private void CreateDummyDatabase()
        {
            // Add records to dummy database
            SiteRole managerRole = new SiteRole("Manager", 1);
            roles.Add(managerRole);

            // Add users
            CreateAsync(new SiteUser(1, "basic_user"));
            CreateAsync(new SiteUser(2, "manager_user"));

            // Add user to manager role
            SiteUser managerUser = FindByNameAsync("manager_user").Result;
            AddToRoleAsync(managerUser, "Manager");
        }

        public Task AddToRoleAsync(SiteUser user, string roleName)
        {
            int userRoleId = userRoles.Count + 1;
            SiteRole siteRole = roles.Where(r => r.Name == roleName).First();
            UserRole userRole = new UserRole(userRoleId, user.Id, siteRole.Id);

            // Add UserRole record to dummy database
            userRoles.Add(userRole);

            return Task.FromResult(0);
        }

        public Task CreateAsync(SiteUser user)
        {
            // Add SiteUser to dummy database
            users.Add(user);

            return Task.FromResult(0);
        }

        /// <summary>
        /// User records should never be deleted.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task DeleteAsync(SiteUser user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }

        public Task<SiteUser> FindByIdAsync(int userId)
        {
            return Task.FromResult(users.Where(u => u.Id == userId).First());
        }

        public Task<SiteUser> FindByNameAsync(string userName)
        {
            return Task.FromResult(users.Where(u => u.UserName == userName).First());
        }

        public Task<IList<string>> GetRolesAsync(SiteUser user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsInRoleAsync(SiteUser user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(SiteUser user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(SiteUser user)
        {
            throw new NotImplementedException();
        }
    }
}