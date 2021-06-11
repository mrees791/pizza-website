﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DataLibrary.Models;
using DataLibrary.Models.Exceptions;
using DataLibrary.Models.QuerySearches;
using DataLibrary.Models.Tables;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PizzaWebsite.Controllers.BaseControllers;
using PizzaWebsite.Models;
using PizzaWebsite.Models.Identity;
using PizzaWebsite.Models.Manage;

namespace PizzaWebsite.Controllers
{
    [Authorize]
    public class ManageController : BaseController
    {
        public ManageController()
        {
        }

        public async Task<ActionResult> ManageAddresses()
        {
            DeliveryAddressSearch addressSearch = new DeliveryAddressSearch()
            {
                UserId = User.Identity.GetUserId()
            };

            List<DeliveryAddressViewModel> addressVmList = new List<DeliveryAddressViewModel>();
            IEnumerable<DeliveryAddress> addressList = await PizzaDb.GetListAsync<DeliveryAddress>("Name", SortOrder.Ascending, addressSearch);

            foreach (DeliveryAddress address in addressList)
            {
                DeliveryAddressViewModel addressVm = new DeliveryAddressViewModel()
                {
                    Id = address.Id,
                    Name = address.Name,
                    AddressType = address.AddressType,
                    StreetAddress = address.StreetAddress,
                    City = address.City,
                    State = address.State,
                    ZipCode = address.ZipCode,
                    PhoneNumber = address.PhoneNumber,
                    DeleteButtonId = $"delete-btn-{address.Id}",
                    AddressRowId = $"address-row-{address.Id}"
                };

                addressVmList.Add(addressVm);
            }

            ManageAddressesViewModel viewModel = new ManageAddressesViewModel()
            {
                AddressVmList = addressVmList
            };

            return View(viewModel);
        }

        public ActionResult AddNewAddress()
        {
            return View("ManageDeliveryAddress", new ManageDeliveryAddressViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitDeliveryAddress(ManageDeliveryAddressViewModel addressVm)
        {
            if (!ModelState.IsValid)
            {
                return View("ManageDeliveryAddress", addressVm);
            }

            DeliveryAddress address = new DeliveryAddress()
            {
                Id = addressVm.Id,
                UserId = User.Identity.GetUserId(),
                Name = addressVm.Name,
                AddressType = addressVm.SelectedAddressType,
                City = addressVm.City,
                PhoneNumber = addressVm.PhoneNumber,
                State = addressVm.SelectedState,
                StreetAddress = addressVm.StreetAddress,
                ZipCode = addressVm.ZipCode
            };

            if (addressVm.IsNewRecord())
            {
                await PizzaDb.InsertAsync(address);
            }
            else
            {
                bool authorized = await PizzaDb.Commands.UserOwnsDeliveryAddressAsync(User.Identity.GetUserId(), address);

                if (!authorized)
                {
                    return NotAuthorizedToModifyAddressErrorMessage();
                }

                await PizzaDb.UpdateAsync(address);
            }

            return RedirectToAction("ManageAddresses");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ModifyDeliveryAddress(int addressId)
        {
            DeliveryAddress address = await PizzaDb.GetAsync<DeliveryAddress>(addressId);

            if (address == null)
            {
                return AddressNotFoundErrorMessage();
            }
            else
            {
                bool authorized = await PizzaDb.Commands.UserOwnsDeliveryAddressAsync(User.Identity.GetUserId(), address);

                if (!authorized)
                {
                    return NotAuthorizedToModifyAddressErrorMessage();
                }

                ManageDeliveryAddressViewModel viewModel = new ManageDeliveryAddressViewModel()
                {
                    Id = addressId,
                    Name = address.Name,
                    City = address.City,
                    PhoneNumber = address.PhoneNumber,
                    SelectedAddressType = address.AddressType,
                    SelectedState = address.State,
                    StreetAddress = address.StreetAddress,
                    ZipCode = address.ZipCode
                };

                return View("ManageDeliveryAddress", viewModel);
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeleteDeliveryAddressAjax(int addressId)
        {
            Response.StatusCode = (int)HttpStatusCode.OK;
            DeliveryAddress address = await PizzaDb.GetAsync<DeliveryAddress>(addressId);

            if (address == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json($"Delivery Address with ID {addressId} does not exist.", MediaTypeNames.Text.Plain);
            }

            bool authorized = await PizzaDb.Commands.UserOwnsDeliveryAddressAsync(User.Identity.GetUserId(), address);

            if (!authorized)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json($"Current user is not authorized to delete delivery address ID {addressId}.", MediaTypeNames.Text.Plain);
            }

            int rowsDeleted = await PizzaDb.DeleteByIdAsync<DeliveryAddress>(addressId);

            if (rowsDeleted == 0)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json($"Unable to delete address.", MediaTypeNames.Text.Plain);
            }

            string responseText = "Address deleted.";

            return Json(responseText, MediaTypeNames.Text.Plain);
        }

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(User.Identity.GetUserId()),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(User.Identity.GetUserId()),
                Logins = await UserManager.GetLoginsAsync(User.Identity.GetUserId()),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(User.Identity.GetUserId())
            };

            return View(model);
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;

            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));

            if (result.Succeeded)
            {
                IdentityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }

            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        private ActionResult AddressNotFoundErrorMessage()
        {
            ErrorMessageViewModel viewModel = new ErrorMessageViewModel()
            {
                Header = "Error",
                ErrorMessage = "Address not found.",
                ReturnUrlAction = $"{Url.Action("Index")}?{Request.QueryString}",
                ShowReturnLink = true
            };

            return View("ErrorMessage", viewModel);
        }

        private ActionResult NotAuthorizedToModifyAddressErrorMessage()
        {
            ErrorMessageViewModel viewModel = new ErrorMessageViewModel()
            {
                Header = "Authorization Error",
                ErrorMessage = "You are not authorized to modify this address.",
                ReturnUrlAction = $"{Url.Action("Index")}?{Request.QueryString}",
                ShowReturnLink = true
            };

            return View("ErrorMessage", viewModel);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

#endregion
    }
}