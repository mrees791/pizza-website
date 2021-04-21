using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PizzaWebsite.Models.PizzaBuilders
{
    public class CartPizzaBuilderViewModel : PizzaBuilderViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Quantity")]
        public int SelectedQuantity { get; set; }
        public List<int> QuantityList { get; set; }
        public List<string> SizeList { get; set; }
        [Required]
        [Display(Name = "Size")]
        public string SelectedSize { get; set; }
        [Display(Name = "Crust")]
        public Dictionary<int, string> CrustList { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "You must select a crust.")]
        public int SelectedCrustId { get; set; }

        public bool IsNewRecord()
        {
            return Id == 0;
        }
    }
}