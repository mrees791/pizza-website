using DataLibrary.Models;
using DataLibrary.Models.JoinLists;
using DataLibrary.Models.Joins;
using DataLibrary.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PizzaWebsite.Models.ManageWebsite
{
    public class ViewEmployeeLocationsViewModel
    {
        public string EmployeeId { get; set; }
        public List<EmployeeLocationViewModel> EmployeeLocationVmList { get; set; }
        public EmployeeLocationViewModel ItemViewModel { get; set; }

        public async Task InitializeAsync(string employeeId, PizzaDatabase pizzaDb)
        {
            EmployeeId = employeeId;
            EmployeeLocationVmList = new List<EmployeeLocationViewModel>();
            var joinList = new EmployeeLocationOnStoreLocationJoin();
            await joinList.LoadListByEmployeeIdAsync(employeeId, pizzaDb);

            foreach (Join2<EmployeeLocation, StoreLocation> join in joinList.Items)
            {
                EmployeeLocationViewModel viewModel = new EmployeeLocationViewModel()
                {
                    Name = join.Table2.Name,
                    PhoneNumber = join.Table2.PhoneNumber,
                    City = join.Table2.City,
                    State = join.Table2.State,
                    ZipCode = join.Table2.ZipCode,
                    IsActiveLocation = join.Table2.IsActiveLocation
                };

                EmployeeLocationVmList.Add(viewModel);
            }
        }
    }
}