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
        private PizzaDatabase pizzaDb;

        public RoleStore(PizzaDatabase pizzaDb)
        {
            this.pizzaDb = pizzaDb;
        }

        public async Task CreateAsync(IdentityRole role)
        {
            await pizzaDb.InsertAsync(role.ToRecord());
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
            pizzaDb.Dispose();
        }

        public async Task<IdentityRole> FindByIdAsync(int roleId)
        {
            List<SiteRole> siteRoles = new List<SiteRole>(await pizzaDb.GetListAsync<SiteRole>(new { Id = roleId }));
            List<IdentityRole> identityRoles = new List<IdentityRole>(siteRoles.Select(siteRole => new IdentityRole(siteRole)));
            return identityRoles.FirstOrDefault();
        }

        public async Task<IdentityRole> FindByNameAsync(string roleName)
        {
            List<SiteRole> siteRoles = new List<SiteRole>(await pizzaDb.GetListAsync<SiteRole>(new { Name = roleName }));
            List<IdentityRole> identityRoles = new List<IdentityRole>(siteRoles.Select(siteRole => new IdentityRole(siteRole)));
            IdentityRole currentRole = identityRoles.FirstOrDefault();

            return currentRole;
        }

        public async Task UpdateAsync(IdentityRole role)
        {
            await pizzaDb.UpdateAsync(role.ToRecord());
        }
    }
}