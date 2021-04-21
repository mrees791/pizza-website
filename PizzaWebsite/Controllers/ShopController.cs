using DataLibrary.Models;
using DataLibrary.Models.Joins;
using DataLibrary.Models.Tables;
using DataLibrary.Models.Utility;
using PizzaWebsite.Models;
using PizzaWebsite.Models.Identity.Stores;
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

        [Authorize]
        public async Task<ActionResult> CreateCustomPizza()
        {
            CartPizzaBuilderViewModel cartPizzaVm = new CartPizzaBuilderViewModel();
            List<PizzaTopping> toppings = new List<PizzaTopping>();
            PizzaBuilderUtility.LoadNewPizzaBuilderLists(PizzaDb, toppings, cartPizzaVm);
            await LoadCartPizzaBuilderListsAsync(cartPizzaVm);

            return View("CartPizzaBuilder", cartPizzaVm);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> CreateCustomPizza(CartPizzaBuilderViewModel model)
        {
            if (ModelState.IsValid)
            {
                SiteUser currentUser = await GetCurrentUserAsync();
                CartPizza cartPizza = CreateCartPizzaFromBuilder(model);

                CartItem cartItem = new CartItem()
                {
                    PricePerItem = cartPizza.CalculatePrice(PizzaDb),
                    CartId = currentUser.CurrentCartId,
                    ProductCategory = ProductCategory.Pizza.ToString(),
                    Quantity = model.SelectedQuantity
                };

                CartItemJoin cartItemJoin = new CartItemJoin()
                {
                    CartItem = cartItem,
                    CartItemType = cartPizza
                };

                if (model.IsNewRecord())
                {
                    PizzaDb.Insert(cartItemJoin);
                }
                else
                {
                    PizzaDb.Update(cartItemJoin);
                }

                return RedirectToAction("Cart");
            }
            else
            {
                return View("CartPizzaBuilder", model);
            }
        }

        private CartPizza CreateCartPizzaFromBuilder(CartPizzaBuilderViewModel model)
        {
            CartPizza cartPizza = new CartPizza()
            {
                CheeseAmount = model.SelectedCheeseAmount,
                MenuPizzaCheeseId = model.SelectedCheeseId,
                MenuPizzaCrustFlavorId = model.SelectedCrustFlavorId,
                MenuPizzaCrustId = model.SelectedCrustId,
                MenuPizzaSauceId = model.SelectedSauceId,
                SauceAmount = model.SelectedSauceAmount,
                Size = model.SelectedSize
            };

            foreach (PizzaToppingViewModel toppingVm in model.ToppingList)
            {
                if (toppingVm.SelectedAmount != "None")
                {
                    cartPizza.Toppings.Add(new CartPizzaTopping()
                    {
                        MenuPizzaToppingTypeId = toppingVm.Id,
                        ToppingAmount = toppingVm.SelectedAmount,
                        ToppingHalf = toppingVm.SelectedToppingHalf
                    });
                }
            }

            return cartPizza;
        }

        private async Task LoadCartPizzaBuilderListsAsync(CartPizzaBuilderViewModel cartPizzaVm)
        {
            cartPizzaVm.SizeList = ListUtility.GetPizzaSizeList();
            cartPizzaVm.CrustList = await CreateCrustDictionaryAsync();
            cartPizzaVm.QuantityList = CreateQuantityList();
        }

        [Authorize]
        public async Task<ActionResult> AddMenuPizzaToCurrentCart(int id, int selectedQuantity, string selectedSize, int selectedCrustId)
        {
            SiteUser currentUser = await GetCurrentUserAsync();
            AddMenuPizzaToCart(id, currentUser.CurrentCartId, selectedQuantity, selectedSize, selectedCrustId);
            return RedirectToAction("Cart");
        }

        [Authorize]
        public void AddMenuPizzaToCart(int menuPizzaId, int cartId, int selectedQuantity, string selectedSize, int selectedCrustId)
        {
            MenuPizza menuPizza = PizzaDb.Get<MenuPizza>(menuPizzaId);
            CartItemJoin cartItemJoin = menuPizza.CreateCartRecords(PizzaDb, cartId, selectedQuantity, selectedSize, selectedCrustId);
            PizzaDb.Insert(cartItemJoin);
        }

        private async Task<Dictionary<int, string>> CreateCrustDictionaryAsync()
        {
            List<MenuPizzaCrust> crustList = await PizzaDb.GetListAsync<MenuPizzaCrust>(new { AvailableForPurchase = true }, "SortOrder");
            Dictionary<int, string> crustListDictionary = new Dictionary<int, string>();

            foreach (MenuPizzaCrust crust in crustList)
            {
                crustListDictionary.Add(crust.Id, crust.Name);
            }

            return crustListDictionary;
        }

        public async Task<ActionResult> PizzaMenu()
        {
            PizzaMenuPageViewModel pizzaMenuVm = new PizzaMenuPageViewModel();

            List<int> quantityList = CreateQuantityList();
            List<string> sizeList = ListUtility.GetPizzaSizeList();
            Dictionary<int, string> crustListDictionary = await CreateCrustDictionaryAsync();

            // Load all menu pizzas
            List<MenuPizza> popularMenuPizzas = await PizzaDb.GetListAsync<MenuPizza>(new { AvailableForPurchase = true, CategoryName = "Popular" }, "SortOrder");
            List<MenuPizza> meatsMenuPizzas = await PizzaDb.GetListAsync<MenuPizza>(new { AvailableForPurchase = true, CategoryName = "Meats" }, "SortOrder");
            List<MenuPizza> veggieMenuPizzas = await PizzaDb.GetListAsync<MenuPizza>(new { AvailableForPurchase = true, CategoryName = "Veggie" }, "SortOrder");
            // Separate into categories
            pizzaMenuVm.PopularPizzaList = CreateMenuPizzaViewModels(popularMenuPizzas, quantityList, sizeList, crustListDictionary);
            pizzaMenuVm.MeatsPizzaList = CreateMenuPizzaViewModels(meatsMenuPizzas, quantityList, sizeList, crustListDictionary);
            pizzaMenuVm.VeggiePizzaList = CreateMenuPizzaViewModels(veggieMenuPizzas, quantityList, sizeList, crustListDictionary);

            return View(pizzaMenuVm);
        }

        private List<MenuPizzaViewModel> CreateMenuPizzaViewModels(List<MenuPizza> menuPizzaList, List<int> quantityList, List<string> sizeList, Dictionary<int, string> crustListDictionary)
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

        private List<int> CreateQuantityList()
        {
            List<int> quantityList = new List<int>();
            int maxQuantity = 10;

            for (int i = 1; i <= maxQuantity; i++)
            {
                quantityList.Add(i);
            }

            return quantityList;
        }

        [Authorize]
        public async Task<ActionResult> Cart()
        {
            CartViewModel cartVm = new CartViewModel();
            List<int> quantityList = CreateQuantityList();
            SiteUser user = await GetCurrentUserAsync();
            List<CartItemJoin> cartItemList = await PizzaDb.GetJoinedCartItemsAsync(user.CurrentCartId);

            foreach (CartItemJoin cartItemJoin in cartItemList)
            {
                CartItemViewModel cartItemVm = new CartItemViewModel()
                {
                    CartItemId = cartItemJoin.CartItem.Id,
                    ProductCategory = cartItemJoin.CartItem.ProductCategory,
                    Price = cartItemJoin.CartItem.PricePerItem.ToString("C", CultureInfo.CurrentCulture),
                    Quantity = cartItemJoin.CartItem.Quantity,
                    QuantityList = quantityList,
                    Name = cartItemJoin.CartItemType.GetName(PizzaDb),
                    Description = cartItemJoin.CartItemType.GetDescriptionHtml(PizzaDb),
                    CartItemQuantitySelectId = $"cartItemQuantitySelect-{cartItemJoin.CartItem.Id}",
                    CartItemDeleteButtonId = $"cartItemDeleteButton-{cartItemJoin.CartItem.Id}",
                    CartItemRowId = $"cartItemRow-{cartItemJoin.CartItem.Id}"
                };

                cartVm.CartItemList.Add(cartItemVm);
            }

            return View(cartVm);
        }

        [HttpPost]
        public async Task DeleteCartItem(int cartItemId)
        {
            SiteUser currentUser = await GetCurrentUserAsync();
            bool authorized = await PizzaDb.CmdUserOwnsCartItemAsync(currentUser, cartItemId);

            if (!authorized)
            {
                throw new Exception($"User with ID {currentUser.Id} is not allowed to delete cart item ID {cartItemId}.");
            }

            PizzaDb.CmdDeleteCartItem(cartItemId);
        }

        [HttpPost]
        public async Task UpdateCartItemQuantity(int cartItemId, int quantity)
        {
            SiteUser currentUser = await GetCurrentUserAsync();
            bool authorized = await PizzaDb.CmdUserOwnsCartItemAsync(currentUser, cartItemId);

            if (!authorized)
            {
                throw new Exception($"User with ID {currentUser.Id} is not allowed to modify cart item ID {cartItemId}.");
            }

            PizzaDb.CmdUpdateCartItemQuantity(cartItemId, quantity);
        }
    }
}