using System.Collections.Generic;
using System.Threading.Tasks;
using DataLibrary.Models.QuerySearches;
using DataLibrary.Models.Services;
using DataLibrary.Models.Tables;

namespace DataLibrary.Models.Builders
{
    /// <summary>
    ///     A class which provides properties needed by the cart pizza builder and menu pizza builder.
    /// </summary>
    public abstract class PizzaBuilder
    {
        protected ListServices ListServices;

        protected PizzaBuilder()
        {
            ListServices = new ListServices();
        }

        public IEnumerable<string> CheeseAmountList { get; private set; }
        public IEnumerable<string> SauceAmountList { get; private set; }
        public IEnumerable<MenuPizzaSauce> SauceList { get; private set; }
        public IEnumerable<MenuPizzaCheese> CheeseList { get; private set; }
        public IEnumerable<MenuPizzaCrustFlavor> CrustFlavorList { get; private set; }
        public IEnumerable<MenuPizzaToppingType> ToppingTypeList { get; private set; }

        public virtual async Task InitializeAsync(MenuItemSearch search, PizzaDatabase pizzaDb)
        {
            SauceAmountList = ListServices.SauceAmountList;
            CheeseAmountList = ListServices.CheeseAmountList;
            CrustFlavorList =
                await pizzaDb.GetListAsync<MenuPizzaCrustFlavor>("SortOrder", SortOrder.Ascending, search);
            SauceList = await pizzaDb.GetListAsync<MenuPizzaSauce>("SortOrder", SortOrder.Ascending, search);
            CheeseList = await pizzaDb.GetListAsync<MenuPizzaCheese>("SortOrder", SortOrder.Ascending, search);
            ToppingTypeList =
                await pizzaDb.GetListAsync<MenuPizzaToppingType>("SortOrder", SortOrder.Ascending, search);
        }
    }
}