using DataLibrary.Models;
using DataLibrary.Models.Joins;
using DataLibrary.Models.Tables;
using DataLibrary.Models.Utility;
using PizzaWebsite.Models;
using PizzaWebsite.Models.Identity.Stores;
using PizzaWebsite.Models.PizzaBuilders;
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
        public ActionResult Checkout()
        {
            // Run checkout cart command from DataLibrary.
            // todo: Finish
            throw new NotImplementedException();
        }

        private async Task<bool> AuthorizedToModifyCartItemAsync(int cartItemId)
        {
            return await PizzaDb.CmdUserOwnsCartItemAsync(await GetCurrentUserAsync(), cartItemId);
        }

        private async Task<CartPizzaBuilderViewModel> CreatePizzaBuilderVm(int cartItemId)
        {
            CartItem cartItem = await PizzaDb.GetAsync<CartItem>(cartItemId);
            CartPizza cartPizza = await PizzaDb.GetAsync<CartPizza>(cartItemId);

            CartPizzaBuilderViewModel viewModel = new CartPizzaBuilderViewModel();
            viewModel.CreateFromEntities(PizzaDb, cartItem, cartPizza);

            return viewModel;
        }

        [Authorize]
        public ActionResult BuildPizza()
        {
            CartPizzaBuilderViewModel cartPizzaVm = new CartPizzaBuilderViewModel();
            cartPizzaVm.CreateDefault(PizzaDb);

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

                CartItem cartItem = new CartItem()
                {
                    Id = model.Id,
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

        public async Task<ActionResult> PizzaMenu()
        {
            PizzaMenuPageViewModel pizzaMenuVm = new PizzaMenuPageViewModel();

            List<int> quantityList = ListUtility.CreateQuantityList();
            List<string> sizeList = ListUtility.GetPizzaSizeList();
            Dictionary<int, string> crustListDictionary = ListUtility.CreateCrustDictionary(PizzaDb);

            // Load all menu pizzas in to each category.
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

        [Authorize]
        public async Task<ActionResult> Cart()
        {
            CartViewModel cartVm = new CartViewModel();
            List<int> quantityList = ListUtility.CreateQuantityList();
            SiteUser user = await GetCurrentUserAsync();
            List<CartItemJoin> cartItemList = await PizzaDb.GetJoinedCartItemListAsync(user.CurrentCartId);

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
            bool authorized = await AuthorizedToModifyCartItemAsync(cartItemId);

            if (!authorized)
            {
                throw new Exception($"Current user is not allowed to delete cart item ID {cartItemId}.");
            }

            PizzaDb.Delete<CartItem>(cartItemId);
        }

        [HttpPost]
        public async Task UpdateCartItemQuantity(int cartItemId, int quantity)
        {
            bool authorized = await AuthorizedToModifyCartItemAsync(cartItemId);

            if (!authorized)
            {
                throw new Exception($"Current user is not allowed to modify cart item ID {cartItemId}.");
            }

            PizzaDb.CmdUpdateCartItemQuantity(cartItemId, quantity);
        }
    }
}