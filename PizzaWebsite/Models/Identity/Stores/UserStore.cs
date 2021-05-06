using DataLibrary.Models;
using DataLibrary.Models.Tables;
using Microsoft.AspNet.Identity;
using PizzaWebsite.Models.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace PizzaWebsite.Models.Identity.Stores
{
    /// <summary>
    /// A custom implementation of the Identity framework's user storage interface.
    /// Reference:
    /// https://docs.microsoft.com/en-us/aspnet/identity/overview/extensibility/overview-of-custom-storage-providers-for-aspnet-identity
    /// </summary>
    public class UserStore :
        IUserStore<IdentityUser, int>,
        IUserPasswordStore<IdentityUser, int>,
        IUserEmailStore<IdentityUser, int>,
        IUserRoleStore<IdentityUser, int>,
        IUserClaimStore<IdentityUser, int>,
        IUserLoginStore<IdentityUser, int>,
        IUserSecurityStampStore<IdentityUser, int>,
        IUserPhoneNumberStore<IdentityUser, int>,
        IUserTwoFactorStore<IdentityUser, int>,
        IUserLockoutStore<IdentityUser, int>
    {
        private PizzaDatabase pizzaDb;

        public UserStore(PizzaDatabase pizzaDb)
        {
            this.pizzaDb = pizzaDb;
        }

        public async Task CreateAsync(IdentityUser user)
        {
            user.Id = await pizzaDb.InsertAsync(user.ToRecord());
        }

        public async Task AddToRoleAsync(IdentityUser user, string roleName)
        {
            List<SiteRole> siteRoles = new List<SiteRole>(await pizzaDb.GetListAsync<SiteRole>(new { Name = roleName }));
            SiteRole siteRole = siteRoles.FirstOrDefault();

            if (siteRole == null)
            {
                throw new ArgumentException("Invalid role name: roleName");
            }

            UserRole userRole = new UserRole();
            userRole.UserId = user.Id;
            userRole.RoleId = siteRole.Id;

            await pizzaDb.InsertAsync(userRole);
        }

        /// <summary>
        /// User records should never be deleted.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task DeleteAsync(IdentityUser user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            pizzaDb.Dispose();
        }

        public async Task<IdentityUser> FindByIdAsync(int userId)
        {
            List<SiteUser> siteUsers = new List<SiteUser>(await pizzaDb.GetListAsync<SiteUser>(new { Id = userId }));
            SiteUser siteUser = siteUsers.FirstOrDefault();

            if (siteUser == null)
            {
                return null;
            }

            return new IdentityUser(siteUser);
        }

        public async Task<IdentityUser> FindByNameAsync(string userName)
        {
            List<SiteUser> siteUsers = new List<SiteUser>(await pizzaDb.GetListAsync<SiteUser>(new { UserName = userName }));
            SiteUser siteUser = siteUsers.FirstOrDefault();

            if (siteUser == null)
            {
                return null;
            }

            return new IdentityUser(siteUser);
        }

        public async Task<IList<string>> GetRolesAsync(IdentityUser user)
        {
            List<SiteRole> siteRoles = new List<SiteRole>(await pizzaDb.GetListAsync<SiteRole>());
            List<UserRole> currentUserRoles = new List<UserRole>(await pizzaDb.GetListAsync<UserRole>(new { UserId = user.Id }));
            IList<string> currentUserRoleNames = new List<string>();

            foreach (UserRole userRole in currentUserRoles)
            {
                var currentRole = siteRoles.Where(r => r.Id == userRole.RoleId).First();
                currentUserRoleNames.Add(currentRole.Name);
            }

            return currentUserRoleNames;
        }

        public async Task<bool> IsInRoleAsync(IdentityUser user, string roleName)
        {
            IList<string> currentUserRoles = await GetRolesAsync(user);
            bool isInRole = currentUserRoles.Contains(roleName);

            return isInRole;
        }

        public async Task RemoveFromRoleAsync(IdentityUser user, string roleName)
        {
            List<SiteRole> siteRoles = new List<SiteRole>(await pizzaDb.GetListAsync<SiteRole>(new { Name = roleName }));
            SiteRole currentRole = siteRoles.FirstOrDefault();

            if (currentRole == null)
            {
                throw new ArgumentException($"Invalid roleName: {roleName}");
            }

            UserRole currentUserRole = (await pizzaDb.GetListAsync<UserRole>(new { UserId = user.Id, RoleId = currentRole.Id })).FirstOrDefault();

            if (currentUserRole != null)
            {
                await pizzaDb.DeleteAsync(currentUserRole);
            }
        }

        public async Task UpdateAsync(IdentityUser user)
        {
            await pizzaDb.UpdateAsync(user.ToRecord());
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
            SiteUser siteUser = await pizzaDb.GetSiteUserByEmailAsync(email);

            if (siteUser == null)
            {
                return null;
            }

            return new IdentityUser(siteUser);
        }

        public async Task<IList<Claim>> GetClaimsAsync(IdentityUser user)
        {
            List<UserClaim> userClaims = new List<UserClaim>(await pizzaDb.GetListAsync<UserClaim>(new { UserId = user.Id }));
            IList<Claim> claims = new List<Claim>(userClaims.Select(uc => new Claim(uc.ClaimType, uc.ClaimValue)));
            return claims;
        }

        public async Task AddClaimAsync(IdentityUser user, Claim claim)
        {
            UserClaim userClaim = new UserClaim()
            {
                UserId = user.Id,
                ClaimType = claim.Type,
                ClaimValue = claim.Value
            };
            await pizzaDb.InsertAsync(userClaim);
        }

        public async Task RemoveClaimAsync(IdentityUser user, Claim claim)
        {
            List<UserClaim> userClaims = new List<UserClaim>(await pizzaDb.GetListAsync<UserClaim>(new { UserId = user.Id }));
            UserClaim currentClaim = userClaims.Where(uc => uc.ClaimType == claim.Type && uc.ClaimValue == claim.Value).FirstOrDefault();

            if (currentClaim != null)
            {
                await pizzaDb.DeleteAsync(currentClaim);
            }
        }

        public async Task AddLoginAsync(IdentityUser user, UserLoginInfo login)
        {
            UserLogin userLogin = new UserLogin()
            {
                UserId = user.Id,
                LoginProvider = login.LoginProvider,
                ProviderKey = login.ProviderKey
            };
            await pizzaDb.InsertAsync(userLogin);
        }

        public async Task RemoveLoginAsync(IdentityUser user, UserLoginInfo login)
        {
            List<UserLogin> userLogins = new List<UserLogin>(await pizzaDb.GetListAsync<UserLogin>(
                new { UserId = user.Id, LoginProvider = login.LoginProvider, ProviderKey = login.ProviderKey }));
            UserLogin currentUserLogin = userLogins.FirstOrDefault();

            if (currentUserLogin != null)
            {
                await pizzaDb.DeleteAsync(currentUserLogin);
            }
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(IdentityUser user)
        {
            List<UserLogin> userLogins = new List<UserLogin>(await pizzaDb.GetListAsync<UserLogin>(new { UserId = user.Id }));
            IList<UserLoginInfo> loginInfoList = new List<UserLoginInfo>(userLogins.Select(ul => new UserLoginInfo(ul.LoginProvider, ul.ProviderKey)));
            return loginInfoList;
        }

        public async Task<IdentityUser> FindAsync(UserLoginInfo login)
        {
            UserLogin currentUserLogin = await pizzaDb.GetUserLoginAsync(login.LoginProvider, login.ProviderKey);

            if (currentUserLogin == null)
            {
                return null;
            }

            IdentityUser user = await FindByIdAsync(currentUserLogin.UserId);
            return user;
        }

        private bool ClaimsAreEqual(Claim claim1, Claim claim2)
        {
            return claim1.Equals(claim2);
        }

        private bool UserLoginIsEqual(UserLoginInfo login1, UserLoginInfo login2)
        {
            return login1.LoginProvider == login2.LoginProvider && login1.ProviderKey == login2.ProviderKey;
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

        public async Task SetTwoFactorEnabledAsync(IdentityUser user, bool enabled)
        {
            user.TwoFactorEnabled = enabled;
            await Task.FromResult(0);
        }

        public async Task<bool> GetTwoFactorEnabledAsync(IdentityUser user)
        {
            return await Task.FromResult(user.TwoFactorEnabled);
        }

        public async Task<DateTimeOffset> GetLockoutEndDateAsync(IdentityUser user)
        {
            return await Task.FromResult(user.LockoutEndDateUtc);
        }

        public async Task SetLockoutEndDateAsync(IdentityUser user, DateTimeOffset lockoutEnd)
        {
            user.LockoutEndDateUtc = lockoutEnd;
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
    }
}