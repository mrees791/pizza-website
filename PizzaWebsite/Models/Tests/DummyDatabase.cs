using PizzaWebsite.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.Tests
{
    // Used to test UserStore functionality before DAL implementation
    public class DummyDatabase
    {
        // User, role definitions
        private List<SiteRole> roles;
        private List<SiteUser> users;

        // Links users to roles, claims, external login APIs
        private List<UserRole> userRoles;
        private List<UserClaim> userClaims;
        private List<UserLogin> userLogins;

        public DummyDatabase()
        {
            users = new List<SiteUser>();
            roles = new List<SiteRole>();
            userRoles = new List<UserRole>();
            userClaims = new List<UserClaim>();
            userLogins = new List<UserLogin>();

            // Add records to dummy database
            SiteRole managerRole = new SiteRole("Manager");
            AddRecord(managerRole);
        }

        // CRUD
        public void AddRecord(SiteUser user)
        {
            users.Add(user);
            user.Id = users.Count;
        }

        public void AddRecord(SiteRole role)
        {
            roles.Add(role);
            role.Id = roles.Count;
        }

        public void AddRecord(UserRole userRole)
        {
            userRoles.Add(userRole);
            userRole.Id = userRoles.Count;
        }

        public void AddRecord(UserClaim userClaim)
        {
            userClaims.Add(userClaim);
            userClaim.Id = userClaims.Count;
        }

        public void AddRecord(UserLogin userLogin)
        {
            userLogins.Add(userLogin);
            userLogin.Id = userLogins.Count;
        }

        public void DeleteRecord(UserRole userRole)
        {
            userRoles.Remove(userRole);
        }

        // Load methods in an actual database context would run a query to get updated records.
        public List<SiteRole> LoadRoles()
        {
            return new List<SiteRole>(roles);
        }

        public List<SiteUser> LoadUsers()
        {
            return new List<SiteUser>(users);
        }

        public List<UserRole> LoadUserRoles()
        {
            return new List<UserRole>(userRoles);
        }

        public List<UserClaim> LoadUserClaims()
        {
            return new List<UserClaim>(userClaims);
        }

        public List<UserLogin> LoadUserLogins()
        {
            return new List<UserLogin>(userLogins);
        }
    }
}