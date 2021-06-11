using DataLibrary.Models.QuerySearches;
using DataLibrary.Models.Tables;
using DataLibrary.Models.Utility;
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
            QuantityList = ListUtility.CreateQuantityList();
            SizeList = ListUtility.GetPizzaSizeList();
            CrustList = await pizzaDb.GetListAsync<MenuPizzaCrust>("SortOrder", SortOrder.Ascending, search);
        }
    }
}
