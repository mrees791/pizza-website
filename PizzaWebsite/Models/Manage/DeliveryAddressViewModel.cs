﻿using DataLibrary.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.Manage
{

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
        public string EditButtonId { get; set; }
        public string DeleteButtonId { get; set; }
        public string AddressRowId { get; set; }

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
            EditButtonId = $"edit-btn-{Id}";
            DeleteButtonId = $"delete-btn-{Id}";
            AddressRowId = $"address-row-{Id}";
        }
    }
}