using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models
{
    public class MenuPizzaViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Display(Name = "Size")]
        public string SelectedSize { get; set; }
        public List<string> SizeList { get; set; }
        public Dictionary<int, string> CrustList { get; set; }
        [Display(Name = "Crust")]
        public int SelectedCrustId { get; set; }
        [Display(Name = "Quantity")]
        public int SelectedQuantity { get; set; }
        public List<int> QuantityList { get; set; }
    }

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