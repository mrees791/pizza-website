﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Menu.Pizza
{
    public class MenuPizzaCheeseModel : MenuPizzaItemModel
    {
        public decimal PriceLight { get; set; }
        public decimal PriceRegular { get; set; }
        public decimal PriceExtra { get; set; }

        public MenuPizzaCheeseModel(int id, bool availableForPurchase, string name, bool hasMenuIcon, string menuIconFile,
            bool hasPizzaBuilderImage, string pizzaBuilderImageFile, string description, decimal priceLight, decimal priceRegular, decimal priceExtra) :
            base(id, availableForPurchase, name, hasMenuIcon, menuIconFile, hasPizzaBuilderImage, pizzaBuilderImageFile, description)
        {
            PriceLight = priceLight;
            PriceRegular = priceRegular;
            PriceExtra = priceExtra;
        }
    }
}
