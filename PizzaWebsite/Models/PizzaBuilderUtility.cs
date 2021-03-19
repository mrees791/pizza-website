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
            pizzaBuilderVm.PizzaSizeList = ListUtility.GetPizzaSizeList();
            pizzaBuilderVm.PizzaCrustList = pizzaDb.GetList<MenuPizzaCrust>(new { AvailableForPurchase = true }, "SortOrder");
            //pizzaBuilderVm.PizzaCrustList = pizzaDb.GetList<MenuPizzaCrust>();
        }
    }
}