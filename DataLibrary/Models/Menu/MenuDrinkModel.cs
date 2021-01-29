﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Menu
{
    public class MenuDrinkModel : MenuItemModel
    {
        public bool AvailableIn20Oz { get; set; }
        public bool AvailableIn2Liter { get; set; }
        public bool AvailableIn2Pack12Oz { get; set; }
        public bool AvailableIn6Pack12Oz { get; set; }
        public decimal Price20Oz { get; set; }
        public decimal Price2Liter { get; set; }
        public decimal Price2Pack12Oz { get; set; }
        public decimal Price6Pack12Oz { get; set; }
        public string Description { get; set; }
    }
}
