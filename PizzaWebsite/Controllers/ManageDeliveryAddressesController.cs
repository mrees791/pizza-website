using DataLibrary.Models;
using DataLibrary.Models.QuerySearches;
using DataLibrary.Models.Tables;
using Microsoft.AspNet.Identity;
using PizzaWebsite.Controllers.BaseControllers;
using PizzaWebsite.Models;
using PizzaWebsite.Models.ManageDeliveryAddresses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PizzaWebsite.Controllers
{
    [Authorize]
    public class ManageDeliveryAddressesController : BaseController
    {
        public async Task<ActionResult> Index()
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
            ManageAddressesViewModel model = new ManageAddressesViewModel()
            {
                AddressVmList = addressVmList
            };
            return View(model);
        }

        public ActionResult AddNewAddress()
        {
            ManageDeliveryAddressViewModel model = new ManageDeliveryAddressViewModel()
            {
                StateList = GeographyServices.StateList,
                AddressTypeList = ListServices.DeliveryAddressTypeList
            };
            return View("ManageDeliveryAddress", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitDeliveryAddress(ManageDeliveryAddressViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("ManageDeliveryAddress", model);
            }
            DeliveryAddress address = new DeliveryAddress()
            {
                Id = model.Id,
                UserId = User.Identity.GetUserId(),
                Name = model.Name,
                AddressType = model.SelectedAddressType,
                City = model.City,
                PhoneNumber = model.PhoneNumber,
                State = model.SelectedState,
                StreetAddress = model.StreetAddress,
                ZipCode = model.ZipCode
            };
            if (model.IsNewRecord())
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
            return RedirectToAction("Index");
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
            bool authorized = await PizzaDb.Commands.UserOwnsDeliveryAddressAsync(User.Identity.GetUserId(), address);
            if (!authorized)
            {
                return NotAuthorizedToModifyAddressErrorMessage();
            }
            ManageDeliveryAddressViewModel model = new ManageDeliveryAddressViewModel()
            {
                Id = addressId,
                Name = address.Name,
                City = address.City,
                PhoneNumber = address.PhoneNumber,
                SelectedAddressType = address.AddressType,
                SelectedState = address.State,
                StreetAddress = address.StreetAddress,
                ZipCode = address.ZipCode,
                AddressTypeList = ListServices.DeliveryAddressTypeList,
                StateList = GeographyServices.StateList
            };
            return View("ManageDeliveryAddress", model);
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
            return Json("Address deleted.", MediaTypeNames.Text.Plain);
        }

        [HttpPost]
        public async Task<ActionResult> GetDeliveryAddressAjax(int addressId)
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
                return Json($"Current user is not allowed to access delivery address ID {addressId}.", MediaTypeNames.Text.Plain);
            }
            // Name, Address Type, Street Address, City, State, Zip Code, Phone Number
            string[] deliveryAddressResponse = new string[]
            {
                address.Name,
                address.AddressType,
                address.StreetAddress,
                address.City,
                address.State,
                address.ZipCode,
                address.PhoneNumber
            };
            return Json(deliveryAddressResponse);
        }

        private ActionResult AddressNotFoundErrorMessage()
        {
            ErrorMessageViewModel model = new ErrorMessageViewModel()
            {
                Header = "Error",
                ErrorMessage = "Address not found.",
                ReturnUrlAction = $"{Url.Action("Index")}?{Request.QueryString}",
                ShowReturnLink = true
            };
            return View("ErrorMessage", model);
        }

        private ActionResult NotAuthorizedToModifyAddressErrorMessage()
        {
            ErrorMessageViewModel model = new ErrorMessageViewModel()
            {
                Header = "Authorization Error",
                ErrorMessage = "You are not authorized to modify this address.",
                ReturnUrlAction = $"{Url.Action("Index")}?{Request.QueryString}",
                ShowReturnLink = true
            };
            return View("ErrorMessage", model);
        }
    }
}