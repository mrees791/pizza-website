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
    }

    public class ManageEmployeeViewModel
    {
        [Display(Name = "Employee ID")]
        public string Id { get; set; }

        [Display(Name = "Currently Employed")]
        public bool CurrentlyEmployed { get; set; }

        [Display(Name = "Is Manager")]
        public bool IsManager { get; set; }
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
}