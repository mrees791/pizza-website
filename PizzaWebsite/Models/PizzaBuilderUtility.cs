using DataLibrary.Models;
using DataLibrary.Models.Tables;
using DataLibrary.Models.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PizzaWebsite.Models
{
    public static class PizzaBuilderUtility
    {
        // todo: May make this async.
        public static void LoadPizzaBuilderLists(PizzaDatabase pizzaDb, PizzaBuilderViewModel pizzaBuilderVm)
        {
            pizzaBuilderVm.SizeList = ListUtility.GetPizzaSizeList();
            pizzaBuilderVm.SauceAmountList = ListUtility.GetSauceAmountList();
            pizzaBuilderVm.CheeseAmountList = ListUtility.GetCheeseAmountList();
            pizzaBuilderVm.CrustList = pizzaDb.GetList<MenuPizzaCrust>(new { AvailableForPurchase = true }, "SortOrder");
            pizzaBuilderVm.SauceList = pizzaDb.GetList<MenuPizzaSauce>(new { AvailableForPurchase = true }, "SortOrder");
            pizzaBuilderVm.CheeseList = pizzaDb.GetList<MenuPizzaCheese>(new { AvailableForPurchase = true }, "SortOrder");
            pizzaBuilderVm.CrustFlavorList = pizzaDb.GetList<MenuPizzaCrustFlavor>(new { AvailableForPurchase = true }, "SortOrder");

            // Create lists for toppings
            List<MenuPizzaToppingType> meatToppings = pizzaDb.GetList<MenuPizzaToppingType>(new { AvailableForPurchase = true, CategoryName = "Meats" }, "SortOrder");
            List<MenuPizzaToppingType> veggieToppings = pizzaDb.GetList<MenuPizzaToppingType>(new { AvailableForPurchase = true, CategoryName = "Veggie" }, "SortOrder");
            pizzaBuilderVm.MeatToppingList = new Dictionary<int, PizzaToppingViewModel>();
            pizzaBuilderVm.VeggieToppingList = new Dictionary<int, PizzaToppingViewModel>();

            foreach (MenuPizzaToppingType topping in meatToppings)
            {
                pizzaBuilderVm.MeatToppingList.Add(
                    topping.Id,
                    new PizzaToppingViewModel()
                {
                    FormName = $"MeatToppingList[{topping.Id}]",
                    Id = topping.Id,
                    Name = topping.Name,
                    AmountList = ListUtility.GetToppingAmountList(),
                    ToppingHalfList = ListUtility.GetToppingHalfList()
                });
            }
            foreach (MenuPizzaToppingType topping in veggieToppings)
            {
                pizzaBuilderVm.VeggieToppingList.Add(
                    topping.Id,
                    new PizzaToppingViewModel()
                {
                    FormName = $"VeggieToppingList[{topping.Id}]",
                    Id = topping.Id,
                    Name = topping.Name,
                    AmountList = ListUtility.GetToppingAmountList(),
                    ToppingHalfList = ListUtility.GetToppingHalfList()
                });
            }
        }
    }
}