using DataLibrary.Models;
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

        [HttpPost]
        public async Task<ActionResult> Checkout(CheckoutViewModel checkoutModel)
        {
            return await SubmitOrder(checkoutModel);
        }

        // todo: Finish SubmitOrder
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

        private async Task<bool> AuthorizedToModifyCartItemAsync(CartItem cartItem)
        {
            return await PizzaDb.Commands.UserOwnsCartItemAsync(User.Identity.GetUserId(), cartItem);
        }

        private async Task<CartPizzaBuilderViewModel> CreatePizzaBuilderVmAsync()
        {
            CartPizzaBuilder pizzaBuilder = new CartPizzaBuilder();
            await pizzaBuilder.InitializeAsync(new MenuItemSearch { AvailableForPurchase = true }, PizzaDb);
            CartItem cartItem = new CartItem()
            {
                Quantity = 1
            };
            CartPizza cartPizza = new CartPizza()
            {
                CheeseAmount = "Regular",
                SauceAmount = "Regular",
                Size = "Medium",
                MenuPizzaCheeseId = pizzaBuilder.CheeseList.First().Id,
                MenuPizzaCrustFlavorId = pizzaBuilder.CrustFlavorList.First().Id,
                MenuPizzaCrustId = pizzaBuilder.CrustList.First().Id,
                MenuPizzaSauceId = pizzaBuilder.SauceList.First().Id
            };
            return await CreatePizzaBuilderVmAsync(cartItem, cartPizza);
        }

        private List<PizzaToppingViewModel> CreateToppingViewModelList(IEnumerable<CartPizzaTopping> cartToppingList, IEnumerable<MenuPizzaToppingType> toppingTypeList)
        {
            List<PizzaTopping> toppingList = new List<PizzaTopping>();
            foreach (CartPizzaTopping cartTopping in cartToppingList)
            {
                PizzaTopping topping = new PizzaTopping()
                {
                    ToppingTypeId = cartTopping.MenuPizzaToppingTypeId,
                    ToppingAmount = cartTopping.ToppingAmount,
                    ToppingHalf = cartTopping.ToppingHalf
                };
                toppingList.Add(topping);
            }
            return PizzaBuilderManager.CreateToppingViewModelList(toppingList, toppingTypeList);
        }

        private async Task<CartPizzaBuilderViewModel> CreatePizzaBuilderVmAsync(CartItem cartItem, CartPizza cartPizza)
        {
            CartPizzaBuilder pizzaBuilder = new CartPizzaBuilder();
            await pizzaBuilder.InitializeAsync(new MenuItemSearch { AvailableForPurchase = true }, PizzaDb);
            IEnumerable<PizzaTopping> toppingList = new List<PizzaTopping>();
            Dictionary<int, string> cheeseDictionary = new Dictionary<int, string>();
            Dictionary<int, string> crustFlavorDictionary = new Dictionary<int, string>();
            Dictionary<int, string> crustDictionary = new Dictionary<int, string>();
            Dictionary<int, string> sauceDictionary = new Dictionary<int, string>();
            Dictionary<int, PizzaTopping> toppingDictionary = new Dictionary<int, PizzaTopping>();
            foreach (MenuPizzaCheese cheese in pizzaBuilder.CheeseList)
            {
                cheeseDictionary.Add(cheese.Id, cheese.Name);
            }
            foreach (MenuPizzaCrustFlavor crustFlavor in pizzaBuilder.CrustFlavorList)
            {
                crustFlavorDictionary.Add(crustFlavor.Id, crustFlavor.Name);
            }
            foreach (MenuPizzaCrust crust in pizzaBuilder.CrustList)
            {
                crustDictionary.Add(crust.Id, crust.Name);
            }
            foreach (MenuPizzaSauce sauce in pizzaBuilder.SauceList)
            {
                sauceDictionary.Add(sauce.Id, sauce.Name);
            }
            List<PizzaToppingViewModel> toppingVmList = CreateToppingViewModelList(cartPizza.ToppingList, pizzaBuilder.ToppingTypeList);
            return new CartPizzaBuilderViewModel()
            {
                Id = cartItem.Id,
                SelectedQuantity = cartItem.Quantity,
                SelectedCheeseAmount = cartPizza.CheeseAmount,
                SelectedCheeseId = cartPizza.MenuPizzaCheeseId,
                SelectedCrustFlavorId = cartPizza.MenuPizzaCrustFlavorId,
                SelectedCrustId = cartPizza.MenuPizzaCrustId,
                SelectedSauceAmount = cartPizza.SauceAmount,
                SelectedSauceId = cartPizza.MenuPizzaSauceId,
                SelectedSize = cartPizza.Size,
                CheeseAmountList = pizzaBuilder.CheeseAmountList,
                QuantityList = pizzaBuilder.QuantityList,
                SizeList = pizzaBuilder.SizeList,
                SauceAmountList = pizzaBuilder.SauceAmountList,
                CheeseDictionary = cheeseDictionary,
                CrustFlavorDictionary = crustFlavorDictionary,
                SauceDictionary = sauceDictionary,
                CrustDictionary = crustDictionary,
                ToppingVmList = toppingVmList
            };
        }

        [Authorize]
        public async Task<ActionResult> BuildPizza()
        {
            CartPizzaBuilder pizzaBuilder = new CartPizzaBuilder();
            await pizzaBuilder.InitializeAsync(new MenuItemSearch() { AvailableForPurchase = true }, PizzaDb);
            CartPizzaBuilderViewModel model = await CreatePizzaBuilderVmAsync();
            return View("CartPizzaBuilder", model);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> BuildPizza(CartPizzaBuilderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("CartPizzaBuilder", model);
            }
            SiteUser currentUser = await GetCurrentUserAsync();
            CartPizza cartPizza = CreateCartPizzaFromViewModel(model);
            decimal pricePerItem = await cartPizza.CalculateItemPriceAsync(PizzaDb);
            CartItem cartItem = new CartItem()
            {
                Id = model.Id,
                CartId = currentUser.CurrentCartId,
                UserId = User.Identity.GetUserId(),
                ProductCategory = ProductCategory.Pizza.ToString(),
                Quantity = model.SelectedQuantity,
                PricePerItem = pricePerItem,
                Price = pricePerItem * model.SelectedQuantity
            };
            if (model.IsNewRecord())
            {
                await PizzaDb.Commands.AddItemToCart(currentUser, cartItem, cartPizza);
            }
            else
            {
                await PizzaDb.Commands.UpdateCartItemAsync(cartItem, cartPizza);
            }
            return RedirectToAction("Cart");
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> ModifyCartPizza(int cartItemId)
        {
            CartItemOnCartPizzaJoinList joinList = new CartItemOnCartPizzaJoinList();
            await joinList.LoadFirstOrDefaultAsync(cartItemId, PizzaDb);
            CartItemJoin join = joinList.Items.FirstOrDefault();
            if (join == null)
            {
                return CartItemDoesNotExistErrorMessage();
            }
            bool authorized = await AuthorizedToModifyCartItemAsync(join.CartItem);
            if (!authorized)
            {
                return CartItemAuthorizationErrorMessage();
            }
            CartPizzaBuilderViewModel cartPizzaVm = await CreatePizzaBuilderVmAsync(join.CartItem, (CartPizza)join.CartItemType);
            return View("CartPizzaBuilder", cartPizzaVm);
        }

        private CartPizza CreateCartPizzaFromViewModel(CartPizzaBuilderViewModel model)
        {
            List<CartPizzaTopping> toppingList = new List<CartPizzaTopping>();
            foreach (PizzaToppingViewModel toppingVm in model.ToppingVmList)
            {
                if (toppingVm.SelectedAmount != "None")
                {
                    toppingList.Add(new CartPizzaTopping()
                    {
                        MenuPizzaToppingTypeId = toppingVm.Id,
                        ToppingAmount = toppingVm.SelectedAmount,
                        ToppingHalf = toppingVm.SelectedToppingHalf
                    });
                }
            }
            return new CartPizza()
            {
                CartItemId = model.Id,
                CheeseAmount = model.SelectedCheeseAmount,
                SauceAmount = model.SelectedSauceAmount,
                Size = model.SelectedSize,
                MenuPizzaCheeseId = model.SelectedCheeseId,
                MenuPizzaCrustId = model.SelectedCrustId,
                MenuPizzaCrustFlavorId = model.SelectedCrustFlavorId,
                MenuPizzaSauceId = model.SelectedSauceId,
                ToppingList = toppingList
            };
        }

        [Authorize]
        public async Task<ActionResult> AddMenuPizzaToCurrentCart(int id, int selectedQuantity, string selectedSize, int selectedCrustId)
        {
            SiteUser currentUser = await GetCurrentUserAsync();
            MenuPizza menuPizza = await PizzaDb.GetAsync<MenuPizza>(id);
            Tuple<CartItem, CartPizza> cartItemRecords = await menuPizza.CreateCartRecordsAsync(selectedQuantity, selectedSize, selectedCrustId, currentUser, PizzaDb);
            await PizzaDb.Commands.AddItemToCart(currentUser, cartItemRecords.Item1, cartItemRecords.Item2);
            return RedirectToAction("Cart");
        }

        public async Task<ActionResult> PizzaMenu()
        {
            List<int> quantityList = ListUtility.CreateQuantityList();
            List<string> sizeList = ListUtility.GetPizzaSizeList();
            Dictionary<int, string> crustListDictionary = await ListUtility.CreateCrustDictionaryAsync(PizzaDb);
            MenuPizzaSearch popularPizzaSearch = new MenuPizzaSearch()
            {
                AvailableForPurchase = true,
                CategoryName = "Popular"
            };
            MenuPizzaSearch meatPizzaSearch = new MenuPizzaSearch()
            {
                AvailableForPurchase = true,
                CategoryName = "Meats"
            };
            MenuPizzaSearch veggiePizzaSearch = new MenuPizzaSearch()
            {
                AvailableForPurchase = true,
                CategoryName = "Veggie"
            };
            // Load all menu pizzas in to each category.
            IEnumerable<MenuPizza> popularMenuPizzas = await PizzaDb.GetListAsync<MenuPizza>("SortOrder", SortOrder.Ascending, popularPizzaSearch);
            IEnumerable<MenuPizza> meatsMenuPizzas = await PizzaDb.GetListAsync<MenuPizza>("SortOrder", SortOrder.Ascending, meatPizzaSearch);
            IEnumerable<MenuPizza> veggieMenuPizzas = await PizzaDb.GetListAsync<MenuPizza>("SortOrder", SortOrder.Ascending, veggiePizzaSearch);
            // Separate into categories
            PizzaMenuPageViewModel model = new PizzaMenuPageViewModel()
            {
                PopularPizzaList = CreateMenuPizzaViewModels(popularMenuPizzas, quantityList, sizeList, crustListDictionary),
                MeatsPizzaList = CreateMenuPizzaViewModels(meatsMenuPizzas, quantityList, sizeList, crustListDictionary),
                VeggiePizzaList = CreateMenuPizzaViewModels(veggieMenuPizzas, quantityList, sizeList, crustListDictionary)
            };
            return View(model);
        }

        private List<MenuPizzaViewModel> CreateMenuPizzaViewModels(IEnumerable<MenuPizza> menuPizzaList, List<int> quantityList, List<string> sizeList, Dictionary<int, string> crustListDictionary)
        {
            List<MenuPizzaViewModel> viewModelList = new List<MenuPizzaViewModel>();
            foreach (MenuPizza menuPizza in menuPizzaList)
            {
                MenuPizzaViewModel viewModel = new MenuPizzaViewModel()
                {
                    Id = menuPizza.Id,
                    Name = menuPizza.PizzaName,
                    QuantityList = quantityList,
                    SelectedQuantity = 1,
                    SizeList = sizeList,
                    SelectedSize = "Medium",
                    CrustList = crustListDictionary
                };
                viewModelList.Add(viewModel);
            }
            return viewModelList;
        }

        [Authorize]
        public async Task<ActionResult> Cart()
        {
            SiteUser user = await GetCurrentUserAsync();
            CartViewModel cartVm = new CartViewModel();
            await cartVm.InitializeAsync(user.CurrentCartId, PizzaDb);
            return View(cartVm);
        }

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
            bool authorized = await AuthorizedToModifyCartItemAsync(cartItem);
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
            bool authorized = await AuthorizedToModifyCartItemAsync(cartItem);
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