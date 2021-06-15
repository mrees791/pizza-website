using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.Shop
{
    public class PizzaMenuPageViewModel
    {
        public IEnumerable<MenuPizzaViewModel> PopularPizzaList { get; set; }
        public IEnumerable<MenuPizzaViewModel> MeatsPizzaList { get; set; }
        public IEnumerable<MenuPizzaViewModel> VeggiePizzaList { get; set; }
    }
}