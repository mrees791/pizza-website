using System.Collections.Generic;
using System.Linq;

namespace PizzaWebsite.Models.ManageStores
{
    public class EmployeeRosterViewModel
    {
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public IEnumerable<EmployeeRosterItemViewModel> EmployeeRosterVmList { get; set; }

        public bool RosterIsEmpty()
        {
            return !EmployeeRosterVmList.Any();
        }
    }
}