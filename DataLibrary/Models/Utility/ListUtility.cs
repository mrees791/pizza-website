using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Utility
{
    public static class ListUtility
    {
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
