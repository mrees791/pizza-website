using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Utility
{
    public static class ListUtility
    {
        public static List<string> GetToppingCategoryList()
        {
            return new List<string>()
            {
                "Meats", "Veggie"
            };
        }
    }
}
