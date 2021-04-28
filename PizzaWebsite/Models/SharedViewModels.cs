using DataLibrary.Models;
using DataLibrary.Models.Tables;
using DataLibrary.Models.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PizzaWebsite.Models
{
    public class PaginationViewModel
    {
        public int TotalPages { get; set; }
        public int RowsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalNumberOfItems { get; set; }
        public NameValueCollection QueryString { get; set; }
    }

    public abstract class PagedListViewModel
    {
        public PaginationViewModel PaginationVm { get; set; }

        public PagedListViewModel()
        {
            PaginationVm = new PaginationViewModel();
        }
    }

    public class CartItemViewModel
    {
        public int CartItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public int Quantity { get; set; }
        public string ProductCategory { get; set; }
        public string CartItemQuantitySelectId { get; set; }
        public string CartItemDeleteButtonId { get; set; }
        public string CartItemRowId { get; set; }
        public List<int> QuantityList { get; set; }
    }

    public class CartViewModel
    {
        public List<CartItemViewModel> CartItemList { get; set; }

        public CartViewModel()
        {
            CartItemList = new List<CartItemViewModel>();
        }

        public bool IsEmpty()
        {
            return !CartItemList.Any();
        }
    }

    public class DeliveryAddressViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AddressType { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }

        public DeliveryAddressViewModel()
        {

        }

        public DeliveryAddressViewModel(DeliveryAddress deliveryAddress)
        {
            Id = deliveryAddress.Id;
            Name = deliveryAddress.Name;
            AddressType = deliveryAddress.AddressType;
            StreetAddress = deliveryAddress.StreetAddress;
            City = deliveryAddress.City;
            State = deliveryAddress.State;
            ZipCode = deliveryAddress.ZipCode;
            PhoneNumber = deliveryAddress.PhoneNumber;
        }
    }

    // todo: May remove
    /*public class ManageDeliveryAddressViewModel
    {
        public int Id { get; set; }

        public bool IsNewAddress()
        {
            return Id == 0;
        }
    }*/
}