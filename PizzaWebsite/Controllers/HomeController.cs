using Microsoft.AspNet.Identity;
using PizzaWebsite.Models.Identity;
using PizzaWebsite.Models.Identity.Validators;
using PizzaWebsite.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using System.Security.Claims;
using PizzaWebsite.ViewModels.Tests;

namespace PizzaWebsite.Controllers
{
    public class HomeController : Controller
    {
        private UserStore userStore;
        private UserManager<SiteUser, int> userManager;

        public HomeController()
        {
            userStore = new UserStore();
            userManager = new UserManager<SiteUser, int>(userStore);
            userManager.UserValidator = new UserValidator(userStore);
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

        [HttpPost]
        public ActionResult Register(RegisterViewModel registerVm)
        {
            if (ModelState.IsValid)
            {
                SiteUser newUser = new SiteUser()
                {
                    UserName = registerVm.UserName,
                    Email = registerVm.Email,
                    PhoneNumber = registerVm.PhoneNumber,
                    ZipCode = registerVm.ZipCode
                };

                IdentityResult result = userManager.Create(newUser, registerVm.Password);

                if (result.Succeeded)
                {
                    newUser = userStore.FindByNameAsync(newUser.UserName).Result;

                    // Testing roles and claims
                    userStore.AddToRoleAsync(newUser, "Manager");
                    userStore.AddClaimAsync(newUser, new Claim("myClaimType", "myClaimValue"));

                    // Needs grouped together in method.
                    IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
                    ClaimsIdentity userIdentity = userManager.CreateIdentity(newUser, DefaultAuthenticationTypes.ApplicationCookie);
                    authenticationManager.SignIn(new AuthenticationProperties() { }, userIdentity);
                    return RedirectToAction("Index", "Home");
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
            return View(registerVm);
        }

        /// <summary>
        /// Takes an error message created by the UserValidatorModel and returns the key for the model state error.
        /// </summary>
        /// <param name="error"></param>
        /// <returns>The key for the model state error.</returns>
        private string GetUserRegistrationErrorKey(string error)
        {
            string lowerCaseError = error.ToLower();

            if (lowerCaseError.StartsWith("name"))
            {
                return "UserName";
            }
            else if (lowerCaseError.StartsWith("email"))
            {
                return "Email";
            }
            else if (lowerCaseError.StartsWith("phone number"))
            {
                return "PhoneNumber";
            }

            return "";
        }

        // todo: Remove test user code
        private RegisterViewModel CreateTestRegistration()
        {
            int additionalId = userStore.DbContext.GetNumberOfUsers();
            long pn = 7402609777 + additionalId;

            RegisterViewModel testUser = new RegisterViewModel()
            {
                UserName = $"mrees{additionalId}",
                Email = $"mrees{additionalId}@gmail.com",
                ConfirmEmail = $"mrees{additionalId}@gmail.com",
                Password = "abacus12345",
                ConfirmPassword = "abacus12345",
                PhoneNumber = pn.ToString(),
                ZipCode = "12345"
            };

            return testUser;
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            RegisterViewModel registerVm = CreateTestRegistration();
            return View(registerVm);
        }

        public ActionResult TestUser()
        {
            TestUserViewModel testUserVm = new TestUserViewModel();
            testUserVm.Message1 = "Not signed in.";

            if (User.Identity.IsAuthenticated)
            {
                string userName = User.Identity.GetUserName();
                SiteUser user = userStore.FindByNameAsync(userName).Result;

                testUserVm.Message1 = $"Signed in as {userName}";
                testUserVm.Message1 += $"Email: {user.Email}";
            }

            return View(testUserVm);
        }

        public ActionResult SignOut()
        {
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut();
            return RedirectToAction("Index");
        }
    }
}