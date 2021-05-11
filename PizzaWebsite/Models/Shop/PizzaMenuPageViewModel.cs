using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.Shop
{
    public class PizzaMenuPageViewModel
    {
        public List<MenuPizzaViewModel> PopularPizzaList { get; set; }
        public List<MenuPizzaViewModel> MeatsPizzaList { get; set; }
        public List<MenuPizzaViewModel> VeggiePizzaList { get; set; }

        public PizzaMenuPageViewModel()
        {
            PopularPizzaList = new List<MenuPizzaViewModel>();
            MeatsPizzaList = new List<MenuPizzaViewModel>();
            VeggiePizzaList = new List<MenuPizzaViewModel>();
        }
    }
}