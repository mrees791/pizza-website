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
        public static void LoadNewPizzaBuilderLists(PizzaDatabase pizzaDb, List<PizzaTopping> toppings, PizzaBuilderViewModel pizzaBuilderVm)
        {
            List<MenuPizzaToppingType> toppingTypeList = pizzaDb.GetList<MenuPizzaToppingType>(new { AvailableForPurchase = true }, "SortOrder");
            List<MenuPizzaCrustFlavor> crustFlavorList = pizzaDb.GetList<MenuPizzaCrustFlavor>(new { AvailableForPurchase = true }, "SortOrder").ToList();
            List<MenuPizzaSauce> pizzaSauceList = pizzaDb.GetList<MenuPizzaSauce>(new { AvailableForPurchase = true }, "SortOrder").ToList();
            List<MenuPizzaCheese> pizzaCheeseList = pizzaDb.GetList<MenuPizzaCheese>(new { AvailableForPurchase = true }, "SortOrder").ToList();

            // Save to view model
            pizzaBuilderVm.SauceAmountList = ListUtility.GetSauceAmountList();
            pizzaBuilderVm.CheeseAmountList = ListUtility.GetCheeseAmountList();

            foreach (MenuPizzaCrustFlavor crustFlavor in crustFlavorList)
            {
                pizzaBuilderVm.CrustFlavorList.Add(crustFlavor.Id, crustFlavor.Name);
            }

            foreach (MenuPizzaSauce sauce in pizzaSauceList)
            {
                pizzaBuilderVm.SauceList.Add(sauce.Id, sauce.Name);
            }

            foreach (MenuPizzaCheese cheese in pizzaCheeseList)
            {
                pizzaBuilderVm.CheeseList.Add(cheese.Id, cheese.Name);
            }

            // Create view models for toppings
            for (int iTopping = 0; iTopping < toppingTypeList.Count; iTopping++)
            {
                MenuPizzaToppingType toppingType = toppingTypeList[iTopping];

                PizzaTopping currentTopping = toppings.Where(t => t.ToppingTypeId == toppingType.Id).FirstOrDefault();

                if (currentTopping == null)
                {
                    currentTopping = new PizzaTopping()
                    {
                        ToppingTypeId = toppingType.Id,
                        ToppingHalf = "Whole",
                        ToppingAmount = "None"
                    };
                }

                PizzaToppingViewModel toppingModel = new PizzaToppingViewModel()
                {
                    ListIndex = iTopping,
                    Category = toppingType.CategoryName,
                    Id = toppingType.Id,
                    Name = toppingType.Name,
                    AmountList = ListUtility.GetToppingAmountList(),
                    ToppingHalfList = ListUtility.GetToppingHalfList(),
                    SelectedAmount = currentTopping.ToppingAmount,
                    SelectedToppingHalf = currentTopping.ToppingHalf
                };

                pizzaBuilderVm.ToppingList.Add(toppingModel);
            }
        }
    }
}