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
    public class PizzaBuilderManager
    {
        public List<PizzaToppingViewModel> CreateToppingViewModelList(IEnumerable<PizzaTopping> toppings, IEnumerable<MenuPizzaToppingType> toppingTypeList)
        {
            List<PizzaToppingViewModel> toppingVmList = new List<PizzaToppingViewModel>();
            foreach (MenuPizzaToppingType toppingType in toppingTypeList)
            {
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