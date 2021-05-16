using DataLibrary.Models;
using DataLibrary.Models.Joins;
using DataLibrary.Models.QuerySearches;
using DataLibrary.Models.Tables;
using DataLibrary.Models.Utility;
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
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PizzaWebsite.Controllers
{
    public class ShopController : BaseController
    {
        public async Task<ActionResult> Checkout()
        {
            await PizzaDb.Commands.CheckoutCartAsync(await GetCurrentUserAsync());

            SiteUser updatedUser = await GetCurrentUserAsync();

            StoreLocationSearch storeSearch = new StoreLocationSearch()
            {
                IsActiveLocation = true
            };
            DeliveryAddressSearch addressSearch = new DeliveryAddressSearch()
            {
                UserId = updatedUser.Id
            };
            IEnumerable<StoreLocation> storeLocationList = await PizzaDb.GetListAsync<StoreLocation>("Name", storeSearch);
            IEnumerable<DeliveryAddress> deliveryAddressList = await PizzaDb.GetListAsync<DeliveryAddress>("Name", addressSearch);
            IEnumerable<CartItemJoin> cartItemList = await PizzaDb.GetJoinedCartItemListAsync(updatedUser.ConfirmOrderCartId);

            List<SelectListItem> deliveryAddressSelectList = new List<SelectListItem>();
            List<SelectListItem> storeLocationSelectList = new List<SelectListItem>();

            foreach (DeliveryAddress deliveryAddress in deliveryAddressList)
            {
                deliveryAddressSelectList.Add(new SelectListItem()
                {
                    Text = deliveryAddress.Name,
                    Value = deliveryAddress.Id.ToString()
                });
            }

            foreach (StoreLocation storeLocation in storeLocationList)
            {
                storeLocationSelectList.Add(new SelectListItem()
                {
                    Text = storeLocation.Name,
                    Value = storeLocation.Id.ToString()
                });
            }

            // Costs
            decimal orderSubtotal = 0.0m;

            foreach (CartItemJoin cartItemJoin in cartItemList)
            {
                orderSubtotal += cartItemJoin.CartItem.Price;
            }

            decimal orderTax = orderSubtotal * 0.05m;
            decimal orderTotal = orderSubtotal + orderTax;

            CheckoutViewModel checkoutModel = new CheckoutViewModel()
            {
                Cart = new CartViewModel()
                {
                    CartItemList = new List<CartItemViewModel>()
                },
                OrderTypeList = ListUtility.CreateCustomerOrderTypeList(),
                DeliveryStateSelectList = StateListCreator.CreateStateList(),
                DeliveryAddressTypeSelectList = ListUtility.CreateDeliveryAddressTypeList(),
                DeliveryAddressSelectList = deliveryAddressSelectList,
                StoreLocationSelectList = storeLocationSelectList,
                OrderSubtotal = orderSubtotal.ToString("C", CultureInfo.CurrentCulture),
                OrderTax = orderTax.ToString("C", CultureInfo.CurrentCulture),
                OrderTotal = orderTotal.ToString("C", CultureInfo.CurrentCulture)
            };

            await checkoutModel.Cart.LoadCartItems(updatedUser.ConfirmOrderCartId, PizzaDb);


            return View("Checkout", checkoutModel);
        }

        /*[HttpPost]
        public async Task<ActionResult> SubmitOrder(CheckoutViewModel checkoutModel)
        {
            SiteUser user = await GetCurrentUserAsync();

            if (!ModelState.IsValid)
            {
                checkoutModel.Cart = new CartViewModel()
                {
                    CartItemList = new List<CartItemViewModel>()
                };

                checkoutModel.OrderTypeList = ListUtility.CreateCustomerOrderTypeList();
                checkoutModel.DeliveryStateSelectList = StateListCreator.CreateStateList();
                checkoutModel.DeliveryAddressTypeSelectList = ListUtility.CreateDeliveryAddressTypeList();

                await checkoutModel.Cart.LoadCartItems(user.ConfirmOrderCartId, PizzaDb);

                return View("Checkout", checkoutModel);
            }

            // todo: Validate checkout cart
            // Make sure the user cannot submit an outdated checkout cart
            /*SiteUser user = await GetCurrentUserAsync();
            bool orderExpired = model.OrderConfirmationId != user.OrderConfirmationId;

            if (orderExpired)
            {
                return RedirectToAction("OrderExpired");
            }*/

            /*IEnumerable<CartItemJoin> cartItemList = await PizzaDb.GetJoinedCartItemListAsync(user.ConfirmOrderCartId);

            decimal orderSubtotal = 0.0m;

            foreach (CartItemJoin cartItem in cartItemList)
            {
                orderSubtotal += await cartItem.CartItemType.CalculatePriceAsync(PizzaDb);
            }

            decimal orderTax = orderSubtotal * 0.05m;
            decimal orderTotal = orderSubtotal + orderTax;

            CustomerOrder customerOrder = new CustomerOrder()
            {
                UserId = user.Id,
                DateOfOrder = DateTime.Now,
                IsDelivery = checkoutModel.IsDelivery(),
                StoreId = checkoutModel.StoreId,
                OrderPhase = OrderPhase.Order_Placed,
                OrderSubtotal = orderSubtotal,
                OrderTax = orderTax,
                OrderTotal = orderTotal
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
        }*/

        public ActionResult OrderConfirmed()
        {
            return View();
        }

        public ActionResult OrderExpired()
        {
            return View();
        }

        private async Task<bool> AuthorizedToModifyCartItemAsync(int cartItemId)
        {
            return await PizzaDb.Commands.UserOwnsCartItemAsync(await GetCurrentUserAsync(), cartItemId);
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
            bool authorized = await AuthorizedToModifyCartItemAsync(cartItemId);

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
            await AddMenuPizzaToCart(id, currentUser.CurrentCartId, selectedQuantity, selectedSize, selectedCrustId);
            return RedirectToAction("Cart");
        }

        [Authorize]
        public async Task AddMenuPizzaToCart(int menuPizzaId, int cartId, int selectedQuantity, string selectedSize, int selectedCrustId)
        {
            MenuPizza menuPizza = await PizzaDb.GetAsync<MenuPizza>(menuPizzaId);
            CartItemJoin cartItemJoin = await menuPizza.CreateCartRecordsAsync(PizzaDb, cartId, selectedQuantity, selectedSize, selectedCrustId);
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

        [HttpPost]
        public async Task DeleteCartItem(int cartItemId)
        {
            bool authorized = await AuthorizedToModifyCartItemAsync(cartItemId);

            if (!authorized)
            {
                throw new Exception($"Current user is not allowed to delete cart item ID {cartItemId}.");
            }

            await PizzaDb.DeleteByIdAsync<CartItem>(cartItemId);
        }

        [HttpPost]
        public async Task<string> UpdateCartItemQuantity(int cartItemId, int quantity)
        {
            bool authorized = await AuthorizedToModifyCartItemAsync(cartItemId);

            if (!authorized)
            {
                throw new Exception($"Current user is not allowed to modify cart item ID {cartItemId}.");
            }

            CartItem updatedCartItem = await PizzaDb.Commands.UpdateCartItemQuantityAsync(cartItemId, quantity);

            return updatedCartItem.Price.ToString("C", CultureInfo.CurrentCulture);
        }
    }
}