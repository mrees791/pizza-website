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
    public class ViewEmployeeLocationsViewModel
    {
        public string EmployeeId { get; set; }
        public List<EmployeeLocationViewModel> EmployeeLocationVmList { get; set; }
        public EmployeeLocationViewModel ItemViewModel { get; set; }

        public async Task InitializeAsync(string employeeId, PizzaDatabase pizzaDb)
        {
            EmployeeId = employeeId;
            EmployeeLocationVmList = new List<EmployeeLocationViewModel>();
            IEnumerable<EmployeeLocationOnStoreLocationJoin> joinList = await EmployeeLocationOnStoreLocationJoin.GetListByEmployeeId(employeeId, pizzaDb);

            foreach (EmployeeLocationOnStoreLocationJoin join in joinList)
            {
                EmployeeLocationViewModel viewModel = new EmployeeLocationViewModel()
                {
                    Name = join.StoreLocation.Name,
                    PhoneNumber = join.StoreLocation.PhoneNumber,
                    City = join.StoreLocation.City,
                    State = join.StoreLocation.State,
                    ZipCode = join.StoreLocation.ZipCode,
                    IsActiveLocation = join.StoreLocation.IsActiveLocation
                };

                EmployeeLocationVmList.Add(viewModel);
            }
        }
    }
}