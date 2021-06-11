using DataLibrary.Models;
using DataLibrary.Models.Exceptions;
using DataLibrary.Models.JoinLists;
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
        public async Task<ActionResult> OrderAgain(int id)
        {
            CustomerOrder customerOrder = await PizzaDb.GetAsync<CustomerOrder>(id);

            if (customerOrder == null)
            {
                throw new RecordDoesNotExistException($"Customer Order with ID {id} does not exist.");
            }

            SiteUser currentUser = await GetCurrentUserAsync();

            bool authorized = await PizzaDb.Commands.UserOwnsCustomerOrderAsync(currentUser, customerOrder);

            if (!authorized)
            {
                throw new Exception($"Current user does not own order with ID {id}.");
            }

            await PizzaDb.Commands.ReorderPreviousOrder(currentUser, customerOrder);

            return RedirectToAction("Cart");
        }

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

            DataLibrary.Models.JoinLists.CartItemJoinList cartItemJoinList = new DataLibrary.Models.JoinLists.CartItemJoinList();
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

        public ActionResult PreviousOrderStatus(int id)
        {
            OrderStatusViewModel orderStatusVm = new OrderStatusViewModel()
            {
                CustomerOrderId = id
            };

            return View(orderStatusVm);
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
            else
            {
                return View("CartPizzaBuilder", model);
            }
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
            IEnumerable<MenuPizza> popularMenuPizzas = await PizzaDb.GetListAsync<MenuPizza>("SortOrder", SortOrder.Ascending, popularPizzaSearch);
            IEnumerable<MenuPizza> meatsMenuPizzas = await PizzaDb.GetListAsync<MenuPizza>("SortOrder", SortOrder.Ascending, meatPizzaSearch);
            IEnumerable<MenuPizza> veggieMenuPizzas = await PizzaDb.GetListAsync<MenuPizza>("SortOrder", SortOrder.Ascending, veggiePizzaSearch);
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

        [Authorize]
        public async Task<ActionResult> PreviousOrder(int? id)
        {
            var join = new CustomerOrderOnDeliveryInfoJoinList();
            await join.LoadFirstOrDefaultByCustomerOrderIdAsync(id.Value, PizzaDb);
            Join<CustomerOrder, DeliveryInfo> result = join.Items.FirstOrDefault();

            PreviousOrderViewModel orderVm = new PreviousOrderViewModel();
            await orderVm.InitializeAsync(true, result.Table1, result.Table2, PizzaDb);

            return View(orderVm);
        }

        [Authorize]
        public async Task<ActionResult> PreviousOrders(int? page, int? rowsPerPage)
        {
            // Set default values
            if (!page.HasValue)
            {
                page = 1;
            }

            if (!rowsPerPage.HasValue)
            {
                rowsPerPage = 10;
            }

            PreviousOrderSearch search = new PreviousOrderSearch()
            {
                UserId = User.Identity.GetUserId()
            };

            PaginationViewModel paginationVm = new PaginationViewModel()
            {
                QueryString = Request.QueryString,
                CurrentPage = page.Value,
                RowsPerPage = rowsPerPage.Value,
                TotalNumberOfItems = await PizzaDb.GetNumberOfRecordsAsync<CustomerOrder>(search),
                TotalPages = await PizzaDb.GetNumberOfPagesAsync<CustomerOrder>(rowsPerPage.Value, search)
            };


            List<PreviousOrderViewModel> previousOrderVmList = new List<PreviousOrderViewModel>();
            IEnumerable<CustomerOrder> previousOrderList = await PizzaDb.GetPagedListAsync<CustomerOrder>(page.Value, rowsPerPage.Value, "Id", SortOrder.Descending, search);

            foreach (CustomerOrder prevOrder in previousOrderList)
            {
                PreviousOrderViewModel orderVm = new PreviousOrderViewModel();
                await orderVm.InitializeAsync(false, prevOrder, null, PizzaDb);
                previousOrderVmList.Add(orderVm);
            }

            PreviousOrderListViewModel previousOrdersVm = new PreviousOrderListViewModel()
            {
                PaginationVm = paginationVm,
                PreviousOrderVmList = previousOrderVmList
            };

            return View("PreviousOrderList", previousOrdersVm);
        }
    }
}