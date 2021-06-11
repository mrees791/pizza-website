using DataLibrary.Models;
using DataLibrary.Models.Builders;
using DataLibrary.Models.QuerySearches;
using DataLibrary.Models.Tables;
using DataLibrary.Models.Utility;
using PizzaWebsite.Models.ManageMenus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PizzaWebsite.Models.PizzaBuilders
{
    public static class PizzaBuilderManager
    {
        public static List<PizzaToppingViewModel> CreateToppingViewModelList(Dictionary<int, PizzaTopping> toppingDictionary, IEnumerable<MenuPizzaToppingType> toppingTypeList)
        {
            List<PizzaToppingViewModel> toppingVmList = new List<PizzaToppingViewModel>();
            foreach (MenuPizzaToppingType toppingType in toppingTypeList)
            {
                PizzaTopping currentTopping = new PizzaTopping()
                {
                    ToppingTypeId = toppingType.Id,
                    ToppingHalf = "Whole",
                    ToppingAmount = "None"
                };
                if (toppingDictionary.ContainsKey(toppingType.Id))
                {
                    currentTopping = toppingDictionary[toppingType.Id];
                }
                PizzaToppingViewModel toppingVm = new PizzaToppingViewModel()
                {
                    Category = toppingType.CategoryName,
                    Id = toppingType.Id,
                    Name = toppingType.Name,
                    AmountList = ListUtility.GetToppingAmountList(),
                    ToppingHalfList = ListUtility.GetToppingHalfList(),
                    SelectedAmount = currentTopping.ToppingAmount,
                    SelectedToppingHalf = currentTopping.ToppingHalf
                };
                toppingVmList.Add(toppingVm);
            }
            return toppingVmList;
        }
    }
}