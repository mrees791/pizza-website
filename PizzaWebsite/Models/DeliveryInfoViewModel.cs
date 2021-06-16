using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models
{
    public class DeliveryInfoViewModel
    {
        public string DeliveryDate { get; set; }
        public string AddressType { get; set; }
        public string AddressName { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
    }
}