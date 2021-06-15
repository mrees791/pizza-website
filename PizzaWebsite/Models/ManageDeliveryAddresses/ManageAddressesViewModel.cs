using System.Collections.Generic;

namespace PizzaWebsite.Models.ManageDeliveryAddresses
{
    public class ManageAddressesViewModel
    {
        public IEnumerable<DeliveryAddressViewModel> AddressVmList { get; set; }
    }
}