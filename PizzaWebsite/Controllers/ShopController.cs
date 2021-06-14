﻿using DataLibrary.Models;
using DataLibrary.Models.Builders;
using DataLibrary.Models.Exceptions;
using DataLibrary.Models.JoinLists;
using DataLibrary.Models.JoinLists.CartItemCategories;
using DataLibrary.Models.QuerySearches;
using DataLibrary.Models.Tables;
using DataLibrary.Models.Utility;
using Microsoft.AspNet.Identity;
using PizzaWebsite.Controllers.BaseControllers;
using PizzaWebsite.Models;
using PizzaWebsite.Models.Carts;
using PizzaWebsite.Models.Geography;
using PizzaWebsite.Models.Identity.Stores;
using PizzaWebsite.Models.PizzaBuilders;
using PizzaWebsite.Models.Shop;
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
        public async Task<ActionResult> Checkout()
        {
            CheckoutViewModel checkoutModel = new CheckoutViewModel();
            SiteUser currentUser = await GetCurrentUserAsync();
            await checkoutModel.InitializeAsync(false, currentUser, PizzaDb);
            return View("Checkout", checkoutModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Checkout(CheckoutViewModel checkoutModel)
        {
            return await SubmitOrder(checkoutModel);
        }

        // todo: Finish SubmitOrder
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> SubmitOrder(CheckoutViewModel checkoutModel)
        {
            SiteUser user = await GetCurrentUserAsync();

            if (!ModelState.IsValid)
            {
                await checkoutModel.InitializeAsync(true, user, PizzaDb);
                return View("Checkout", checkoutModel);
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
                IsDelivery = checkoutModel.IsDelivery(),
                StoreId = checkoutModel.SelectedStoreLocationId,
                OrderPhase = OrderPhase.Order_Placed,
                OrderSubtotal = costSummary.Subtotal,
                OrderTax = costSummary.Tax,
                OrderTotal = costSummary.Total
            };
            if (checkoutModel.IsDelivery())
            {
                if (checkoutModel.IsNewDeliveryAddress() && checkoutModel.SaveNewDeliveryAddress)
                {
                    DeliveryAddress deliveryAddress = new DeliveryAddress()
                    {
                        UserId = User.Identity.GetUserId(),
                        Name = checkoutModel.DeliveryAddressName,
                        AddressType = checkoutModel.SelectedDeliveryAddressType,
                        City = checkoutModel.DeliveryCity,
                        PhoneNumber = checkoutModel.DeliveryPhoneNumber,
                        State = checkoutModel.SelectedDeliveryState,
                        StreetAddress = checkoutModel.DeliveryStreetAddress,
                        ZipCode = checkoutModel.DeliveryZipCode
                    };
                    await PizzaDb.InsertAsync(deliveryAddress);
                }
                DeliveryInfo deliveryInfo = new DeliveryInfo()
                {
                    DateOfDelivery = DateTime.Now,
                    DeliveryAddressName = checkoutModel.DeliveryAddressName,
                    DeliveryAddressType = checkoutModel.SelectedDeliveryAddressType,
                    DeliveryCity = checkoutModel.DeliveryCity,
                    DeliveryPhoneNumber = checkoutModel.DeliveryPhoneNumber,
                    DeliveryState = checkoutModel.SelectedDeliveryState,
                    DeliveryStreetAddress = checkoutModel.DeliveryStreetAddress,
                    DeliveryZipCode = checkoutModel.DeliveryZipCode
                };
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
            CartViewModel cartVm = new CartViewModel();
            await cartVm.InitializeAsync(user.CurrentCartId, PizzaDb);
            return View(cartVm);
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