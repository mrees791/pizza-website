using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models
{
    // todo: Finish or remove if not needed.
    public class PaginationViewModel
    {
        public int NumberOfItems { get; set; }
        public int ItemsOnPage { get; set; }
        public int CurrentPage { get; set; }
    }
}