using DataLibrary.Models;
using DataLibrary.Models.JoinLists;
using DataLibrary.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PizzaWebsite.Models.ManageStores
{
    public class EmployeeRosterViewModel
    {
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public List<EmployeeRosterItemViewModel> EmployeeRosterVmList { get; set; }

        public bool RosterIsEmpty()
        {
            return !EmployeeRosterVmList.Any();
        }
    }
}