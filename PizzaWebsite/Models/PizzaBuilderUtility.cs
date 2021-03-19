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