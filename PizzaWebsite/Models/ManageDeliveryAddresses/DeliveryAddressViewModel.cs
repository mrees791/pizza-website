namespace PizzaWebsite.Models.ManageDeliveryAddresses
{
    public class DeliveryAddressViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AddressType { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public string DeleteButtonId { get; set; }
        public string AddressRowId { get; set; }
    }
}