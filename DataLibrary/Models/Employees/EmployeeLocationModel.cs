using DataLibrary.Models.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Employees
{
    public class EmployeeLocationModel
    {
        public int Id { get; set; }
        public EmployeeModel Employee { get; set; }
        public StoreLocationModel Store { get; set; }
    }
}
