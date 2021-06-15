namespace PizzaWebsite.Models.ManageStores
{
    public class RemoveEmployeeFromRosterViewModel
    {
        public int EmployeeLocationId { get; set; }
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public string EmployeeId { get; set; }
    }
}