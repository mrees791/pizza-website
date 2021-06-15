using System.ComponentModel.DataAnnotations;
using PizzaWebsite.Models.Carts;

namespace PizzaWebsite.Models.Shop
{
    public class PreviousOrderViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Date")]
        public string DateOfOrder { get; set; }

        [Display(Name = "Type")]
        public string OrderType { get; set; }

        [Display(Name = "Total")]
        public string OrderTotal { get; set; }

        public CartViewModel CartVm { get; set; }
    }
}