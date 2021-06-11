using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.Manage
{
    public class ManageAddressesViewModel
    {
        public IEnumerable<DeliveryAddressViewModel> AddressVmList { get; set; }
    }
}