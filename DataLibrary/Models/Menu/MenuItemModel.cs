﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Menu
{
    public abstract class MenuItemModel
    {
        public int Id { get; set; }
        public bool AvailableForPurchase { get; set; }
        public string Name { get; set; }

        public MenuItemModel(int id, bool availableForPurchase, string name)
        {
            Id = id;
            AvailableForPurchase = availableForPurchase;
            Name = name;
        }
    }
}
