using System.Collections.Generic;
using System.Threading.Tasks;
using DataLibrary.Models.QuerySearches;
using DataLibrary.Models.Tables;

namespace DataLibrary.Models.Builders
{
    public class CartPizzaBuilder : PizzaBuilder
    {
        public IEnumerable<int> QuantityList { get; private set; }
        public IEnumerable<string> SizeList { get; private set; }
        public IEnumerable<MenuPizzaCrust> CrustList { get; private set; }

        public override async Task InitializeAsync(MenuItemSearch search, PizzaDatabase pizzaDb)
        {
            await base.InitializeAsync(search, pizzaDb);
            QuantityList = ListServices.DefaultQuantityList;
            SizeList = ListServices.PizzaSizeList;
            CrustList = await pizzaDb.GetListAsync<MenuPizzaCrust>("SortOrder", SortOrder.Ascending, search);
        }
    }
}