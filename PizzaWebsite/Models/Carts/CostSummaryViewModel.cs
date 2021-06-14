using DataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.Carts
{
    public class CostSummaryViewModel
    {
        public string Subtotal { get; set; }
        public string Tax { get; set; }
        public string Total { get; set; }
    }
}