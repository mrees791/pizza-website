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
            pizzaBuilderVm.CrustList = new Dictionary<int, MenuPizzaCrust>();
            pizzaBuilderVm.CrustFlavorList = new Dictionary<int, MenuPizzaCrustFlavor>();
            pizzaBuilderVm.SauceList = new Dictionary<int, MenuPizzaSauce>();
            pizzaBuilderVm.CheeseList = new Dictionary<int, MenuPizzaCheese>();
            pizzaBuilderVm.MeatToppingList = new Dictionary<int, PizzaToppingViewModel>();
            pizzaBuilderVm.VeggieToppingList = new Dictionary<int, PizzaToppingViewModel>();

            pizzaBuilderVm.SizeList = ListUtility.GetPizzaSizeList();
            pizzaBuilderVm.SauceAmountList = ListUtility.GetSauceAmountList();
            pizzaBuilderVm.CheeseAmountList = ListUtility.GetCheeseAmountList();

            List<MenuPizzaCrust> menuCrustList = pizzaDb.GetList<MenuPizzaCrust>(new { AvailableForPurchase = true }, "SortOrder");
            List<MenuPizzaCrustFlavor> menuCrustFlavorList = pizzaDb.GetList<MenuPizzaCrustFlavor>(new { AvailableForPurchase = true }, "SortOrder");
            List<MenuPizzaSauce> menuSauceList = pizzaDb.GetList<MenuPizzaSauce>(new { AvailableForPurchase = true }, "SortOrder");
            List<MenuPizzaCheese> menuCheeseList = pizzaDb.GetList<MenuPizzaCheese>(new { AvailableForPurchase = true }, "SortOrder");
            List<MenuPizzaToppingType> meatToppings = pizzaDb.GetList<MenuPizzaToppingType>(new { AvailableForPurchase = true, CategoryName = "Meats" }, "SortOrder");
            List<MenuPizzaToppingType> veggieToppings = pizzaDb.GetList<MenuPizzaToppingType>(new { AvailableForPurchase = true, CategoryName = "Veggie" }, "SortOrder");

            // Create dictionaries for ingredients
            foreach (MenuPizzaCrust crust in menuCrustList)
            {
                pizzaBuilderVm.CrustList.Add(crust.Id, crust);
            }
            foreach (MenuPizzaCrustFlavor crustFlavor in menuCrustFlavorList)
            {
                pizzaBuilderVm.CrustFlavorList.Add(crustFlavor.Id, crustFlavor);
            }
            foreach (MenuPizzaSauce sauce in menuSauceList)
            {
                pizzaBuilderVm.SauceList.Add(sauce.Id, sauce);
            }
            foreach (MenuPizzaCheese cheese in menuCheeseList)
            {
                pizzaBuilderVm.CheeseList.Add(cheese.Id, cheese);
            }

            // Create dictionaries for toppings

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