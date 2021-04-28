﻿using DataLibrary.Models;
using DataLibrary.Models.Tables;
using DataLibrary.Models.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PizzaWebsite.Models.PizzaBuilders
{
    public class MenuPizzaBuilderViewModel : PizzaBuilderViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int SortOrder { get; set; }
        [Required]
        [Display(Name = "Name")]
        [MaxLength(100)]
        public string Name { get; set; }
        [Display(Name = "Available for Purchase")]
        public bool AvailableForPurchase { get; set; }
        public List<string> CategoryList { get; set; }
        [Required]
        [Display(Name = "Category")]
        public string SelectedCategory { get; set; }
        [Required]
        [Display(Name = "Description")]
        [MaxLength(512)]
        public string Description { get; set; }

        public bool IsNewRecord()
        {
            return Id == 0;
        }

        protected override void LoadBuilderLists(PizzaDatabase pizzaDb, List<PizzaTopping> toppings)
        {
            base.LoadBuilderLists(pizzaDb, toppings);

            CategoryList = ListUtility.GetPizzaCategoryList();
        }

        public void CreateFromEntity(PizzaDatabase pizzaDb, MenuPizza menuPizza)
        {
            Id = menuPizza.Id;
            Name = menuPizza.PizzaName;
            AvailableForPurchase = menuPizza.AvailableForPurchase;
            SelectedCategory = menuPizza.CategoryName;
            Description = menuPizza.Description;
            SelectedCheeseAmount = menuPizza.CheeseAmount;
            SelectedCheeseId = menuPizza.MenuPizzaCheeseId;
            SelectedCrustFlavorId = menuPizza.MenuPizzaCrustFlavorId;
            SelectedSauceId = menuPizza.MenuPizzaSauceId;
            SelectedSauceAmount = menuPizza.SauceAmount;

            List<PizzaTopping> toppings = new List<PizzaTopping>();

            foreach (MenuPizzaTopping topping in menuPizza.Toppings)
            {
                toppings.Add(new PizzaTopping()
                {
                    ToppingTypeId = topping.MenuPizzaToppingTypeId,
                    ToppingAmount = topping.ToppingAmount,
                    ToppingHalf = topping.ToppingHalf
                });
            }

            LoadBuilderLists(pizzaDb, toppings);
        }
    }
}