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
        IUserClaimStore<SiteUser, int>
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
            UserClaim currentClaim = userClaims.Where(uc => uc.Claim == claim).First();
            dbContext.DeleteRecord(currentClaim);

            return Task.FromResult(0);
        }
    }
}