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

        public ListServices()
        {
            _deliveryAddressTypeList = new List<string>()
            {
                "House", "Business", "Apartment", "Other"
            };
        }

        public IEnumerable<string> DeliveryAddressTypeList { get => _deliveryAddressTypeList; }
    }
}
