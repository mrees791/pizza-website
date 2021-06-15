using System.Globalization;
using DataLibrary.Models;
using PizzaWebsite.Models.Carts;

namespace PizzaWebsite.Models.ViewModelServices
{
    public class CostSummaryServices
    {
        public CostSummaryViewModel CreateViewModel(CostSummary costSummary)
        {
            return new CostSummaryViewModel
            {
                Subtotal = costSummary.Subtotal.ToString("C", CultureInfo.CurrentCulture),
                Tax = costSummary.Tax.ToString("C", CultureInfo.CurrentCulture),
                Total = costSummary.Total.ToString("C", CultureInfo.CurrentCulture)
            };
        }
    }
}