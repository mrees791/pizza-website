using DataLibrary.Models.Tables;
using PizzaWebsite.Models.Geography;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models
{
    public class ManageStoresViewModel
    {
        public StoreLocationViewModel StoreLocationVm { get; set; }
        public List<StoreLocationViewModel> StoreLocationVmList { get; set; }
        public string NameFilterField { get; set; }

        public ManageStoresViewModel()
        {
            StoreLocationVmList = new List<StoreLocationViewModel>();
        }
    }

    public class StoreLocationViewModel : IDatabaseRecordConverter<DataLibrary.Models.Tables.StoreLocation>
    {
        public StoreLocationViewModel()
        {
            IsNewRecord = true;
            IsActiveLocation = true;
            StateList = StateListCreator.CreateStateList();
        }

        public StoreLocationViewModel(StoreLocation dbRecord) : this()
        {
            IsNewRecord = false;
            Id = dbRecord.Id;
            Name = dbRecord.Name;
            StreetAddress = dbRecord.StreetAddress;
            City = dbRecord.City;
            SelectedState = dbRecord.State;
            ZipCode = dbRecord.ZipCode;
            PhoneNumber = dbRecord.PhoneNumber;
            IsActiveLocation = dbRecord.IsActiveLocation;
        }

        public bool IsNewRecord { get; set; }
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

        public StoreLocation ToDbModel()
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
    }
}