using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.CustomerOrders
{
    public class DeliveryInfoModel
    {
        public int Id { get; set; }
        public DateTime DateOfDelivery { get; set; }
        public string DeliveryAddressType { get; set; }
        public string DeliveryAddressName { get; set; }
        public string DeliveryStreetAddress { get; set; }
        public string DeliveryCity { get; set; }
        public string DeliveryState { get; set; }
        public string DeliveryZipCode { get; set; }
        public string DeliveryPhoneNumber { get; set; }
    }
}
