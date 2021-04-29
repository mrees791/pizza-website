using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.Manage
{
    public class ManageAddressesViewModel
    {
        public List<DeliveryAddressViewModel> AddressList { get; set; }

        public ManageAddressesViewModel()
        {
            AddressList = new List<DeliveryAddressViewModel>();
        }
    }
}