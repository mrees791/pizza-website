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
        public static void LoadPizzaBuilderLists(PizzaDatabase pizzaDb, PizzaBuilderViewModel pizzaBuilderVm)
        {
            pizzaBuilderVm.SizeList = ListUtility.GetPizzaSizeList();
            pizzaBuilderVm.CrustList = pizzaDb.GetList<MenuPizzaCrust>(new { AvailableForPurchase = true }, "SortOrder");

            List<MenuPizzaToppingType> meatToppings = pizzaDb.GetList<MenuPizzaToppingType>(new { AvailableForPurchase = true, CategoryName = "Meats" }, "SortOrder");
            List<MenuPizzaToppingType> veggieToppings = pizzaDb.GetList<MenuPizzaToppingType>(new { AvailableForPurchase = true, CategoryName = "Veggie" }, "SortOrder");
            pizzaBuilderVm.MeatToppingList = new List<PizzaToppingViewModel>();
            pizzaBuilderVm.VeggieToppingList = new List<PizzaToppingViewModel>();

            for (int iTopping = 0; iTopping < meatToppings.Count; iTopping++)
            {
                MenuPizzaToppingType topping = meatToppings[iTopping];

                pizzaBuilderVm.MeatToppingList.Add(new PizzaToppingViewModel()
                {
                    FormName = $"MeatToppingList[{iTopping}]",
                    Id = topping.Id,
                    Name = topping.Name,
                    AmountList = ListUtility.GetToppingAmountList(),
                    ToppingHalfList = ListUtility.GetToppingHalfList()
                });
            }
            for (int iTopping = 0; iTopping < veggieToppings.Count; iTopping++)
            {
                MenuPizzaToppingType topping = veggieToppings[iTopping];

                pizzaBuilderVm.VeggieToppingList.Add(new PizzaToppingViewModel()
                {
                    FormName = $"VeggieToppingList[{iTopping}]",
                    Id = topping.Id,
                    Name = topping.Name,
                    AmountList = ListUtility.GetToppingAmountList(),
                    ToppingHalfList = ListUtility.GetToppingHalfList()
                });
            }
        }
    }
}