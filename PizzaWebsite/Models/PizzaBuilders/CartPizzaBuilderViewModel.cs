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
        public List<int> QuantityList { get; set; }
        public List<string> SizeList { get; set; }
        [Required]
        [Display(Name = "Size")]
        public string SelectedSize { get; set; }
        [Display(Name = "Crust")]
        public Dictionary<int, string> CrustList { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "You must select a crust.")]
        public int SelectedCrustId { get; set; }

        public bool IsNewRecord()
        {
            return Id == 0;
        }

        protected override void LoadBuilderLists(PizzaDatabase pizzaDb, List<PizzaTopping> toppings)
        {
            base.LoadBuilderLists(pizzaDb, toppings);

            CrustList = ListUtility.CreateCrustDictionary(pizzaDb);
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

        public void CreateDefault(PizzaDatabase pizzaDb)
        {
            SelectedCheeseAmount = "Regular";
            SelectedSauceAmount = "Regular";
            SelectedSize = "Medium";

            LoadBuilderLists(pizzaDb, new List<PizzaTopping>());
        }

        public void CreateFromEntities(PizzaDatabase pizzaDb, CartItem cartItem, CartPizza cartPizza)
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

            LoadBuilderLists(pizzaDb, toppings);
        }
    }
}