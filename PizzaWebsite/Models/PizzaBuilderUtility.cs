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
            List<MenuPizzaCrust> crustList = pizzaDb.GetList<MenuPizzaCrust>(new { AvailableForPurchase = true }, "SortOrder");
            pizzaBuilderVm.SauceAmountList = ListUtility.GetSauceAmountList();
            pizzaBuilderVm.CheeseAmountList = ListUtility.GetCheeseAmountList();
            pizzaBuilderVm.CrustList = new Dictionary<int, string>();
            pizzaBuilderVm.CrustFlavorList = pizzaDb.GetList<MenuPizzaCrustFlavor>(new { AvailableForPurchase = true }, "SortOrder").Select(f => f.Name).ToList();
            pizzaBuilderVm.SauceList = pizzaDb.GetList<MenuPizzaSauce>(new { AvailableForPurchase = true }, "SortOrder").Select(s => s.Name).ToList();
            pizzaBuilderVm.CheeseList = pizzaDb.GetList<MenuPizzaCheese>(new { AvailableForPurchase = true }, "SortOrder").Select(c => c.Name).ToList();

            foreach (MenuPizzaCrust crust in crustList)
            {
                pizzaBuilderVm.CrustList.Add(crust.Id, crust.Name);
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