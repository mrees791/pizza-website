using Microsoft.AspNet.Identity;
using PizzaWebsite.Models.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace PizzaWebsite.Models.Identity
{
    public class UserStore :
        IUserStore<SiteUser, int>,
        IUserPasswordStore<SiteUser, int>,
        IUserEmailStore<SiteUser, int>,
        IUserRoleStore<SiteUser, int>,
        IUserClaimStore<SiteUser, int>,
        IUserLoginStore<SiteUser, int>,
        IUserSecurityStampStore<SiteUser, int>,
        IUserPhoneNumberStore<SiteUser, int>,
        IUserTwoFactorStore<SiteUser, int>,
        IUserLockoutStore<SiteUser, int>
    {
        // Dummy database serves as a test before DAL implementation
        private DummyDatabase dbContext;

        public UserStore()
        {
            dbContext = new DummyDatabase();
            CreateDummyDatabaseRecords();
        }

        private void CreateDummyDatabaseRecords()
        {
            // Add users
            CreateAsync(new SiteUser("basic_user", "PASSHASH1234", "user@gmail.com"));
            CreateAsync(new SiteUser("manager_user", "PASSHASH5678", "manager@gmail.com"));

            // Add user to manager role
            SiteUser managerUser = FindByNameAsync("manager_user").Result;
            AddToRoleAsync(managerUser, "Manager");
        }

        public Task AddToRoleAsync(SiteUser user, string roleName)
        {
            List<SiteRole> roles = dbContext.LoadRoles();
            SiteRole siteRole = roles.Where(r => r.Name == roleName).First();
            UserRole userRole = new UserRole(user.Id, siteRole.Id);

            // Add UserRole record to dummy database
            dbContext.AddRecord(userRole);

            return Task.FromResult(0);
        }

        public Task CreateAsync(SiteUser user)
        {
            // Add SiteUser to dummy database
            dbContext.AddRecord(user);

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
            List<SiteUser> users = dbContext.LoadUsers();
            return Task.FromResult(users.Where(u => u.Id == userId).First());
        }

        public Task<SiteUser> FindByNameAsync(string userName)
        {
            List<SiteUser> users = dbContext.LoadUsers();
            return Task.FromResult(users.Where(u => u.UserName == userName).First());
        }

        public Task<IList<string>> GetRolesAsync(SiteUser user)
        {
            List<SiteRole> roles = dbContext.LoadRoles();
            List<UserRole> userRoles = dbContext.LoadUserRoles();
            IList<string> currentUserRoleNames = new List<string>();
            List<UserRole> currentUserRoles = userRoles.Where(ur => ur.UserId == user.Id).ToList();

            foreach (UserRole userRole in currentUserRoles)
            {
                SiteRole role = roles.Where(r => r.Id == userRole.RoleId).First();
                currentUserRoleNames.Add(role.Name);
            }

            return Task.FromResult(currentUserRoleNames);
        }

        public Task<bool> IsInRoleAsync(SiteUser user, string roleName)
        {
            IList<string> currentUserRoles = GetRolesAsync(user).Result;

            return Task.FromResult(currentUserRoles.Contains(roleName));
        }

        public Task RemoveFromRoleAsync(SiteUser user, string roleName)
        {
            List<SiteRole> roles = dbContext.LoadRoles();
            List<UserRole> userRoles = dbContext.LoadUserRoles();
            SiteRole role = roles.Where(r => r.Name == roleName).First();
            UserRole userRole = userRoles.
                Where(ur => ur.RoleId == role.Id && ur.UserId == user.Id).First();

            dbContext.DeleteRecord(userRole);

            return Task.FromResult(0);
        }

        public Task UpdateAsync(SiteUser user)
        {
            // Update user records in database
            return Task.FromResult(0);
        }

        public Task SetPasswordHashAsync(SiteUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(SiteUser user)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(SiteUser user)
        {
            return Task.FromResult(user.HasPassword());
        }

        public Task SetEmailAsync(SiteUser user, string email)
        {
            user.Email = email;
            return Task.FromResult(0);
        }

        public Task<string> GetEmailAsync(SiteUser user)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(SiteUser user)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(SiteUser user, bool confirmed)
        {
            user.EmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task<SiteUser> FindByEmailAsync(string email)
        {
            List<SiteUser> users = dbContext.LoadUsers();
            return Task.FromResult(users.Where(u => u.Email == email).First());
        }

        public Task<IList<Claim>> GetClaimsAsync(SiteUser user)
        {
            List<UserClaim> userClaims = dbContext.LoadUserClaims().Where(uc => uc.UserId == user.Id).ToList();
            IList<Claim> claims = userClaims.Select(uc => uc.Claim).ToList();

            return Task.FromResult(claims);
        }

        public Task AddClaimAsync(SiteUser user, Claim claim)
        {
            UserClaim userClaim = new UserClaim(user.Id, claim);
            dbContext.AddRecord(userClaim);

            return Task.FromResult(0);
        }

        public Task RemoveClaimAsync(SiteUser user, Claim claim)
        {
            List<UserClaim> userClaims = dbContext.LoadUserClaims().Where(uc => uc.UserId == user.Id).ToList();
            UserClaim currentClaim = userClaims.Where(uc => ClaimsAreEqual(uc.Claim, claim)).First();
            dbContext.DeleteRecord(currentClaim);

            return Task.FromResult(0);
        }

        public Task AddLoginAsync(SiteUser user, UserLoginInfo login)
        {
            UserLogin userLogin = new UserLogin(user.Id, login);
            dbContext.AddRecord(userLogin);

            return Task.FromResult(0);
        }

        public Task RemoveLoginAsync(SiteUser user, UserLoginInfo login)
        {
            UserLogin userLoginRecord = dbContext.LoadUserLogins().Where(ul => UserLoginIsEqual(ul.UserLoginInfo, login)).First();
            dbContext.DeleteRecord(userLoginRecord);

            return Task.FromResult(0);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(SiteUser user)
        {
            List<UserLogin> userLoginRecords = dbContext.LoadUserLogins().Where(ul => ul.UserId == user.Id).ToList();
            IList<UserLoginInfo> loginInfo = userLoginRecords.Select(ul => ul.UserLoginInfo).ToList();

            return Task.FromResult(loginInfo);
        }

        public Task<SiteUser> FindAsync(UserLoginInfo login)
        {
            List<UserLogin> userLogins = dbContext.LoadUserLogins();
            List<SiteUser> users = dbContext.LoadUsers();
            UserLogin userLogin = userLogins.Where(ul => UserLoginIsEqual(ul.UserLoginInfo, login)).First();

            return Task.FromResult(users.Where(u => u.Id == userLogin.UserId).First());
        }

        private bool ClaimsAreEqual(Claim claim1, Claim claim2)
        {
            return claim1.Type == claim2.Type && claim1.Value == claim2.Value;
        }

        private bool UserLoginIsEqual(UserLoginInfo login1, UserLoginInfo login2)
        {
            return login1.LoginProvider == login2.LoginProvider && login1.ProviderKey == login2.ProviderKey;
        }

        public Task SetSecurityStampAsync(SiteUser user, string stamp)
        {
            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }

        public Task<string> GetSecurityStampAsync(SiteUser user)
        {
            return Task.FromResult(user.SecurityStamp);
        }

        public Task SetPhoneNumberAsync(SiteUser user, string phoneNumber)
        {
            user.PhoneNumber = phoneNumber;
            return Task.FromResult(0);
        }

        public Task<string> GetPhoneNumberAsync(SiteUser user)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(SiteUser user)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberConfirmedAsync(SiteUser user, bool confirmed)
        {
            user.PhoneNumberConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task SetTwoFactorEnabledAsync(SiteUser user, bool enabled)
        {
            user.TwoFactorEnabled = enabled;
            return Task.FromResult(0);
        }

        public Task<bool> GetTwoFactorEnabledAsync(SiteUser user)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(SiteUser user)
        {
            return Task.FromResult(user.LockoutEndDate);
        }

        public Task SetLockoutEndDateAsync(SiteUser user, DateTimeOffset lockoutEnd)
        {
            user.LockoutEndDate = lockoutEnd;
            return Task.FromResult(0);
        }

        public Task<int> IncrementAccessFailedCountAsync(SiteUser user)
        {
            int accessFailedCount = user.IncrementAccessFailedCount();
            return Task.FromResult(accessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(SiteUser user)
        {
            user.ResetAccessFailedCount();
            return Task.FromResult(0);
        }

        public Task<int> GetAccessFailedCountAsync(SiteUser user)
        {
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(SiteUser user)
        {
            return Task.FromResult(user.LockoutEnabled);
        }

        public Task SetLockoutEnabledAsync(SiteUser user, bool enabled)
        {
            user.LockoutEnabled = enabled;
            return Task.FromResult(0);
        }
    }
}