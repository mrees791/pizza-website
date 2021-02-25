﻿using Microsoft.AspNet.Identity;
using PizzaWebsite2.Models.Databases;
using PizzaWebsite2.Models.Databases.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace PizzaWebsite2.Models.Identity
{
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
        private DummyDatabase dbContext;

        public UserStore()
        {
            dbContext = new DummyDatabase();
        }
        public Task CreateAsync(IdentityUser user)
        {
            dbContext.AddRecord(user);
            return Task.FromResult(0);
        }

        public Task AddToRoleAsync(IdentityUser user, string roleName)
        {
            List<IdentityRole> roles = dbContext.LoadRoles();
            IdentityRole IdentityRole = roles.Where(r => r.Name == roleName).FirstOrDefault();

            if (IdentityRole != null)
            {
                UserRole userRole = new UserRole(user.Id, IdentityRole.Id);
                dbContext.AddRecord(userRole);

                return Task.FromResult(0);
            }

            throw new ArgumentException("Invalid role name: roleName");
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
        }

        public Task<IdentityUser> FindByIdAsync(int userId)
        {
            List<IdentityUser> users = dbContext.LoadUsers();
            IdentityUser user = users.Where(u => u.Id == userId).FirstOrDefault();

            if (user != null)
            {
                return Task.FromResult(user);
            }

            return Task.FromResult<IdentityUser>(null);
        }

        public Task<IdentityUser> FindByNameAsync(string userName)
        {
            List<IdentityUser> users = dbContext.LoadUsers();
            IdentityUser user = users.Where(u => u.UserName == userName).FirstOrDefault();

            if (user != null)
            {
                return Task.FromResult(user);
            }

            return Task.FromResult<IdentityUser>(null);
        }

        public Task<IList<string>> GetRolesAsync(IdentityUser user)
        {
            List<IdentityRole> roles = dbContext.LoadRoles();
            List<UserRole> userRoles = dbContext.LoadUserRoles();
            IList<string> currentUserRoleNames = new List<string>();
            List<UserRole> currentUserRoles = userRoles.Where(ur => ur.UserId == user.Id).ToList();

            foreach (UserRole userRole in currentUserRoles)
            {
                IdentityRole role = roles.Where(r => r.Id == userRole.RoleId).First();
                currentUserRoleNames.Add(role.Name);
            }

            return Task.FromResult(currentUserRoleNames);
        }

        public Task<bool> IsInRoleAsync(IdentityUser user, string roleName)
        {
            IList<string> currentUserRoles = GetRolesAsync(user).Result;
            bool isInRole = currentUserRoles.Contains(roleName);
            return Task.FromResult(isInRole);
        }

        public Task RemoveFromRoleAsync(IdentityUser user, string roleName)
        {
            List<IdentityRole> roles = dbContext.LoadRoles();
            List<UserRole> userRoles = dbContext.LoadUserRoles();
            IdentityRole role = roles.Where(r => r.Name == roleName).FirstOrDefault();

            if (role != null)
            {
                UserRole userRole = userRoles.Where(ur => ur.RoleId == role.Id && ur.UserId == user.Id).FirstOrDefault();

                if (userRole != null)
                {
                    dbContext.DeleteRecord(userRole);
                }

                return Task.FromResult(0);
            }

            throw new ArgumentException("Invalid role name: roleName");
        }

        public Task UpdateAsync(IdentityUser user)
        {
            // Update user records in database.
            throw new NotImplementedException();
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

        public Task<IdentityUser> FindByEmailAsync(string email)
        {
            List<IdentityUser> users = dbContext.LoadUsers();
            IdentityUser user = users.Where(u => u.Email == email).FirstOrDefault();

            if (user != null)
            {
                return Task.FromResult(user);
            }

            return Task.FromResult<IdentityUser>(null);
        }

        public Task<IdentityUser> FindByPhoneNumberAsync(string phoneNumber)
        {
            List<IdentityUser> users = dbContext.LoadUsers();
            IdentityUser user = users.Where(u => u.PhoneNumber == phoneNumber).FirstOrDefault();

            if (user != null)
            {
                return Task.FromResult(user);
            }

            return Task.FromResult<IdentityUser>(null);
        }

        public Task<IList<Claim>> GetClaimsAsync(IdentityUser user)
        {
            List<UserClaim> userClaims = dbContext.LoadUserClaims().Where(uc => uc.UserId == user.Id).ToList();
            IList<Claim> claims = userClaims.Select(uc => uc.Claim).ToList();
            return Task.FromResult(claims);
        }

        public Task AddClaimAsync(IdentityUser user, Claim claim)
        {
            UserClaim userClaim = new UserClaim(user.Id, claim);
            dbContext.AddRecord(userClaim);
            return Task.FromResult(0);
        }

        public Task RemoveClaimAsync(IdentityUser user, Claim claim)
        {
            List<UserClaim> userClaims = dbContext.LoadUserClaims().Where(uc => uc.UserId == user.Id).ToList();
            UserClaim currentClaim = userClaims.Where(uc => ClaimsAreEqual(uc.Claim, claim)).FirstOrDefault();

            if (currentClaim != null)
            {
                dbContext.DeleteRecord(currentClaim);
            }

            return Task.FromResult(0);
        }

        public Task AddLoginAsync(IdentityUser user, UserLoginInfo login)
        {
            UserLogin userLogin = new UserLogin(user.Id, login);
            dbContext.AddRecord(userLogin);
            return Task.FromResult(0);
        }

        public Task RemoveLoginAsync(IdentityUser user, UserLoginInfo login)
        {
            List<UserLogin> userLogins = dbContext.LoadUserLogins();
            UserLogin userLogin = userLogins.Where(ul => UserLoginIsEqual(ul.UserLoginInfo, login)).FirstOrDefault();

            if (userLogin != null)
            {
                dbContext.DeleteRecord(userLogin);
            }

            return Task.FromResult(0);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(IdentityUser user)
        {
            List<UserLogin> userLogins = dbContext.LoadUserLogins();
            IList<UserLoginInfo> loginInfo = userLogins.Where(ul => ul.UserId == user.Id).Select(ul => ul.UserLoginInfo).ToList();

            return Task.FromResult(loginInfo);
        }

        public Task<IdentityUser> FindAsync(UserLoginInfo login)
        {
            List<IdentityUser> users = dbContext.LoadUsers();
            List<UserLogin> userLogins = dbContext.LoadUserLogins();
            UserLogin userLogin = userLogins.Where(ul => UserLoginIsEqual(ul.UserLoginInfo, login)).FirstOrDefault();

            if (userLogin != null)
            {
                IdentityUser user = users.Where(u => u.Id == userLogin.UserId).FirstOrDefault();

                if (user != null)
                {
                    return Task.FromResult(user);
                }
            }

            return Task.FromResult<IdentityUser>(null);
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
            return Task.FromResult(user.LockoutEndDate);
        }

        public Task SetLockoutEndDateAsync(IdentityUser user, DateTimeOffset lockoutEnd)
        {
            user.LockoutEndDate = lockoutEnd;
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
        public DummyDatabase DbContext { get => dbContext; }
    }
}