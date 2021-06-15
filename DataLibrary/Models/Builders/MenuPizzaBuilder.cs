using System.Collections.Generic;
using System.Threading.Tasks;
using DataLibrary.Models.QuerySearches;

namespace DataLibrary.Models.Builders
{
    public class MenuPizzaBuilder : PizzaBuilder
    {
        public IEnumerable<string> CategoryList { get; private set; }

        public override async Task InitializeAsync(MenuItemSearch search, PizzaDatabase pizzaDb)
        {
            await base.InitializeAsync(search, pizzaDb);
            CategoryList = ListServices.PizzaCategoryList;
        }
    }
}