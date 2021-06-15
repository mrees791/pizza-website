using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.Shop
{
    public class MenuPizzaViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Display(Name = "Size")]
        public string SelectedSize { get; set; }
        public IEnumerable<string> SizeList { get; set; }
        public Dictionary<int, string> CrustList { get; set; }
        [Display(Name = "Crust")]
        public int SelectedCrustId { get; set; }
        [Display(Name = "Quantity")]
        public int SelectedQuantity { get; set; }
        public IEnumerable<int> QuantityList { get; set; }
    }
}