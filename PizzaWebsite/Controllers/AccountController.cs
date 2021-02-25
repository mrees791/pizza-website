using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PizzaWebsite.Models.Identity;
using PizzaWebsite.Models.Tests;
using PizzaWebsite.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PizzaWebsite.Controllers
{
    [RequireHttps]
    public class AccountController : Controller
    {
        private DummyDatabase dbContext;
        private UserManager<SiteUser, int> userManager;
        private SignInManager<SiteUser, int> signInManager;

        public AccountController()
        {

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel registerVm)
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

                IdentityResult result = await userManager.CreateAsync(newUser, registerVm.Password);

                if (result.Succeeded)
                {
                    SignInManager<SiteUser, int> signInManager = CreateSignInManager();

                    await signInManager.SignInAsync(newUser, false, false);

                    // Good email confirmation code
                    /*string code = await userManager.GenerateEmailConfirmationTokenAsync(newUser.Id);
                    string callbackUrl = Url.Action("ConfirmEmail", "Account",
                        new
                        {
                            userId = newUser.Id,
                            code = code
                        },
                        Request.Url.Scheme);
                    await userManager.SendEmailAsync(newUser.Id,
                        "Confirm Your Little Brutus Account",
                        $"Confirm your account by clicking <a href=\"{callbackUrl}\">here</a>");

                    InfoViewModel infoVm = new InfoViewModel()
                    {
                        Message = "Check your email and confirm your account. You must be confirmed before you can log in."
                    };

                    return RedirectToAction("Info", infoVm);*/
                    return RedirectToAction(nameof(Profile));
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

        public ActionResult Info(InfoViewModel infoVm)
        {
            return View(infoVm);
        }

        [HttpGet]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            int id = 0;
            bool validId = int.TryParse(userId, out id);

            if (!validId || userId == null || code == null)
            {
                return View("Error");
            }

            var result = await userManager.ConfirmEmailAsync(id, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        private SignInManager<SiteUser, int> CreateSignInManager()
        {
            SignInManager<SiteUser, int> signInManager = new SignInManager<SiteUser, int>(userManager, AuthenticationManager);

            return signInManager;
        }

        /// <summary>
        /// Signs in a user with a claims identity after they have already been authenticated.
        /// </summary>
        /// <param name="user"></param>
        /*private void SignInUser(SiteUser user)
        {
            ClaimsIdentity userIdentity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
            authenticationManager.SignIn(new AuthenticationProperties() { }, userIdentity);
        }*/

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
                Email = $"acct@littlebrutus.ddns.net",
                ConfirmEmail = $"acct@littlebrutus.ddns.net",
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
                userProfileVm.Message1 += $", Email Confirmed: {user.EmailConfirmed}";
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
            AuthenticationManager.SignOut();
            return RedirectToAction("Index");
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SignIn(SignInViewModel signInVm)
        {
            signInVm.LoginProviders.AddRange(AuthenticationManager.GetExternalAuthenticationTypes());

            if (ModelState.IsValid)
            {
                // Require the user to have a confirmed email before they can sign in.

                // Authenticate username and password
                SiteUser user = userManager.Find(signInVm.UserName, signInVm.Password);

                if (user != null)
                {
                    SignInManager<SiteUser, int> signInManager = CreateSignInManager();
                    await signInManager.SignInAsync(user, false, false);

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
        public ActionResult SignIn(string returnUrl)
        {
            SignInViewModel signInVm = new SignInViewModel();

            if (User.Identity.IsAuthenticated)
            {
                string userName = User.Identity.GetUserName();
                signInVm.UserIsSignedIn = true;
                signInVm.AlreadySignedInMessage = $"You are already signed in as {userName}.";
            }
            else
            {
                signInVm.ReturnUrl = returnUrl;
                signInVm.LoginProviders.AddRange(AuthenticationManager.GetExternalAuthenticationTypes());
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