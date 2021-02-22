using Microsoft.VisualStudio.TestTools.UnitTesting;
using PizzaWebsite.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaWebsite.Tests
{
    [TestClass]
    public class UserStoreTests
    {
        /*private UserStore userStore;

        [ClassInitialize]
        public static void TestUserStore()
        {
            userStore = new UserStore();
        }

        [TestMethod]
        public void TestCreateAsync()
        {
            userStore.CreateAsync(new SiteUser("basic_user", "PASSHASH1234", "user@gmail.com"));
            userStore.CreateAsync(new SiteUser("manager_user", "PASSHASH5678", "manager@gmail.com"));

            List<SiteUser> users = userStore.DbContext.LoadUsers();

            Assert.AreEqual(1, users[0].Id, "First user ID should be 1.");
            Assert.AreEqual(2, users[1].Id, "Second user ID should be 2.");
            Assert.AreEqual(2, users.Count, "There should be 2 users.");
        }

        [TestMethod]
        public void TestFindByNameAsync()
        {
            SiteUser basicUser = userStore.FindByNameAsync("basic_user").Result;
            SiteUser managerUser = userStore.FindByNameAsync("manager_user").Result;

            Assert.IsNotNull(basicUser);
            Assert.IsNotNull(managerUser);
            Assert.AreEqual(1, basicUser.Id);
            Assert.AreEqual(2, managerUser.Id);
            Assert.AreEqual("basic_user", basicUser.UserName, "basicUser's UserName property should be basic_user.");
            Assert.AreEqual("manager_user", managerUser.UserName, "managerUser's UserName property should be manager_user.");
        }

        /*[TestMethod]
        public void TestAddToRoleAsync()
        {
            // Add user to manager role
            SiteUser managerUser = userStore.FindByNameAsync("manager_user").Result;
            userStore.AddToRoleAsync(managerUser, "Manager");

            List<UserRole> userRoles = userStore.DbContext.LoadUserRoles();
            UserRole managerUserRole = userRoles[0];

            Assert.AreEqual()
        }*/
    }
}
