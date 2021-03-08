using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Filters.TableFilters
{
    public class StoreLocationFilter : SearchFilter
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }

        protected override List<FilterPair> CreateFilterPairs()
        {
            List<FilterPair> filterPairs = new List<FilterPair>();

            if (Name != null)
            {
                AddFilterPair(filterPairs, "Name", Name);
            }
            if (PhoneNumber != null)
            {
                AddFilterPair(filterPairs, "PhoneNumber", PhoneNumber);
            }

            return filterPairs;
        }
    }
}
