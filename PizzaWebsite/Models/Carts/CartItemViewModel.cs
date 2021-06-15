using DataLibrary.Models;
using DataLibrary.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PizzaWebsite.Models.Carts
{

    public class CartItemViewModel
    {
        public int CartItemId { get; set; }
        public string Price { get; set; }
        public int Quantity { get; set; }
        public string ProductCategory { get; set; }
        public string CartItemQuantitySelectId { get; set; }
        public string CartItemDeleteButtonId { get; set; }
        public string CartItemRowId { get; set; }
        public string CartItemPriceCellId { get; set; }
        public IEnumerable<int> QuantityList { get; set; }
        public string Name { get; set; }
        public string DescriptionHtml { get; set; }
        public string ModifyActionName { get; set; }
        public string ModifyControllerName { get; set; }
        public bool ControlsEnabled { get; set; }
    }
}