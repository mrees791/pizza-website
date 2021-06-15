using DataLibrary.Models;
using DataLibrary.Models.Builders;
using DataLibrary.Models.JoinLists;
using DataLibrary.Models.JoinLists.CartItemCategories;
using DataLibrary.Models.QuerySearches;
using DataLibrary.Models.Tables;
using Microsoft.AspNet.Identity;
using PizzaWebsite.Controllers.BaseControllers;
using PizzaWebsite.Models;
using PizzaWebsite.Models.Carts;
using PizzaWebsite.Models.Geography;
using PizzaWebsite.Models.Identity.Stores;
using PizzaWebsite.Models.PizzaBuilders;
using PizzaWebsite.Models.Shop;
using PizzaWebsite.Models.ViewModelServices;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PizzaWebsite.Controllers
{
    public class ShopController : BaseController
    {
        private CartServices _cartServices;
        private CheckoutServices _checkoutServices;

        public ShopController()
        {
            _cartServices = new CartServices();
            _checkoutServices = new CheckoutServices();
        }

        [Authorize]
        public async Task<ActionResult> Checkout()
        {
            SiteUser currentUser = await GetCurrentUserAsync();
            await PizzaDb.Commands.CheckoutCartAsync(currentUser);
            CheckoutViewModel model = await _checkoutServices.CreateViewModelAsync(currentUser, PizzaDb, ListServices.DefaultQuantityList, GeographyServices.StateList,
                ListServices.CustomerOrderTypeList, ListServices.DeliveryAddressTypeList);
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> SubmitOrder(CheckoutViewModel model)
        {
            SiteUser user = await GetCurrentUserAsync();
            if (!ModelState.IsValid)
            {
                model.CartVm = await _cartServices.CreateViewModelAsync(user.ConfirmOrderCartId, PizzaDb, ListServices.DefaultQuantityList);
                return View("Checkout", model);
            }
            // todo: Finish client side validation using OrderConfirmationId
            /*bool orderExpired = checkoutModel.OrderConfirmationId != user.OrderConfirmationId;

            if (orderExpired)
            {
                return RedirectToAction("OrderExpired");
            }*/
            CartItemJoinList cartItemJoinList = new CartItemJoinList();
            await cartItemJoinList.LoadListByCartIdAsync(user.ConfirmOrderCartId, PizzaDb);
            CostSummary costSummary = new CostSummary(cartItemJoinList.Items);
            CustomerOrder customerOrder = new CustomerOrder()
            {
                UserId = user.Id,
                DateOfOrder = DateTime.Now,
                IsDelivery = model.IsDelivery(),
                StoreId = model.SelectedStoreLocationId,
                OrderPhase = OrderPhase.Order_Placed,
                OrderSubtotal = costSummary.Subtotal,
                OrderTax = costSummary.Tax,
                OrderTotal = costSummary.Total
            };
            if (model.IsDelivery())
            {
                // Initialize delivery info
                DeliveryInfo deliveryInfo = null;
                if (model.IsNewDeliveryAddress())
                {
                    deliveryInfo = new DeliveryInfo()
                    {
                        DateOfDelivery = DateTime.Now,
                        DeliveryAddressName = model.DeliveryAddressName,
                        DeliveryAddressType = model.SelectedDeliveryAddressType,
                        DeliveryCity = model.DeliveryCity,
                        DeliveryPhoneNumber = model.DeliveryPhoneNumber,
                        DeliveryState = model.SelectedDeliveryState,
                        DeliveryStreetAddress = model.DeliveryStreetAddress,
                        DeliveryZipCode = model.DeliveryZipCode
                    };
                    if (model.SaveNewDeliveryAddress)
                    {
                        DeliveryAddress deliveryAddress = new DeliveryAddress()
                        {
                            UserId = User.Identity.GetUserId(),
                            Name = model.DeliveryAddressName,
                            AddressType = model.SelectedDeliveryAddressType,
                            City = model.DeliveryCity,
                            PhoneNumber = model.DeliveryPhoneNumber,
                            State = model.SelectedDeliveryState,
                            StreetAddress = model.DeliveryStreetAddress,
                            ZipCode = model.DeliveryZipCode
                        };
                        await PizzaDb.InsertAsync(deliveryAddress);
                    }
                }
                else
                {
                    DeliveryAddress deliveryAddress = await PizzaDb.GetAsync<DeliveryAddress>(model.SelectedDeliveryAddressId);
                    if (deliveryAddress == null)
                    {
                        throw new Exception($"Delivery address with ID {model.SelectedDeliveryAddressId} does not exist.");
                    }
                    bool isAuthorized = await PizzaDb.Commands.UserOwnsDeliveryAddressAsync(User.Identity.GetUserId(), deliveryAddress);
                    if (!isAuthorized)
                    {
                        throw new Exception($"User does not have authorization to access delivery address with ID {model.SelectedDeliveryAddressId}.");
                    }
                    deliveryInfo = new DeliveryInfo()
                    {
                        DateOfDelivery = DateTime.Now,
                        DeliveryAddressName = deliveryAddress.Name,
                        DeliveryAddressType = deliveryAddress.AddressType,
                        DeliveryCity = deliveryAddress.City,
                        DeliveryPhoneNumber = deliveryAddress.PhoneNumber,
                        DeliveryState = deliveryAddress.State,
                        DeliveryStreetAddress = deliveryAddress.StreetAddress,
                        DeliveryZipCode = deliveryAddress.ZipCode
                    };
                }
                await PizzaDb.Commands.AddCustomerOrderAsync(user, customerOrder, deliveryInfo);
            }
            else
            {
                await PizzaDb.Commands.AddCustomerOrderAsync(user, customerOrder);
            }
            OrderStatusViewModel orderStatusVm = new OrderStatusViewModel()
            {
                CustomerOrderId = customerOrder.Id
            };
            return View("OrderStatus", orderStatusVm);
        }

        public ActionResult OrderConfirmed()
        {
            return View();
        }

        public ActionResult OrderExpired()
        {
            return View();
        }

        [Authorize]
        public async Task<ActionResult> Cart()
        {
            SiteUser user = await GetCurrentUserAsync();
            CartViewModel model = await _cartServices.CreateViewModelAsync(user.CurrentCartId, PizzaDb, ListServices.DefaultQuantityList);
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> DeleteCartItemAjax(int cartItemId)
        {
            Response.StatusCode = (int)HttpStatusCode.OK;
            CartItem cartItem = await PizzaDb.GetAsync<CartItem>(cartItemId);
            if (cartItem == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json($"Cart Item with ID {cartItemId} does not exist.", MediaTypeNames.Text.Plain);
            }
            bool authorized = await PizzaDb.Commands.UserOwnsCartItemAsync(User.Identity.GetUserId(), cartItem);
            if (!authorized)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json($"Current user is not allowed to modify cart item ID {cartItemId}.", MediaTypeNames.Text.Plain);
            }
            int rowsDeleted = await PizzaDb.DeleteByIdAsync<CartItem>(cartItemId);
            string responseText = $"{rowsDeleted} rows deleted.";
            return Json(responseText, MediaTypeNames.Text.Plain);
        }

        [HttpPost]
        public async Task<ActionResult> GetCartSubtotalAjax(int cartId)
        {
            Response.StatusCode = (int)HttpStatusCode.OK;
            Cart cart = await PizzaDb.GetAsync<Cart>(cartId);
            if (cart == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json($"Cart with ID {cartId} does not exist.", MediaTypeNames.Text.Plain);
            }
            SiteUser user = await GetCurrentUserAsync();
            bool authorized = await PizzaDb.Commands.UserOwnsCartAsync(user, cart);

            if (!authorized)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json($"Current user does not own cart with ID {cartId}.", MediaTypeNames.Text.Plain);
            }
            decimal cartSubtotal = await PizzaDb.Commands.CalculateCartSubtotalAsync(cartId);
            string formattedPrice = cartSubtotal.ToString("C", CultureInfo.CurrentCulture);
            return Json(formattedPrice);
        }

        private string GetOrderStatusMessage(OrderPhase orderPhase)
        {
            switch (orderPhase)
            {
                case OrderPhase.Order_Placed:
                    return "Your order has been placed.";
                case OrderPhase.Prep:
                    return "Your order is being prepared.";
                case OrderPhase.Bake:
                    return "Your order is being baked.";
                case OrderPhase.Box:
                    return "Your order is being boxed.";
                case OrderPhase.Ready_For_Pickup:
                    return "Your order is ready for pickup.";
                case OrderPhase.Out_For_Delivery:
                    return "Your order is out for delivery.";
            }
            return "Unable to get order status.";
        }

        [HttpPost]
        public async Task<ActionResult> GetOrderStatusAjax(int orderId)
        {
            Response.StatusCode = (int)HttpStatusCode.OK;
            CustomerOrder order = await PizzaDb.GetAsync<CustomerOrder>(orderId);
            if (order == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json($"Order with ID {order} does not exist.", MediaTypeNames.Text.Plain);
            }
            SiteUser user = await GetCurrentUserAsync();
            bool authorized = await PizzaDb.Commands.UserOwnsCustomerOrderAsync(user, order);
            if (!authorized)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json($"Current user does not own order with ID {orderId}.", MediaTypeNames.Text.Plain);
            }
            string orderStatus = GetOrderStatusMessage(order.OrderPhase);
            return Json(orderStatus);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateCartItemQuantityAjax(int cartItemId, int quantity)
        {
            Response.StatusCode = (int)HttpStatusCode.OK;
            CartItem cartItem = await PizzaDb.GetAsync<CartItem>(cartItemId);
            if (cartItem == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json($"Cart Item with ID {cartItemId} does not exist.", MediaTypeNames.Text.Plain);
            }
            bool authorized = await PizzaDb.Commands.UserOwnsCartItemAsync(User.Identity.GetUserId(), cartItem);
            if (!authorized)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json($"Current user is not allowed to modify cart item ID {cartItemId}.", MediaTypeNames.Text.Plain);
            }
            cartItem = await PizzaDb.Commands.UpdateCartItemQuantityAsync(cartItem, quantity);
            string updatedPrice = cartItem.Price.ToString("C", CultureInfo.CurrentCulture);
            return Json(updatedPrice);
        }

        private ActionResult CartItemAuthorizationErrorMessage()
        {
            ErrorMessageViewModel model = new ErrorMessageViewModel()
            {
                Header = "Authorization Error",
                ErrorMessage = "You are not authorized to modify this item.",
                ReturnUrlAction = $"{Url.Action("Cart")}",
                ShowReturnLink = true
            };
            return View("ErrorMessage", model);
        }

        private ActionResult CartItemDoesNotExistErrorMessage()
        {
            ErrorMessageViewModel model = new ErrorMessageViewModel()
            {
                Header = "Error",
                ErrorMessage = "That item does not exist. It may have been removed from your cart.",
                ReturnUrlAction = $"{Url.Action("Cart")}",
                ShowReturnLink = true
            };
            return View("ErrorMessage", model);
        }
    }
}