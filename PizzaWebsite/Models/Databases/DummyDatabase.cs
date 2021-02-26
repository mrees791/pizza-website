using PizzaWebsite.Models.Databases.Users;
using PizzaWebsite.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.Databases
{
    // Dummy database serves as a test before DAL implementation
    public class DummyDatabase : IDisposable
    {
        // User, role definitions
        private static List<IdentityRole> roles = new List<IdentityRole>();
        private static List<IdentityUser> users = new List<IdentityUser>();

        // Links users to roles, claims, external login APIs
        private static List<UserRole> userRoles = new List<UserRole>();
        private static List<UserClaim> userClaims = new List<UserClaim>();
        private static List<UserLogin> userLogins = new List<UserLogin>();

        static DummyDatabase()
        {
            // Add records to dummy database
            IdentityRole managerRole = new IdentityRole("Manager");
            roles.Add(managerRole);
            managerRole.Id = roles.Count;
        }

        public DummyDatabase()
        {
        }

        public int GetNumberOfUsers()
        {
            return users.Count;
        }

        // CRUD
        public void AddRecord(IdentityUser user)
        {
            users.Add(user);
            user.Id = users.Count;
        }

        public void AddRecord(IdentityRole role)
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

        public void DeleteRecord(UserClaim userClaim)
        {
            userClaims.Remove(userClaim);
        }

        public void DeleteRecord(UserLogin userLogin)
        {
            userLogins.Remove(userLogin);
        }

        // Load methods in an actual database context would run a query to get updated records.
        public List<IdentityRole> LoadRoles()
        {
            return new List<IdentityRole>(roles);
        }

        public List<IdentityUser> LoadUsers()
        {
            return new List<IdentityUser>(users);
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

        public void Dispose()
        {
            // todo: Dispose database resources.
        }
    }
}