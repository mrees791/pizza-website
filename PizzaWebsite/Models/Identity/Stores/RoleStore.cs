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
    public class RoleStore : IRoleStore<IdentityRole, string>
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

        public async Task<IdentityRole> FindByIdAsync(string roleId)
        {
            return await FindByNameAsync(roleId);
        }

        public async Task<IdentityRole> FindByNameAsync(string roleName)
        {
            return new IdentityRole(await pizzaDb.GetSiteRoleByNameAsync(roleName));
        }

        public async Task UpdateAsync(IdentityRole role)
        {
            await pizzaDb.UpdateAsync(role.ToRecord());
        }
    }
}