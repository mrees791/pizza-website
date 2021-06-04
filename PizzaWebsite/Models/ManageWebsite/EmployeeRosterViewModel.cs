using DataLibrary.Models;
using DataLibrary.Models.JoinLists;
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
            var joinList = new EmployeeOnEmployeeLocationJoin();
            await joinList.LoadListByStoreIdAsync(storeId, pizzaDb);

            StoreId = storeId;
            StoreName = storeLocation.Name;

            foreach (Join<Employee, EmployeeLocation> join in joinList.Items)
            {
                SiteUser siteUser = await pizzaDb.GetSiteUserByIdAsync(join.Table1.UserId);
                bool isManager = await pizzaDb.UserIsInRole(siteUser, managerRole);

                EmployeeRosterItemViewModel itemVm = new EmployeeRosterItemViewModel()
                {
                    EmployeeId = join.Table1.Id,
                    EmployeeLocationId = join.Table2.Id,
                    UserId = join.Table1.UserId,
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