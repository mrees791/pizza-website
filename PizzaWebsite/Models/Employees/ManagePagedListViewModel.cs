using System.Collections.Generic;

namespace PizzaWebsite.Models.Employees
{
    public class ManagePagedListViewModel<TItemViewModel>
        where TItemViewModel : class
    {
        public TItemViewModel ItemViewModel { get; set; }
        public List<TItemViewModel> ItemViewModelList { get; set; }
        public PaginationViewModel PaginationVm { get; set; }
    }
}