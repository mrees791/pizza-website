using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PizzaWebsite.Models.ManageMenus
{
    public class ManageMenuPizzaSauceViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int SortOrder { get; set; }

        [Required]
        [Display(Name = "Name")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Display(Name = "Available for Purchase")]
        public bool AvailableForPurchase { get; set; }

        [Display(Name = "Price (Light Amount)")]
        [Range(0.01, 100.0, ErrorMessage = "Price must be between $0.01 and $100.00.")]
        public decimal PriceLight { get; set; }

        [Display(Name = "Price (Regular Amount)")]
        [Range(0.01, 100.0, ErrorMessage = "Price must be between $0.01 and $100.00.")]
        public decimal PriceRegular { get; set; }

        [Display(Name = "Price (Extra Amount)")]
        [Range(0.01, 100.0, ErrorMessage = "Price must be between $0.01 and $100.00.")]
        public decimal PriceExtra { get; set; }

        [Required]
        [Display(Name = "Description")]
        [MaxLength(512)]
        public string Description { get; set; }

        public bool IsNewRecord()
        {
            return Id == 0;
        }
    }
}