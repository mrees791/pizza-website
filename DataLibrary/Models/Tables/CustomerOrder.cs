using Dapper;
using DataLibrary.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Tables
{
    [Table("CustomerOrder")]
    public class CustomerOrder : IRecord
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
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

        public dynamic GetId()
        {
            return Id;
        }

        public void Insert(PizzaDatabase pizzaDb, IDbTransaction transaction = null)
        {
            Id = pizzaDb.Connection.Insert(this, transaction).Value;
        }

        public bool InsertRequiresTransaction()
        {
            return false;
        }

        public void MapEntity(PizzaDatabase pizzaDb)
        {
        }

        public int Update(PizzaDatabase pizzaDb, IDbTransaction transaction = null)
        {
            return pizzaDb.Connection.Update(this, transaction);
        }

        public bool UpdateRequiresTransaction()
        {
            return false;
        }
    }
}
