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
        private PizzaDatabase database;

        public UserStore(PizzaDatabase database)
        {
            this.database = database;
        }

        public Task CreateAsync(IdentityUser user)
        {
            user.Id = database.Insert(user.ToDbModel());
            return Task.FromResult(0);
        }

        public async Task AddToRoleAsync(IdentityUser user, string roleName)
        {
            List<SiteRole> siteRoles = await database.GetListAsync<SiteRole>(new { Name = roleName });
            SiteRole siteRole = siteRoles.FirstOrDefault();

            if (siteRole == null)
            {
                throw new ArgumentException("Invalid role name: roleName");
            }

            UserRole userRole = new UserRole();
            userRole.UserId = user.Id;
            userRole.RoleId = siteRole.Id;

            database.Insert(userRole);
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
            database.Dispose();
        }

        public async Task<IdentityUser> FindByIdAsync(int userId)
        {
            List<SiteUser> siteUsers = await database.GetListAsync<SiteUser>(new { Id = userId });
            SiteUser siteUser = siteUsers.FirstOrDefault();

            if (siteUser == null)
            {
                return null;
            }

            return new IdentityUser(siteUser);
        }

        public async Task<IdentityUser> FindByNameAsync(string userName)
        {
            List<SiteUser> siteUsers = await database.GetListAsync<SiteUser>(new { UserName = userName });
            SiteUser siteUser = siteUsers.FirstOrDefault();

            if (siteUser == null)
            {
                return null;
            }

            return new IdentityUser(siteUser);
        }

        public async Task<IList<string>> GetRolesAsync(IdentityUser user)
        {
            List<SiteRole> siteRolesTask = await database.GetListAsync<SiteRole>(null);
            List<UserRole> currentUserRolesTask = await database.GetListAsync<UserRole>(new { UserId = user.Id });
            IList<string> currentUserRoleNames = new List<string>();

            foreach (UserRole userRole in currentUserRolesTask)
            {
                var currentRole = siteRolesTask.Where(r => r.Id == userRole.RoleId).First();
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
            List<SiteRole> siteRoles = await database.GetListAsync<SiteRole>(new { Name = roleName });
            SiteRole currentRole = siteRoles.FirstOrDefault();

            if (currentRole == null)
            {
                throw new ArgumentException("Invalid role name: roleName");
            }

            UserRole currentUserRole = (await database.GetListAsync<UserRole>(new { UserId = user.Id, RoleId = currentRole.Id })).FirstOrDefault();

            if (currentUserRole != null)
            {
                database.Delete(currentUserRole);
            }
        }

        public Task UpdateAsync(IdentityUser user)
        {
            database.Update(user.ToDbModel());
            return Task.FromResult(0);
        }

        public Task SetPasswordHashAsync(IdentityUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(IdentityUser user)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(IdentityUser user)
        {
            return Task.FromResult(user.HasPassword());
        }

        public Task SetEmailAsync(IdentityUser user, string email)
        {
            user.Email = email;
            return Task.FromResult(0);
        }

        public Task<string> GetEmailAsync(IdentityUser user)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(IdentityUser user)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(IdentityUser user, bool confirmed)
        {
            user.EmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public async Task<IdentityUser> FindByEmailAsync(string email)
        {
            List<SiteUser> siteUsers = await database.GetListAsync<SiteUser>(new { Email = email });
            SiteUser siteUser = siteUsers.FirstOrDefault();

            if (siteUser == null)
            {
                return null;
            }

            return new IdentityUser(siteUser);
        }

        public async Task<IList<Claim>> GetClaimsAsync(IdentityUser user)
        {
            List<UserClaim> userClaims = await database.GetListAsync<UserClaim>(new { UserId = user.Id });
            IList<Claim> claims = userClaims.Select(uc => new Claim(uc.ClaimType, uc.ClaimValue)).ToList();
            return claims;
        }

        public Task AddClaimAsync(IdentityUser user, Claim claim)
        {
            UserClaim userClaim = new UserClaim()
            {
                UserId = user.Id,
                ClaimType = claim.Type,
                ClaimValue = claim.Value
            };
            database.Insert(userClaim);
            return Task.FromResult(0);
        }

        public async Task RemoveClaimAsync(IdentityUser user, Claim claim)
        {
            List<UserClaim> userClaims = await database.GetListAsync<UserClaim>(new { UserId = user.Id });
            UserClaim currentClaim = userClaims.Where(uc => uc.ClaimType == claim.Type && uc.ClaimValue == claim.Value).FirstOrDefault();

            if (currentClaim != null)
            {
                database.Delete(currentClaim);
            }
        }

        public Task AddLoginAsync(IdentityUser user, UserLoginInfo login)
        {
            UserLogin userLogin = new UserLogin()
            {
                UserId = user.Id,
                LoginProvider = login.LoginProvider,
                ProviderKey = login.ProviderKey
            };
            database.Insert(userLogin);
            return Task.FromResult(0);
        }

        public async Task RemoveLoginAsync(IdentityUser user, UserLoginInfo login)
        {
            List<UserLogin> userLogins = await database.GetListAsync<UserLogin>(new { UserId = user.Id, LoginProvider = login.LoginProvider, ProviderKey = login.ProviderKey });
            UserLogin currentUserLogin = userLogins.FirstOrDefault();

            if (currentUserLogin != null)
            {
                database.Delete(currentUserLogin);
            }
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(IdentityUser user)
        {
            List<UserLogin> userLogins = await database.GetListAsync<UserLogin>(new { UserId = user.Id });
            IList<UserLoginInfo> loginInfoList = userLogins.Select(ul => new UserLoginInfo(ul.LoginProvider, ul.ProviderKey)).ToList();
            return loginInfoList;
        }

        public async Task<IdentityUser> FindAsync(UserLoginInfo login)
        {
            List<UserLogin> userLogins = await database.GetListAsync<UserLogin>(new { LoginProvider = login.LoginProvider, ProviderKey = login.ProviderKey });
            UserLogin currentUserLogin = userLogins.FirstOrDefault();

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

        public Task SetSecurityStampAsync(IdentityUser user, string stamp)
        {
            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }

        public Task<string> GetSecurityStampAsync(IdentityUser user)
        {
            return Task.FromResult(user.SecurityStamp);
        }

        public Task SetPhoneNumberAsync(IdentityUser user, string phoneNumber)
        {
            user.PhoneNumber = phoneNumber;
            return Task.FromResult(0);
        }

        public Task<string> GetPhoneNumberAsync(IdentityUser user)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(IdentityUser user)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberConfirmedAsync(IdentityUser user, bool confirmed)
        {
            user.PhoneNumberConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task SetTwoFactorEnabledAsync(IdentityUser user, bool enabled)
        {
            user.TwoFactorEnabled = enabled;
            return Task.FromResult(0);
        }

        public Task<bool> GetTwoFactorEnabledAsync(IdentityUser user)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(IdentityUser user)
        {
            return Task.FromResult(user.LockoutEndDateUtc);
        }

        public Task SetLockoutEndDateAsync(IdentityUser user, DateTimeOffset lockoutEnd)
        {
            user.LockoutEndDateUtc = lockoutEnd;
            return Task.FromResult(0);
        }

        public Task<int> IncrementAccessFailedCountAsync(IdentityUser user)
        {
            int accessFailedCount = user.IncrementAccessFailedCount();
            return Task.FromResult(accessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(IdentityUser user)
        {
            user.ResetAccessFailedCount();
            return Task.FromResult(0);
        }

        public Task<int> GetAccessFailedCountAsync(IdentityUser user)
        {
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(IdentityUser user)
        {
            return Task.FromResult(user.LockoutEnabled);
        }

        public Task SetLockoutEnabledAsync(IdentityUser user, bool enabled)
        {
            user.LockoutEnabled = enabled;
            return Task.FromResult(0);
        }
    }
}