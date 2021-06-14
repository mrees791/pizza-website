using DataLibrary.Models;
using DataLibrary.Models.Tables;
using PizzaWebsite.Models.Carts;
using PizzaWebsite.Models.ViewModelServices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

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