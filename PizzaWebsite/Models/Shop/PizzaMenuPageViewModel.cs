using System.Collections.Generic;

namespace PizzaWebsite.Models.Shop
{
    public class PizzaMenuPageViewModel
    {
        public IEnumerable<MenuPizzaViewModel> PopularPizzaList { get; set; }
        public IEnumerable<MenuPizzaViewModel> MeatsPizzaList { get; set; }
        public IEnumerable<MenuPizzaViewModel> VeggiePizzaList { get; set; }
    }
}