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
        public string StoreName { get; set; }
        public List<EmployeeRosterItemViewModel> EmployeeRosterList { get; set; }

        public async Task InitializeAsync(int storeId, PizzaDatabase pizzaDb)
        {
            EmployeeRosterList = new List<EmployeeRosterItemViewModel>();
            StoreLocation storeLocation = await pizzaDb.GetAsync<StoreLocation>(storeId);
            SiteRole managerRole = await pizzaDb.GetSiteRoleByNameAsync("Manager");
            IEnumerable<EmployeeLocationJoin> employeeLocationJoinList = await pizzaDb.GetJoinedEmployeeLocationListByStoreId(storeId);

            StoreId = storeId;
            StoreName = storeLocation.Name;

            foreach (EmployeeLocationJoin locationJoin in employeeLocationJoinList)
            {
                SiteUser siteUser = await pizzaDb.GetSiteUserByIdAsync(locationJoin.Employee.UserId);
                bool isManager = await pizzaDb.UserIsInRole(siteUser, managerRole);

                EmployeeRosterItemViewModel itemVm = new EmployeeRosterItemViewModel()
                {
                    EmployeeId = locationJoin.Employee.Id,
                    EmployeeLocationId = locationJoin.EmployeeLocation.Id,
                    UserId = locationJoin.Employee.UserId,
                    IsManager = isManager
                };

                EmployeeRosterList.Add(itemVm);
            }
        }

        public bool RosterIsEmpty()
        {
            return !EmployeeRosterList.Any();
        }
    }
}