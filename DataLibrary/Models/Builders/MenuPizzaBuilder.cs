using DataLibrary.Models.QuerySearches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Builders
{
    public class MenuPizzaBuilder : PizzaBuilder
    {
        public IEnumerable<string> CategoryList { get; private set; }

        public override async Task InitializeAsync(MenuItemSearch search, PizzaDatabase pizzaDb)
        {
            await base.InitializeAsync(search, pizzaDb);
            CategoryList = listServices.PizzaCategoryList;
        }
    }
}
