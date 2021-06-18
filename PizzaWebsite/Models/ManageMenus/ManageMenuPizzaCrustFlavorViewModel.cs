using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PizzaWebsite.Models.ManageMenus
{
    public class ManageMenuPizzaCrustFlavorViewModel
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