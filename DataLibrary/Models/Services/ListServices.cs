using System.Collections.Generic;
using System.Threading.Tasks;
using DataLibrary.Models.Tables;

namespace DataLibrary.Models.Services
{
    public class ListServices
    {
        public ListServices()
        {
            DeliveryAddressTypeList = new List<string>
            {
                "House", "Business", "Apartment", "Other"
            };
            CustomerOrderTypeList = new List<string>
            {
                "Pickup", "Delivery"
            };
            PizzaCategoryList = new List<string>
            {
                "Popular", "Meats", "Veggie"
            };
            PizzaSizeList = new List<string>
            {
                "Small", "Medium", "Large"
            };
            ToppingCategoryList = new List<string>
            {
                "Meats", "Veggie"
            };
            ToppingAmountList = new List<string>
            {
                "None", "Light", "Regular", "Extra"
            };
            ToppingHalfList = new List<string>
            {
                "Whole", "Left", "Right"
            };
            SauceAmountList = new List<string>
            {
                "None", "Light", "Regular", "Extra"
            };
            CheeseAmountList = new List<string>
            {
                "None", "Light", "Regular", "Extra"
            };
            DefaultQuantityList = CreateQuantityList(10);
        }

        public IEnumerable<string> DeliveryAddressTypeList { get; }

        public IEnumerable<string> CustomerOrderTypeList { get; }

        public IEnumerable<string> PizzaCategoryList { get; }

        public IEnumerable<string> PizzaSizeList { get; }

        public IEnumerable<string> ToppingCategoryList { get; }

        public IEnumerable<string> ToppingAmountList { get; }

        public IEnumerable<string> ToppingHalfList { get; }

        public IEnumerable<string> SauceAmountList { get; }

        public IEnumerable<string> CheeseAmountList { get; }

        public IEnumerable<int> DefaultQuantityList { get; }

        public IEnumerable<int> CreateQuantityList(int max)
        {
            List<int> quantityList = new List<int>();
            for (int i = 1; i <= max; i++)
            {
                quantityList.Add(i);
            }

            return quantityList;
        }

        public async Task<Dictionary<int, string>> CreateCrustDictionaryAsync(PizzaDatabase pizzaDb)
        {
            List<MenuPizzaCrust> crustList = new List<MenuPizzaCrust>(
                await pizzaDb.GetListAsync<MenuPizzaCrust>("order by SortOrder", new {AvailableForPurchase = true}));
            Dictionary<int, string> crustListDictionary = new Dictionary<int, string>();
            foreach (MenuPizzaCrust crust in crustList)
            {
                crustListDictionary.Add(crust.Id, crust.Name);
            }

            return crustListDictionary;
        }
    }
}