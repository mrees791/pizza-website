using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Required]
        [Display(Name = "Size")]
        public string SelectedSize { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "You must select a crust.")]
        [Display(Name = "Crust")]
        public int SelectedCrustId { get; set; }

        public Dictionary<int, string> CrustDictionary { get; set; }
        public IEnumerable<string> SizeList { get; set; }
        public IEnumerable<int> QuantityList { get; set; }

        public bool IsNewRecord()
        {
            return Id == 0;
        }
    }
}