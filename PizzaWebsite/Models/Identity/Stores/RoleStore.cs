using DataLibrary.Models;
using DataLibrary.Models.Tables;
using Microsoft.AspNet.Identity;
using PizzaWebsite.Models.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PizzaWebsite.Models.Identity.Stores
{
    /// <summary>
    /// A custom implementation of the Identity framework's role storage interface.
    /// Reference:
    /// https://docs.microsoft.com/en-us/aspnet/identity/overview/extensibility/overview-of-custom-storage-providers-for-aspnet-identity
    /// </summary>
    public class RoleStore : IRoleStore<IdentityRole, int>
    {
        private PizzaDatabase database;

        public RoleStore(PizzaDatabase database)
        {
            this.database = database;
        }

        public Task CreateAsync(IdentityRole role)
        {
            database.Insert(role.ToDbModel());
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
            database.Dispose();
        }

        public async Task<IdentityRole> FindByIdAsync(int roleId)
        {
            List<SiteRole> siteRoles = await database.GetListAsync<SiteRole>("", new { Id = roleId });
            List<IdentityRole> identityRoles = siteRoles.Select(sr => new IdentityRole(sr)).ToList();
            IdentityRole currentRole = identityRoles.FirstOrDefault();

            return currentRole;
        }

        public async Task<IdentityRole> FindByNameAsync(string roleName)
        {
            List<SiteRole> siteRoles = await database.GetListAsync<SiteRole>("", new { Name = roleName });
            List<IdentityRole> identityRoles = siteRoles.Select(sr => new IdentityRole(sr)).ToList();
            IdentityRole currentRole = identityRoles.FirstOrDefault();

            return currentRole;
        }

        public Task UpdateAsync(IdentityRole role)
        {
            database.Update(role.ToDbModel());
            return Task.FromResult(0);
        }
    }
}