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
    /// <summary>
    /// A class which provides properties needed by the cart pizza builder and menu pizza builder.
    /// </summary>
    public abstract class PizzaBuilder
    {
        public IEnumerable<string> CheeseAmountList { get; private set; }
        public IEnumerable<string> SauceAmountList { get; private set; }
        public IEnumerable<MenuPizzaSauce> SauceList { get; private set; }
        public IEnumerable<MenuPizzaCheese> CheeseList { get; private set; }
        public IEnumerable<MenuPizzaCrustFlavor> CrustFlavorList { get; private set; }
        public IEnumerable<MenuPizzaToppingType> ToppingTypeList { get; private set; }

        public virtual async Task InitializeAsync(MenuItemSearch search, PizzaDatabase pizzaDb)
        {
            SauceAmountList = ListUtility.GetSauceAmountList();
            CheeseAmountList = ListUtility.GetCheeseAmountList();
            CrustFlavorList = await pizzaDb.GetListAsync<MenuPizzaCrustFlavor>("SortOrder", SortOrder.Ascending, search);
            SauceList = await pizzaDb.GetListAsync<MenuPizzaSauce>("SortOrder", SortOrder.Ascending, search);
            CheeseList = await pizzaDb.GetListAsync<MenuPizzaCheese>("SortOrder", SortOrder.Ascending, search);
            ToppingTypeList = await pizzaDb.GetListAsync<MenuPizzaToppingType>("SortOrder", SortOrder.Ascending, search);
        }
    }
}
