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
using PizzaWebsite.Models.Tests;
using System.Net.Mail;
using System.Net;
using System.Diagnostics;
using System.Configuration;

namespace PizzaWebsite.Controllers
{
    public class HomeController : Controller
    {
        private DummyDatabase dbContext;
        private UserManager<SiteUser, int> userManager;

        public HomeController()
        {
            dbContext = new DummyDatabase();
            UserStore userStore = new UserStore();
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

                // Confirm email

                // Add user records if successful.
                IdentityResult result = userManager.Create(newUser, registerVm.Password);

                if (result.Succeeded)
                {
                    SignInUser(newUser);
                    return RedirectToAction(nameof(UserProfile));
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
        /// Signs in a user with a claims identity after they have already been authenticated.
        /// </summary>
        /// <param name="user"></param>
        private void SignInUser(SiteUser user)
        {
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
            ClaimsIdentity userIdentity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
            authenticationManager.SignIn(new AuthenticationProperties() { }, userIdentity);
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
            int additionalId = dbContext.GetNumberOfUsers();
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

            if (User.Identity.IsAuthenticated)
            {
                string userName = User.Identity.GetUserName();
                registerVm.UserIsSignedIn = true;
                registerVm.AlreadySignedInMessage = $"You are already signed in as {userName}.";
            }

            return View(registerVm);
        }

        public ActionResult UserProfile()
        {
            UserProfileViewModel userProfileVm = new UserProfileViewModel();

            if (User.Identity.IsAuthenticated)
            {
                userProfileVm.UserIsSignedIn = true;
                string userName = User.Identity.GetUserName();
                SiteUser user = userManager.FindByNameAsync(userName).Result;
                IList<string> roles = userManager.GetRolesAsync(user.Id).Result;
                IList<Claim> claims = userManager.GetClaimsAsync(user.Id).Result;

                userProfileVm.Message1 = $"Signed in as {userName}";
                userProfileVm.Message1 += $", Email: {user.Email}";
                userProfileVm.Message1 += $", Roles: ";
                foreach (string role in roles)
                {
                    userProfileVm.Message1 += $" {role} ";
                }
                userProfileVm.Message1 += $", Claims: ";
                foreach (Claim claim in claims)
                {
                    userProfileVm.Message1 += $" ({claim.Issuer}:{claim.Type},{claim.Value}) ";
                }
            }

            return View(userProfileVm);
        }

        public ActionResult SignOut()
        {
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SignIn(SignInViewModel signInVm)
        {
            if (ModelState.IsValid)
            {
                SiteUser user = userManager.Find(signInVm.UserName, signInVm.Password);

                if (user != null)
                {
                    SignInUser(user);
                    return RedirectToAction(nameof(UserProfile));
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            return View(signInVm);
        }

        [AllowAnonymous]
        public ActionResult SignIn()
        {
            SignInViewModel signInVm = new SignInViewModel();

            if (User.Identity.IsAuthenticated)
            {
                string userName = User.Identity.GetUserName();
                signInVm.UserIsSignedIn = true;
                signInVm.AlreadySignedInMessage = $"You are already signed in as {userName}.";
            }

            return View(signInVm);
        }

        // todo: Remove test authorization code.
        [Authorize(Roles = "Manager")]
        public ActionResult ManageSite()
        {
            return View();
        }
    }
}