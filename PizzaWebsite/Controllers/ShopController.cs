using DataLibrary.Models;
using DataLibrary.Models.Exceptions;
using DataLibrary.Models.Joins;
using DataLibrary.Models.QuerySearches;
using DataLibrary.Models.Tables;
using DataLibrary.Models.Utility;
using Microsoft.AspNet.Identity;
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
            await checkoutModel.InitializeAsync(currentUser, PizzaDb);

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
                await checkoutModel.InitializeAsync(user, PizzaDb);

                return View("Checkout", checkoutModel);
            }

            // todo: Finish client side validation using OrderConfirmationId
            /*bool orderExpired = checkoutModel.OrderConfirmationId != user.OrderConfirmationId;

            if (orderExpired)
            {
                return RedirectToAction("OrderExpired");
            }*/

            IEnumerable<CartItemJoin> cartItemJoinList = await PizzaDb.GetJoinedCartItemListAsync(user.ConfirmOrderCartId);
            CostSummary costSummary = new CostSummary(cartItemJoinList);

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

                await PizzaDb.Commands.SubmitCustomerOrderAsync(user, customerOrder, deliveryInfo);
            }
            else
            {
                await PizzaDb.Commands.SubmitCustomerOrderAsync(user, customerOrder);
            }

            return RedirectToAction("OrderConfirmed");
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
            return await PizzaDb.Commands.UserOwnsCartItemAsync(await GetCurrentUserAsync(), cartItem);
        }

        private async Task<CartPizzaBuilderViewModel> CreatePizzaBuilderVm(int cartItemId)
        {
            CartItem cartItem = await PizzaDb.GetAsync<CartItem>(cartItemId);
            CartPizza cartPizza = await PizzaDb.GetAsync<CartPizza>(cartItemId);

            CartPizzaBuilderViewModel viewModel = new CartPizzaBuilderViewModel();
            await viewModel.CreateFromRecordsAsync(PizzaDb, cartItem, cartPizza);

            return viewModel;
        }

        [Authorize]
        public async Task<ActionResult> BuildPizza()
        {
            CartPizzaBuilderViewModel cartPizzaVm = new CartPizzaBuilderViewModel();
            await cartPizzaVm.CreateDefaultAsync(PizzaDb);

            return View("CartPizzaBuilder", cartPizzaVm);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> ModifyCartPizza(int cartItemId)
        {
            CartItem cartItem = await PizzaDb.GetAsync<CartItem>(cartItemId);

            if (cartItem == null)
            {
                throw new RecordDoesNotExistException($"Cart Item with ID {cartItemId} does not exist.");
            }

            bool authorized = await AuthorizedToModifyCartItemAsync(cartItem);

            if (!authorized)
            {
                throw new Exception($"Current user is not allowed to modify cart item ID {cartItemId}.");
            }

            CartPizzaBuilderViewModel cartPizzaVm = await CreatePizzaBuilderVm(cartItemId);

            return View("CartPizzaBuilder", cartPizzaVm);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> BuildPizza(CartPizzaBuilderViewModel model)
        {
            if (ModelState.IsValid)
            {
                SiteUser currentUser = await GetCurrentUserAsync();
                CartPizza cartPizza = model.ToCartPizza();

                decimal pricePerItem = await cartPizza.CalculateItemPriceAsync(PizzaDb);

                CartItem cartItem = new CartItem()
                {
                    Id = model.Id,
                    CartId = currentUser.CurrentCartId,
                    UserId = User.Identity.GetUserId<int>(),
                    ProductCategory = ProductCategory.Pizza.ToString(),
                    Quantity = model.SelectedQuantity,
                    PricePerItem = pricePerItem,
                    Price = pricePerItem * model.SelectedQuantity
                };

                CartItemJoin cartItemJoin = new CartItemJoin()
                {
                    CartItem = cartItem,
                    CartItemType = cartPizza
                };

                if (model.IsNewRecord())
                {
                    await PizzaDb.InsertAsync(cartItemJoin);
                }
                else
                {
                    await PizzaDb.UpdateAsync(cartItemJoin);
                }

                return RedirectToAction("Cart");
            }
            else
            {
                return View("CartPizzaBuilder", model);
            }
        }

        [Authorize]
        public async Task<ActionResult> AddMenuPizzaToCurrentCart(int id, int selectedQuantity, string selectedSize, int selectedCrustId)
        {
            SiteUser currentUser = await GetCurrentUserAsync();
            await AddMenuPizzaToCart(id, currentUser.CurrentCartId, currentUser.Id, selectedQuantity, selectedSize, selectedCrustId);
            return RedirectToAction("Cart");
        }

        [Authorize]
        public async Task AddMenuPizzaToCart(int menuPizzaId, int cartId, int userId, int selectedQuantity, string selectedSize, int selectedCrustId)
        {
            MenuPizza menuPizza = await PizzaDb.GetAsync<MenuPizza>(menuPizzaId);
            CartItemJoin cartItemJoin = await menuPizza.CreateCartRecordsAsync(PizzaDb, cartId, userId, selectedQuantity, selectedSize, selectedCrustId);
            await PizzaDb.InsertAsync(cartItemJoin);
        }

        public async Task<ActionResult> PizzaMenu()
        {
            PizzaMenuPageViewModel pizzaMenuVm = new PizzaMenuPageViewModel();

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
            IEnumerable<MenuPizza> popularMenuPizzas = await PizzaDb.GetListAsync<MenuPizza>("SortOrder", popularPizzaSearch);
            IEnumerable<MenuPizza> meatsMenuPizzas = await PizzaDb.GetListAsync<MenuPizza>("SortOrder", meatPizzaSearch);
            IEnumerable<MenuPizza> veggieMenuPizzas = await PizzaDb.GetListAsync<MenuPizza>("SortOrder", veggiePizzaSearch);
            // Separate into categories
            pizzaMenuVm.PopularPizzaList = CreateMenuPizzaViewModels(popularMenuPizzas, quantityList, sizeList, crustListDictionary);
            pizzaMenuVm.MeatsPizzaList = CreateMenuPizzaViewModels(meatsMenuPizzas, quantityList, sizeList, crustListDictionary);
            pizzaMenuVm.VeggiePizzaList = CreateMenuPizzaViewModels(veggieMenuPizzas, quantityList, sizeList, crustListDictionary);

            return View(pizzaMenuVm);
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
            CartViewModel cartVm = new CartViewModel()
            {
                CartItemList = new List<CartItemViewModel>()
            };

            await cartVm.LoadCartItems(user.CurrentCartId, PizzaDb);

            return View(cartVm);
        }

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
    }
}