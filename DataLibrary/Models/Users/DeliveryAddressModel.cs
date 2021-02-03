using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Users
{
    public class DeliveryAddressModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string AddressType { get; set; }
        public string AddressName { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
    }
}
