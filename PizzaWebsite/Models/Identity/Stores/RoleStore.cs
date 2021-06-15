using System;
using System.Threading.Tasks;
using DataLibrary.Models;
using Microsoft.AspNet.Identity;

namespace PizzaWebsite.Models.Identity.Stores
{
    /// <summary>
    ///     A custom implementation of the Identity framework's role storage interface.
    ///     Reference:
    ///     https://docs.microsoft.com/en-us/aspnet/identity/overview/extensibility/overview-of-custom-storage-providers-for-aspnet-identity
    /// </summary>
    public class RoleStore : IRoleStore<IdentityRole, string>
    {
        private readonly PizzaDatabase _pizzaDb;

        public RoleStore(PizzaDatabase pizzaDb)
        {
            _pizzaDb = pizzaDb;
        }

        public async Task CreateAsync(IdentityRole role)
        {
            await _pizzaDb.InsertAsync(role.ToRecord());
        }

        /// <summary>
        ///     Role records should never be deleted.
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public Task DeleteAsync(IdentityRole role)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _pizzaDb.Dispose();
        }

        public async Task<IdentityRole> FindByIdAsync(string roleId)
        {
            return await FindByNameAsync(roleId);
        }

        public async Task<IdentityRole> FindByNameAsync(string roleName)
        {
            return new IdentityRole(await _pizzaDb.GetSiteRoleByNameAsync(roleName));
        }

        public async Task UpdateAsync(IdentityRole role)
        {
            await _pizzaDb.UpdateAsync(role.ToRecord());
        }
    }
}