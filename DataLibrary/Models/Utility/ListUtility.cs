using DataLibrary.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Utility
{
    public static class ListUtility
    {
        public static List<string> CreateCustomerOrderTypeList()
        {
            return new List<string>()
            {
                "Pickup", "Delivery"
            };
        }

        public static List<string> CreateDeliveryAddressTypeList()
        {
            return new List<string>()
            {
                "House", "Business", "Apartment", "Other"
            };
        }

        public static async Task<Dictionary<int, string>> CreateCrustDictionaryAsync(PizzaDatabase pizzaDb)
        {
            List<MenuPizzaCrust> crustList = new List<MenuPizzaCrust>(await pizzaDb.GetListAsync<MenuPizzaCrust>("order by SortOrder", new { AvailableForPurchase = true }));
            Dictionary<int, string> crustListDictionary = new Dictionary<int, string>();

            foreach (MenuPizzaCrust crust in crustList)
            {
                crustListDictionary.Add(crust.Id, crust.Name);
            }

            return crustListDictionary;
        }

        public static List<int> CreateQuantityList()
        {
            List<int> quantityList = new List<int>();
            int maxQuantity = 10;

            for (int i = 1; i <= maxQuantity; i++)
            {
                quantityList.Add(i);
            }

            return quantityList;
        }

        public static List<string> GetPizzaCategoryList()
        {
            return new List<string>()
            {
                "Popular", "Meats", "Veggie"
            };
        }

        public static List<string> GetPizzaSizeList()
        {
            return new List<string>()
            {
                "Small", "Medium", "Large"
            };
        }

        public static List<string> GetToppingCategoryList()
        {
            return new List<string>()
            {
                "Meats", "Veggie"
            };
        }

        public static List<string> GetToppingAmountList()
        {
            return new List<string>()
            {
                "None", "Light", "Regular", "Extra"
            };
        }

        public static List<string> GetToppingHalfList()
        {
            return new List<string>()
            {
                "Whole", "Left", "Right"
            };
        }

        public static List<string> GetSauceAmountList()
        {
            return new List<string>()
            {
                "None", "Light", "Regular", "Extra"
            };
        }

        public static List<string> GetCheeseAmountList()
        {
            return new List<string>()
            {
                "None", "Light", "Regular", "Extra"
            };
        }
    }
}
