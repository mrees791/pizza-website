using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DataLibrary.Models;
using DataLibrary.Models.Tables;
using Microsoft.AspNet.Identity;

namespace PizzaWebsite.Models.Identity.Stores
{
    /// <summary>
    ///     A custom implementation of the Identity framework's user storage interface.
    ///     Reference:
    ///     https://docs.microsoft.com/en-us/aspnet/identity/overview/extensibility/overview-of-custom-storage-providers-for-aspnet-identity
    /// </summary>
    public class UserStore :
        IUserStore<IdentityUser>,
        IUserPasswordStore<IdentityUser>,
        IUserEmailStore<IdentityUser>,
        IUserRoleStore<IdentityUser>,
        IUserClaimStore<IdentityUser>,
        IUserLoginStore<IdentityUser>,
        IUserSecurityStampStore<IdentityUser>,
        IUserPhoneNumberStore<IdentityUser>,
        IUserTwoFactorStore<IdentityUser, string>,
        IUserLockoutStore<IdentityUser, string>
    {
        private readonly PizzaDatabase _pizzaDb;

        public UserStore(PizzaDatabase pizzaDb)
        {
            _pizzaDb = pizzaDb;
        }

        public async Task<IList<Claim>> GetClaimsAsync(IdentityUser user)
        {
            List<UserClaim> userClaims =
                new List<UserClaim>(await _pizzaDb.GetListAsync<UserClaim>(new {UserId = user.Id}));
            IList<Claim> claims = new List<Claim>(userClaims.Select(uc => new Claim(uc.ClaimType, uc.ClaimValue)));
            return claims;
        }

        public async Task AddClaimAsync(IdentityUser user, Claim claim)
        {
            UserClaim userClaim = new UserClaim
            {
                UserId = user.Id,
                ClaimType = claim.Type,
                ClaimValue = claim.Value
            };

            await _pizzaDb.InsertAsync(userClaim);
        }

        public async Task RemoveClaimAsync(IdentityUser user, Claim claim)
        {
            await _pizzaDb.RemoveClaimAsync(user.Id, claim.Type, claim.Value);
        }

        public async Task SetEmailAsync(IdentityUser user, string email)
        {
            user.Email = email;
            await Task.FromResult(0);
        }

        public async Task<string> GetEmailAsync(IdentityUser user)
        {
            return await Task.FromResult(user.Email);
        }

        public async Task<bool> GetEmailConfirmedAsync(IdentityUser user)
        {
            return await Task.FromResult(user.EmailConfirmed);
        }

        public async Task SetEmailConfirmedAsync(IdentityUser user, bool confirmed)
        {
            user.EmailConfirmed = confirmed;
            await Task.FromResult(0);
        }

        public async Task<IdentityUser> FindByEmailAsync(string email)
        {
            SiteUser siteUser = await _pizzaDb.GetSiteUserByEmailAsync(email);

            if (siteUser == null)
            {
                return null;
            }

            return new IdentityUser(siteUser);
        }

        public async Task<DateTimeOffset> GetLockoutEndDateAsync(IdentityUser user)
        {
            return await Task.FromResult(user.LockoutEndDateUtc);
        }

        public async Task SetLockoutEndDateAsync(IdentityUser user, DateTimeOffset lockoutEnd)
        {
            user.LockoutEndDateUtc = lockoutEnd.DateTime;
            await Task.FromResult(0);
        }

        public async Task<int> IncrementAccessFailedCountAsync(IdentityUser user)
        {
            int accessFailedCount = user.IncrementAccessFailedCount();
            return await Task.FromResult(accessFailedCount);
        }

        public async Task ResetAccessFailedCountAsync(IdentityUser user)
        {
            user.ResetAccessFailedCount();
            await Task.FromResult(0);
        }

        public async Task<int> GetAccessFailedCountAsync(IdentityUser user)
        {
            return await Task.FromResult(user.AccessFailedCount);
        }

        public async Task<bool> GetLockoutEnabledAsync(IdentityUser user)
        {
            return await Task.FromResult(user.LockoutEnabled);
        }

        public async Task SetLockoutEnabledAsync(IdentityUser user, bool enabled)
        {
            user.LockoutEnabled = enabled;
            await Task.FromResult(0);
        }

        public async Task AddLoginAsync(IdentityUser user, UserLoginInfo login)
        {
            UserLogin userLogin = new UserLogin
            {
                UserId = user.Id,
                LoginProvider = login.LoginProvider,
                ProviderKey = login.ProviderKey
            };

            await _pizzaDb.InsertAsync(userLogin);
        }

        public async Task RemoveLoginAsync(IdentityUser user, UserLoginInfo login)
        {
            await _pizzaDb.DeleteLoginAsync(user.Id, login.LoginProvider, login.ProviderKey);
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(IdentityUser user)
        {
            List<UserLogin> userLogins =
                new List<UserLogin>(await _pizzaDb.GetListAsync<UserLogin>(new {UserId = user.Id}));
            IList<UserLoginInfo> loginInfoList =
                new List<UserLoginInfo>(userLogins.Select(ul => new UserLoginInfo(ul.LoginProvider, ul.ProviderKey)));
            return loginInfoList;
        }

        public async Task<IdentityUser> FindAsync(UserLoginInfo login)
        {
            UserLogin currentUserLogin = await _pizzaDb.GetLoginAsync(login.LoginProvider, login.ProviderKey);

            if (currentUserLogin == null)
            {
                return null;
            }

            return await FindByIdAsync(currentUserLogin.UserId);
        }

        public async Task SetPasswordHashAsync(IdentityUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            await Task.FromResult(0);
        }

        public async Task<string> GetPasswordHashAsync(IdentityUser user)
        {
            return await Task.FromResult(user.PasswordHash);
        }

        public async Task<bool> HasPasswordAsync(IdentityUser user)
        {
            return await Task.FromResult(user.HasPassword());
        }

        public async Task SetPhoneNumberAsync(IdentityUser user, string phoneNumber)
        {
            user.PhoneNumber = phoneNumber;
            await Task.FromResult(0);
        }

        public async Task<string> GetPhoneNumberAsync(IdentityUser user)
        {
            return await Task.FromResult(user.PhoneNumber);
        }

        public async Task<bool> GetPhoneNumberConfirmedAsync(IdentityUser user)
        {
            return await Task.FromResult(user.PhoneNumberConfirmed);
        }

        public async Task SetPhoneNumberConfirmedAsync(IdentityUser user, bool confirmed)
        {
            user.PhoneNumberConfirmed = confirmed;
            await Task.FromResult(0);
        }

        public async Task<IList<string>> GetRolesAsync(IdentityUser user)
        {
            IEnumerable<UserRole> userRoleList = await _pizzaDb.GetUserRoleListAsync(user.Id);
            return new List<string>(userRoleList.Select(r => r.RoleName));
        }

        public async Task<bool> IsInRoleAsync(IdentityUser user, string roleName)
        {
            SiteRole siteRole = await _pizzaDb.GetSiteRoleByNameAsync(roleName);
            SiteUser siteUser = await _pizzaDb.GetSiteUserByIdAsync(user.Id);

            if (siteRole == null)
            {
                throw new ArgumentException($"Invalid role name: {roleName}");
            }

            if (siteUser == null)
            {
                throw new ArgumentException($"User does not exist. ID: {user.Id}");
            }

            return await _pizzaDb.UserIsInRole(siteUser, siteRole);
        }

        public async Task AddToRoleAsync(IdentityUser user, string roleName)
        {
            SiteRole siteRole = await _pizzaDb.GetSiteRoleByNameAsync(roleName);
            SiteUser siteUser = await _pizzaDb.GetSiteUserByIdAsync(user.Id);

            if (siteRole == null)
            {
                throw new ArgumentException($"Invalid role name: {roleName}");
            }

            if (siteUser == null)
            {
                throw new ArgumentException($"User does not exist. ID: {user.Id}");
            }

            if (!await IsInRoleAsync(user, roleName))
            {
                await _pizzaDb.Commands.AddUserToRoleAsync(siteUser, siteRole);
            }
        }

        public async Task RemoveFromRoleAsync(IdentityUser user, string roleName)
        {
            SiteRole siteRole = await _pizzaDb.GetSiteRoleByNameAsync(roleName);
            SiteUser siteUser = await _pizzaDb.GetSiteUserByIdAsync(user.Id);

            if (siteRole == null)
            {
                throw new ArgumentException($"Invalid role name: {roleName}");
            }

            if (siteUser == null)
            {
                throw new ArgumentException($"User does not exist. ID: {user.Id}");
            }

            if (await IsInRoleAsync(user, roleName))
            {
                await _pizzaDb.Commands.RemoveUserFromRoleAsync(siteUser, siteRole);
            }
        }

        public async Task SetSecurityStampAsync(IdentityUser user, string stamp)
        {
            user.SecurityStamp = stamp;
            await Task.FromResult(0);
        }

        public async Task<string> GetSecurityStampAsync(IdentityUser user)
        {
            return await Task.FromResult(user.SecurityStamp);
        }

        public async Task CreateAsync(IdentityUser user)
        {
            user.Id = await _pizzaDb.InsertAsync(user.ToRecord());
        }

        /// <summary>
        ///     User records should never be deleted.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task DeleteAsync(IdentityUser user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _pizzaDb.Dispose();
        }

        public async Task<IdentityUser> FindByIdAsync(string userId)
        {
            SiteUser siteUser = await _pizzaDb.GetSiteUserByIdAsync(userId);

            if (siteUser == null)
            {
                return null;
            }

            return new IdentityUser(siteUser);
        }

        public async Task<IdentityUser> FindByNameAsync(string userName)
        {
            SiteUser siteUser = await _pizzaDb.GetSiteUserByNameAsync(userName);

            if (siteUser == null)
            {
                return null;
            }

            return new IdentityUser(siteUser);
        }

        public async Task UpdateAsync(IdentityUser user)
        {
            await _pizzaDb.UpdateAsync(user.ToRecord());
        }

        public async Task SetTwoFactorEnabledAsync(IdentityUser user, bool enabled)
        {
            user.TwoFactorEnabled = enabled;
            await Task.FromResult(0);
        }

        public async Task<bool> GetTwoFactorEnabledAsync(IdentityUser user)
        {
            return await Task.FromResult(user.TwoFactorEnabled);
        }

        private bool ClaimsAreEqual(Claim claim1, Claim claim2)
        {
            return claim1.Equals(claim2);
        }

        private bool UserLoginIsEqual(UserLoginInfo login1, UserLoginInfo login2)
        {
            return login1.LoginProvider == login2.LoginProvider && login1.ProviderKey == login2.ProviderKey;
        }
    }
}