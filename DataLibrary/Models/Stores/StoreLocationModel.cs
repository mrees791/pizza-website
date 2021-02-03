using DataLibrary.Models.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Stores
{
    public class StoreLocationModel
    {
        public int Id { get; set; }
        public string StoreName { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActiveLocation { get; set; }
        public List<EmployeeModel> Employees { get; set; }
    }
}
