﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Tables
{
    [Table("DeliveryInfo")]
    public class DeliveryInfo : Record
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateOfDelivery { get; set; }
        public string DeliveryAddressType { get; set; }
        public string DeliveryAddressName { get; set; }
        public string DeliveryStreetAddress { get; set; }
        public string DeliveryCity { get; set; }
        public string DeliveryState { get; set; }
        public string DeliveryZipCode { get; set; }
        public string DeliveryPhoneNumber { get; set; }

        public override dynamic GetId()
        {
            return Id;
        }

        internal override async Task<dynamic> InsertAsync(PizzaDatabase pizzaDb, IDbTransaction transaction = null)
        {
            int? id = await pizzaDb.Connection.InsertAsync(this, transaction);
            Id = id.Value;
            return Id;
        }

        internal override bool InsertRequiresTransaction()
        {
            return false;
        }

        internal override async Task MapEntityAsync(PizzaDatabase pizzaDb, IDbTransaction transaction = null)
        {
            await Task.FromResult(0);
        }

        internal override async Task<int> UpdateAsync(PizzaDatabase pizzaDb, IDbTransaction transaction = null)
        {
            return await pizzaDb.Connection.UpdateAsync(this, transaction);
        }

        internal override bool UpdateRequiresTransaction()
        {
            return false;
        }
    }
}