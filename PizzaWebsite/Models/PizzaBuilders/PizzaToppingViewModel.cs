using System.Collections.Generic;

namespace PizzaWebsite.Models.PizzaBuilders
{
    public class PizzaToppingViewModel
    {
        public int ListIndex { get; set; }
        public int Id { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public string SelectedAmount { get; set; }
        public string SelectedToppingHalf { get; set; }
        public IEnumerable<string> AmountList { get; set; }
        public IEnumerable<string> ToppingHalfList { get; set; }
    }
}