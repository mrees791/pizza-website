using DataLibrary.Models.QuerySearches;
using DataLibrary.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            QuantityList = listServices.DefaultQuantityList;
            SizeList = listServices.PizzaSizeList;
            CrustList = await pizzaDb.GetListAsync<MenuPizzaCrust>("SortOrder", SortOrder.Ascending, search);
        }
    }
}
