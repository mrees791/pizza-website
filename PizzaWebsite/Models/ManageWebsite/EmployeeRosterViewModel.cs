using DataLibrary.Models;
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

        public async Task InitializeAsync(int storeId, bool isPostBack, PizzaDatabase pizzaDb)
        {
            this.StoreId = storeId;

            StoreLocation storeLocation = await pizzaDb.GetAsync<StoreLocation>(storeId);
            ViewTitle = $"{storeLocation.Name} Employee Roster";
        }
    }
}