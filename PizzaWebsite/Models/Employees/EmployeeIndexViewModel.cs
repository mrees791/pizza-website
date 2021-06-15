namespace PizzaWebsite.Models.Employees
{
    public class EmployeeIndexViewModel
    {
        public string EmployeeId { get; set; }
        public bool AuthorizedToManageMenu { get; set; }
        public bool AuthorizedToManageStores { get; set; }
        public bool AuthorizedToManageUsers { get; set; }
        public bool AuthorizedToManageEmployees { get; set; }
    }
}