using DataLibrary.Models;
using DataLibrary.Models.Joins;
using DataLibrary.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PizzaWebsite.Models.ManageWebsite
{
    public class EmployeeRosterViewModel
    {
        public int StoreId { get; set; }
        public string ViewTitle { get; set; }
        public List<EmployeeRosterItemViewModel> EmployeeRosterList { get; set; }

        public async Task InitializeAsync(int storeId, PizzaDatabase pizzaDb)
        {
            this.StoreId = storeId;
            StoreLocation storeLocation = await pizzaDb.GetAsync<StoreLocation>(storeId);

            ViewTitle = $"{storeLocation.Name} Employee Roster";
            EmployeeRosterList = new List<EmployeeRosterItemViewModel>();

            IEnumerable<EmployeeLocationJoin> employeeLocationJoinList = await pizzaDb.GetJoinedEmployeeLocationListByStoreId(storeId);

            foreach (EmployeeLocationJoin locationJoin in employeeLocationJoinList)
            {
                EmployeeRosterItemViewModel itemVm = new EmployeeRosterItemViewModel()
                {
                    // todo: Finish here
                };

                EmployeeRosterList.Add(itemVm);
            }
        }
    }
}