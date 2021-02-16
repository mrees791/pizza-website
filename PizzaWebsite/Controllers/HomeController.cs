using DataLibrary.BusinessLogic.Users;
using Microsoft.AspNet.Identity;
using PizzaWebsite.Models.Identity;
using PizzaWebsite.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PizzaWebsite.Controllers
{
    public class HomeController : Controller
    {
        private UserStoreModel userStore;
        private UserManager<IdentityUserModel> userManager;

        public HomeController()
        {
            userStore = new UserStoreModel();
            userManager = new UserManager<IdentityUserModel>(userStore);
            userManager.UserValidator = new Models.Identity.Validators.UserValidatorModel();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        private IdentityResult CreateUser(RegisterUserModel userRegistration)
        {
            var user = new IdentityUserModel()
            {
                UserName = userRegistration.UserName,
                Email = userRegistration.Email,
                PhoneNumber = userRegistration.PhoneNumber,
                ZipCode = userRegistration.ZipCode
            };
            return userManager.CreateAsync(user, userRegistration.Password).Result;
        }

        [HttpPost]
        public  ActionResult Register(RegisterUserModel userRegistration)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = CreateUser(userRegistration);

                if (result.Succeeded)
                {
                    // Needs implemented.
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        string key = GetUserRegistrationErrorKey(error);
                        ModelState.AddModelError(key, error);
                    }
                }
            }
            return View(userRegistration);
        }

        private string GetUserRegistrationErrorKey(string error)
        {
            if (error.StartsWith("Name"))
            {
                return "UserName";
            }
            else if (error.StartsWith("Email"))
            {
                return "Email";
            }

            return "";
        }

        // todo: Remove test user code
        private RegisterUserModel CreateTestRegistration()
        {
            int additionalId = DatabaseUserProcessor.GetNumberOfUsers() + 10;

            RegisterUserModel testUser = new RegisterUserModel()
            {
                UserName = $"mrees123",
                Email = $"mrees123@gmail.com",
                ConfirmEmail = $"mrees123@gmail.com",
                Password = "abacus12345",
                ConfirmPassword = "abacus12345",
                PhoneNumber = "7402609777",
                ZipCode = "12345"
            };

            return testUser;
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            RegisterUserModel registerUser = CreateTestRegistration();
            return View(registerUser);
        }
    }
}