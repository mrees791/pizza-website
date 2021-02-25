using Microsoft.AspNet.Identity;
using PizzaWebsite2.Models.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PizzaWebsite2.Models.Identity
{
    public class RoleStore : IRoleStore<IdentityRole, int>
    {
        private DummyDatabase dbContext;

        public RoleStore()
        {
            dbContext = new DummyDatabase();
        }

        public Task CreateAsync(IdentityRole role)
        {
            dbContext.AddRecord(role);
            return Task.FromResult(0);
        }

        /// <summary>
        /// Role records should never be deleted.
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public Task DeleteAsync(IdentityRole role)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }

        public Task<IdentityRole> FindByIdAsync(int roleId)
        {
            List<IdentityRole> roles = dbContext.LoadRoles();
            IdentityRole role = roles.Where(r => r.Id == roleId).FirstOrDefault();

            if (role != null)
            {
                return Task.FromResult(role);
            }

            return Task.FromResult<IdentityRole>(null);
        }

        public Task<IdentityRole> FindByNameAsync(string roleName)
        {
            List<IdentityRole> roles = dbContext.LoadRoles();
            IdentityRole role = roles.Where(r => r.Name == roleName).FirstOrDefault();

            if (role != null)
            {
                return Task.FromResult(role);
            }

            return Task.FromResult<IdentityRole>(null);
        }

        public Task UpdateAsync(IdentityRole role)
        {
            // Update role record in database.
            throw new NotImplementedException();
        }
    }
}