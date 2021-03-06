﻿using DataLibrary.Models;
using DataLibrary.Models.Builders;
using DataLibrary.Models.QuerySearches;
using DataLibrary.Models.Tables;
using PizzaWebsite.Models.ManageMenus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PizzaWebsite.Models.PizzaBuilders
{
    public class PizzaBuilderServices
    {
        public List<PizzaToppingViewModel> CreateToppingViewModelList(IEnumerable<PizzaTopping> toppingList, IEnumerable<MenuPizzaToppingType> toppingTypeList,
            IEnumerable<string> toppingAmountList, IEnumerable<string> toppingHalfList)
        {
            List<PizzaToppingViewModel> toppingVmList = new List<PizzaToppingViewModel>();
            Dictionary<int, PizzaTopping> toppingDictionary = new Dictionary<int, PizzaTopping>();
            foreach (PizzaTopping topping in toppingList)
            {
                toppingDictionary.Add(topping.ToppingTypeId, topping);
            }
            for (int iToppingType = 0; iToppingType < toppingTypeList.Count(); iToppingType++)
            {
                MenuPizzaToppingType toppingType = toppingTypeList.ElementAt(iToppingType);
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
                    ListIndex = iToppingType,
                    Category = toppingType.CategoryName,
                    Id = toppingType.Id,
                    Name = toppingType.Name,
                    AmountList = toppingAmountList,
                    ToppingHalfList = toppingHalfList,
                    SelectedAmount = currentTopping.ToppingAmount,
                    SelectedToppingHalf = currentTopping.ToppingHalf
                };
                toppingVmList.Add(toppingVm);
            }
            return toppingVmList;
        }
    }
}