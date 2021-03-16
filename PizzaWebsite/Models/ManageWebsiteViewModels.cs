using DataLibrary.Models;
using DataLibrary.Models.Tables;
using Microsoft.AspNet.Identity.Owin;
using PizzaWebsite.Models.Geography;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

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

    public class ManageStoreViewModel
    {
        public ManageStoreViewModel()
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
        [StringLength(5, MinimumLength = 5)]
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
    }

    public class ManageUserViewModel
    {
        public int Id { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Banned")]
        public bool IsBanned { get; set; }

        public ManageUserViewModel()
        {

        }
    }

    public class ManagePagedListViewModel<TItemViewModel> : PagedListViewModel where TItemViewModel : class, new()
    {
        public TItemViewModel ItemViewModel { get; set; }
        public List<TItemViewModel> ItemViewModelList { get; set; }

        public ManagePagedListViewModel()
        {
            ItemViewModelList = new List<TItemViewModel>();
        }
    }

    // todo: Update documentation here.
    /*public class ManageListViewModel<TViewModel, TEntity> where TEntity : class, new() where TViewModel : CreateEditEntityViewModel<TEntity>, new()
    {
        public TViewModel ItemViewModel { get; set; }
        public List<TViewModel> ItemViewModelList { get; set; }
        public PaginationViewModel PaginationVm { get; set; }

        public ManageListViewModel()
        {
            ItemViewModelList = new List<TViewModel>();
            PaginationVm = new PaginationViewModel();
        }

        public async Task LoadViewModelRecordsAsync(PizzaDatabase database, HttpRequestBase request, int? page, int? rowsPerPage, string sortColumnName, object searchFilters)
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

            int totalNumberOfItems = await database.GetNumberOfRecords<TEntity>(searchFilters);
            int totalPages = await database.GetNumberOfPagesAsync<TEntity>(searchFilters, rowsPerPage.Value);
            List<TEntity> databaseRecords = await database.GetListPagedAsync<TEntity>(searchFilters, page.Value, rowsPerPage.Value, sortColumnName);

            // Navigation pane
            PaginationVm.QueryString = request.QueryString;
            PaginationVm.CurrentPage = page.Value;
            PaginationVm.RowsPerPage = rowsPerPage.Value;
            PaginationVm.TotalPages = totalPages;
            PaginationVm.TotalNumberOfItems = totalNumberOfItems;

            foreach (TEntity recordModel in databaseRecords)
            {
                TViewModel viewModel = new TViewModel();
                viewModel.FromEntity(recordModel);
                ItemViewModelList.Add(viewModel);
            }
        }
    }

    public abstract class CreateEditEntityViewModel<TEntity> : IEntityConverter<TEntity> where TEntity : class, new()
    {
        public CreateEditEntityViewModel()
        {

        }

        public abstract void FromEntity(TEntity entity);
        public abstract TEntity ToEntity();
    }

    public class ManageEmployeeViewModel : CreateEditEntityViewModel<Employee>
    {
        [Display(Name = "Employee ID")]
        public string Id { get; set; }

        [Display(Name = "Currently Employed")]
        public bool CurrentlyEmployed { get; set; }

        [Display(Name = "Is Manager")]
        public bool IsManager { get; set; }

        public ManageEmployeeViewModel()
        {

        }

        public override void FromEntity(Employee entity)
        {
            Id = entity.Id;
            CurrentlyEmployed = entity.CurrentlyEmployed;
        }

        public override Employee ToEntity()
        {
            return new Employee()
            {
                Id = Id,
                CurrentlyEmployed = CurrentlyEmployed
            };
        }
    }*/
}