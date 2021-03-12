using DataLibrary.Models;
using DataLibrary.Models.Filters;
using DataLibrary.Models.Tables;
using PizzaWebsite.Models.Geography;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PizzaWebsite.Models
{
    public class ErrorViewModel
    {
        public string ErrorMessage { get; set; }
        public string ReturnUrlAction { get; set; }
    }

    public class ConfirmationViewModel
    {
        public string ConfirmationMessage { get; set; }
        public string ReturnUrlAction { get; set; }
    }

    // T is ViewModel class, U is DataLibrary equivalent, V is the filter class
    // todo: Update documentation here.
    public class ManageListViewModel<T, U, V> where U : class, new() where T : ManageViewModelBase<U>, new() where V : SearchFilter, new()
    {
        public T ItemViewModel { get; set; }
        public List<T> ItemViewModelList { get; set; }
        public PaginationViewModel PaginationVm { get; set; }
        public V SearchFilter { get; set; }

        public ManageListViewModel()
        {
            ItemViewModelList = new List<T>();
            PaginationVm = new PaginationViewModel();
            SearchFilter = new V();
        }

        public async Task LoadViewModelRecordsAsync(PizzaDatabase database, HttpRequestBase request, int? page, int? rowsPerPage, string sortColumnName)
        {
            // Set default values
            if (!page.HasValue)
            {
                page = 1;
            }
            if (!rowsPerPage.HasValue)
            {
                rowsPerPage = 10;
            }

            int totalNumberOfItems = await database.GetNumberOfRecords<U>(SearchFilter);
            int totalPages = await database.GetNumberOfPagesAsync<U>(SearchFilter, rowsPerPage.Value);
            List<U> databaseRecords = await database.GetListPagedAsync<U>(SearchFilter, page.Value, rowsPerPage.Value, sortColumnName);

            // Navigation pane
            PaginationVm.QueryString = request.QueryString;
            PaginationVm.CurrentPage = page.Value;
            PaginationVm.RowsPerPage = rowsPerPage.Value;
            PaginationVm.TotalPages = totalPages;
            PaginationVm.TotalNumberOfItems = totalNumberOfItems;

            foreach (U recordModel in databaseRecords)
            {
                T locationVm = new T();
                locationVm.FromDbModel(recordModel);
                ItemViewModelList.Add(locationVm);
            }
        }
    }

    public abstract class ManageViewModelBase<T> : IDatabaseRecordConverter<T> where T : class, new()
    {
        public ManageViewModelBase()
        {

        }

        public abstract void FromDbModel(T dbModel);
        public abstract T ToDbModel();
    }

    public class StoreLocationViewModel : ManageViewModelBase<DataLibrary.Models.Tables.StoreLocation>
    {
        public StoreLocationViewModel()
        {
            StateList = StateListCreator.CreateStateList();
        }

        public List<State> StateList { get; set; }
        public int Id { get; set; }

        [Required]
        [Display(Name = "Store Name")]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Street Address")]
        [MaxLength(50)]
        public string StreetAddress { get; set; }

        [Required]
        [Display(Name = "City")]
        [MaxLength(50)]
        public string City { get; set; }

        [Required]
        [Display(Name = "State")]
        public string SelectedState { get; set; }

        [Required]
        [Display(Name = "Zip Code")]
        [StringLength(5,MinimumLength = 5)]
        public string ZipCode { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [StringLength(10, MinimumLength = 10)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActiveLocation { get; set; }

        public bool IsNewRecord()
        {
            return Id == 0;
        }

        public override StoreLocation ToDbModel()
        {
            return new StoreLocation()
            {
                Id = Id,
                Name = Name,
                StreetAddress = StreetAddress,
                City = City,
                State = SelectedState,
                ZipCode = ZipCode,
                PhoneNumber = PhoneNumber,
                IsActiveLocation = IsActiveLocation
            };
        }

        public override void FromDbModel(StoreLocation dbModel)
        {
            Id = dbModel.Id;
            Name = dbModel.Name;
            StreetAddress = dbModel.StreetAddress;
            City = dbModel.City;
            SelectedState = dbModel.State;
            ZipCode = dbModel.ZipCode;
            PhoneNumber = dbModel.PhoneNumber;
            IsActiveLocation = dbModel.IsActiveLocation;
        }
    }
}