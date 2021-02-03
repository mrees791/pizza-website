using DataLibrary.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Employees
{
    public class EmployeeModel : UserModel
    {
        public int EmployeeId { get; set; }
        public bool IsCurrentlyEmployed { get; set; }
        public List<EmployeeLocationModel> EmployeeLocations { get; set; }
    }
}
