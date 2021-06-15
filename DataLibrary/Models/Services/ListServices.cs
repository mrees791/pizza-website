using DataLibrary.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Services
{
    public class ListServices
    {
        private IEnumerable<string> _deliveryAddressTypeList;
        private IEnumerable<string> _customerOrderTypeList;
        private IEnumerable<string> _pizzaCategoryList;
        private IEnumerable<string> _pizzaSizeList;
        private IEnumerable<string> _toppingCategoryList;
        private IEnumerable<string> _toppingAmountList;
        private IEnumerable<string> _toppingHalfList;
        private IEnumerable<string> _sauceAmountList;
        private IEnumerable<string> _cheeseAmountList;
        private IEnumerable<int> _defaultQuantityList;

        public ListServices()
        {
            _deliveryAddressTypeList = new List<string>()
            {
                "House", "Business", "Apartment", "Other"
            };
            _customerOrderTypeList = new List<string>()
            {
                "Pickup", "Delivery"
            };
            _pizzaCategoryList = new List<string>()
            {
                "Popular", "Meats", "Veggie"
            };
            _pizzaSizeList = new List<string>()
            {
                "Small", "Medium", "Large"
            };
            _toppingCategoryList = new List<string>()
            {
                "Meats", "Veggie"
            };
            _toppingAmountList = new List<string>()
            {
                "None", "Light", "Regular", "Extra"
            };
            _toppingHalfList = new List<string>()
            {
                "Whole", "Left", "Right"
            };
            _sauceAmountList = new List<string>()
            {
                "None", "Light", "Regular", "Extra"
            };
            _cheeseAmountList = new List<string>()
            {
                "None", "Light", "Regular", "Extra"
            };
            _defaultQuantityList = CreateQuantityList(10);
        }

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
            List<MenuPizzaCrust> crustList = new List<MenuPizzaCrust>(await pizzaDb.GetListAsync<MenuPizzaCrust>("order by SortOrder", new { AvailableForPurchase = true }));
            Dictionary<int, string> crustListDictionary = new Dictionary<int, string>();
            foreach (MenuPizzaCrust crust in crustList)
            {
                crustListDictionary.Add(crust.Id, crust.Name);
            }
            return crustListDictionary;
        }

        public IEnumerable<string> DeliveryAddressTypeList { get => _deliveryAddressTypeList; }
        public IEnumerable<string> CustomerOrderTypeList { get => _customerOrderTypeList; }
        public IEnumerable<string> PizzaCategoryList { get => _pizzaCategoryList; }
        public IEnumerable<string> PizzaSizeList { get => _pizzaSizeList; }
        public IEnumerable<string> ToppingCategoryList { get => _toppingCategoryList; }
        public IEnumerable<string> ToppingAmountList { get => _toppingAmountList; }
        public IEnumerable<string> ToppingHalfList { get => _toppingHalfList; }
        public IEnumerable<string> SauceAmountList { get => _sauceAmountList; }
        public IEnumerable<string> CheeseAmountList { get => _cheeseAmountList; }
        public IEnumerable<int> DefaultQuantityList { get => _defaultQuantityList; }
    }
}
