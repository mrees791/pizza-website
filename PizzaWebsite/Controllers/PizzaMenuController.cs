using DataLibrary.Models;
using DataLibrary.Models.Builders;
using DataLibrary.Models.JoinLists.CartItemCategories;
using DataLibrary.Models.QuerySearches;
using DataLibrary.Models.Tables;
using DataLibrary.Models.Utility;
using Microsoft.AspNet.Identity;
using PizzaWebsite.Controllers.BaseControllers;
using PizzaWebsite.Models;
using PizzaWebsite.Models.PizzaBuilders;
using PizzaWebsite.Models.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PizzaWebsite.Controllers
{
    public class PizzaMenuController : BaseController
    {
        public async Task<ActionResult> Index()
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

        private async Task<CartPizzaBuilderViewModel> CreatePizzaBuilderVmAsync(CartItem cartItem, CartPizza cartPizza)
        {
            CartPizzaBuilder pizzaBuilder = new CartPizzaBuilder();
            await pizzaBuilder.InitializeAsync(new MenuItemSearch { AvailableForPurchase = true }, PizzaDb);
            Dictionary<int, string> cheeseDictionary = new Dictionary<int, string>();
            Dictionary<int, string> crustFlavorDictionary = new Dictionary<int, string>();
            Dictionary<int, string> crustDictionary = new Dictionary<int, string>();
            Dictionary<int, string> sauceDictionary = new Dictionary<int, string>();
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
                CartItem prevCartItem = await PizzaDb.GetAsync<CartItem>(model.Id);
                if (prevCartItem == null)
                {
                    ModelState.AddModelError("", $"Cart item with ID {model.Id} does not exist.");
                    return View("CartPizzaBuilder", model);
                }
                bool authorized = await PizzaDb.Commands.UserOwnsCartItemAsync(currentUser.Id, prevCartItem);
                if (!authorized)
                {
                    ModelState.AddModelError("", $"You are not authorized to modify cart item with ID {model.Id}.");
                    return View("CartPizzaBuilder", model);
                }
                await PizzaDb.Commands.UpdateCartItemAsync(cartItem, cartPizza);
            }
            return RedirectToAction("Cart", "Shop");
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> ModifyCartPizza(int cartItemId)
        {
            CartItemOnCartPizzaJoinList joinList = new CartItemOnCartPizzaJoinList();
            await joinList.LoadFirstOrDefaultAsync(cartItemId, PizzaDb);
            CartItemJoin join = joinList.Items.FirstOrDefault();
            if (join == null)
            {
                return RedirectToAction("CartItemDoesNotExistErrorMessage", "Shop");
            }
            bool authorized = await PizzaDb.Commands.UserOwnsCartItemAsync(User.Identity.GetUserId(), join.CartItem);
            if (!authorized)
            {
                return RedirectToAction("CartItemAuthorizationErrorMessage", "Shop");
            }
            CartPizzaBuilderViewModel model = await CreatePizzaBuilderVmAsync(join.CartItem, (CartPizza)join.CartItemType);
            return View("CartPizzaBuilder", model);
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
            MenuPizza menuPizza = await PizzaDb.GetAsync<MenuPizza>(id);
            if (menuPizza == null)
            {
                return MenuPizzaDoesNotExistErrorMessage(id);
            }
            SiteUser currentUser = await GetCurrentUserAsync();
            Tuple<CartItem, CartPizza> cartItemRecords = await menuPizza.CreateCartRecordsAsync(selectedQuantity, selectedSize, selectedCrustId, currentUser, PizzaDb);
            await PizzaDb.Commands.AddItemToCart(currentUser, cartItemRecords.Item1, cartItemRecords.Item2);
            return RedirectToAction("Cart", "Shop");
        }

        private ActionResult MenuPizzaDoesNotExistErrorMessage(int id)
        {
            ErrorMessageViewModel model = new ErrorMessageViewModel()
            {
                Header = "Error",
                ErrorMessage = $"A pizza with ID {id} does not exist.",
                ReturnUrlAction = $"{Url.Action("Index")}",
                ShowReturnLink = true
            };
            return View("ErrorMessage", model);
        }
    }
}