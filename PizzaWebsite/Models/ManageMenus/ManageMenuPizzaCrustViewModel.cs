using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PizzaWebsite.Models.ManageMenus
{
    public class ManageMenuPizzaCrustViewModel
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

        [Display(Name = "Price (Small)")]
        [Range(0.01, 100.0, ErrorMessage = "Price must be between $0.01 and $100.00.")]
        public decimal PriceSmall { get; set; }

        [Display(Name = "Price (Medium)")]
        [Range(0.01, 100.0, ErrorMessage = "Price must be between $0.01 and $100.00.")]
        public decimal PriceMedium { get; set; }

        [Display(Name = "Price (Large)")]
        [Range(0.01, 100.0, ErrorMessage = "Price must be between $0.01 and $100.00.")]
        public decimal PriceLarge { get; set; }

        [Required]
        [Display(Name = "Description")]
        [MaxLength(512)]
        public string Description { get; set; }

        public bool HasMenuIcon { get; set; }
        public string MenuIconFile { get; set; }
        public bool HasPizzaBuilderImage { get; set; }
        public string PizzaBuilderImageFile { get; set; }

        public bool IsNewRecord()
        {
            return Id == 0;
        }
    }
}