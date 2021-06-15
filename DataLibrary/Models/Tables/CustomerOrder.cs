using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace DataLibrary.Models.Tables
{
    [Table("CustomerOrder")]
    public class CustomerOrder : Record
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }
        public int StoreId { get; set; }
        public int CartId { get; set; }
        public bool IsCancelled { get; set; }
        public decimal OrderSubtotal { get; set; }
        public decimal OrderTax { get; set; }
        public decimal OrderTotal { get; set; }
        public OrderPhase OrderPhase { get; set; }
        public bool OrderCompleted { get; set; }
        public DateTime DateOfOrder { get; set; }
        public bool IsDelivery { get; set; }
        public int? DeliveryInfoId { get; set; }

        public override dynamic GetId()
        {
            return Id;
        }

        public string GetOrderType()
        {
            return IsDelivery ? "Delivery" : "Pickup";
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