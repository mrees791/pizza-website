using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.ManageDeliveryAddresses
{
    public class ManageAddressesViewModel
    {
        public IEnumerable<DeliveryAddressViewModel> AddressVmList { get; set; }
    }
}