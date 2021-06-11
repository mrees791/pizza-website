using DataLibrary.Models;
using DataLibrary.Models.Tables;
using DataLibrary.Models.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PizzaWebsite.Models.PizzaBuilders
{
    public class CartPizzaBuilderViewModel : PizzaBuilderViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Quantity")]
        public int SelectedQuantity { get; set; }
        [Required]
        [Display(Name = "Size")]
        public string SelectedSize { get; set; }
        public Dictionary<int, string> CrustList { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "You must select a crust.")]
        [Display(Name = "Crust")]
        public int SelectedCrustId { get; set; }

        public bool IsNewRecord()
        {
            return Id == 0;
        }

        protected override async Task LoadBuilderListsAsync(PizzaDatabase pizzaDb, List<PizzaTopping> toppings)
        {
            await base.LoadBuilderListsAsync(pizzaDb, toppings);

            CrustList = await ListUtility.CreateCrustDictionaryAsync(pizzaDb);
            SizeList = ListUtility.GetPizzaSizeList();
            QuantityList = ListUtility.CreateQuantityList();
        }

        public CartPizza ToCartPizza()
        {
            CartPizza cartPizza = new CartPizza()
            {
                CartItemId = Id,
                CheeseAmount = SelectedCheeseAmount,
                MenuPizzaCheeseId = SelectedCheeseId,
                MenuPizzaCrustFlavorId = SelectedCrustFlavorId,
                MenuPizzaCrustId = SelectedCrustId,
                MenuPizzaSauceId = SelectedSauceId,
                SauceAmount = SelectedSauceAmount,
                Size = SelectedSize
            };

            foreach (PizzaToppingViewModel toppingVm in ToppingList)
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

        public async Task CreateDefaultAsync(PizzaDatabase pizzaDb)
        {
            SelectedCheeseAmount = "Regular";
            SelectedSauceAmount = "Regular";
            SelectedSize = "Medium";

            await LoadBuilderListsAsync(pizzaDb, new List<PizzaTopping>());
        }

        public async Task CreateFromRecordsAsync(PizzaDatabase pizzaDb, CartItem cartItem, CartPizza cartPizza)
        {
            Id = cartItem.Id;
            SelectedQuantity = cartItem.Quantity;
            SelectedCheeseAmount = cartPizza.CheeseAmount;
            SelectedCheeseId = cartPizza.MenuPizzaCheeseId;
            SelectedCrustFlavorId = cartPizza.MenuPizzaCrustFlavorId;
            SelectedCrustId = cartPizza.MenuPizzaCrustId;
            SelectedSauceAmount = cartPizza.SauceAmount;
            SelectedSauceId = cartPizza.MenuPizzaSauceId;
            SelectedSize = cartPizza.Size;

            List<PizzaTopping> toppings = new List<PizzaTopping>();

            foreach (CartPizzaTopping cartTopping in cartPizza.Toppings)
            {
                toppings.Add(new PizzaTopping()
                {
                    ToppingTypeId = cartTopping.MenuPizzaToppingTypeId,
                    ToppingAmount = cartTopping.ToppingAmount,
                    ToppingHalf = cartTopping.ToppingHalf
                });
            }

            await LoadBuilderListsAsync(pizzaDb, toppings);
        }
    }
}