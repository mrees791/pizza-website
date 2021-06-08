using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.Employees
{
    public class ManagePagedListViewModel<TItemViewModel> : PagedListViewModel where TItemViewModel : class, new()
    {
        public TItemViewModel ItemViewModel { get; set; }
        public List<TItemViewModel> ItemViewModelList { get; set; }

        public ManagePagedListViewModel()
        {
            ItemViewModelList = new List<TItemViewModel>();
        }
    }
}