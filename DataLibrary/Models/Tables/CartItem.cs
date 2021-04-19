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
    [Table("CartItem")]
    public class CartItem : IRecord
    {
        [Key]
        public int Id { get; set; }
        public int CartId { get; set; }
        public decimal PricePerItem { get; set; }
        public int Quantity { get; set; }
        public string ProductCategory { get; set; }

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
            return pizzaDb.Connection.Update(this);
        }

        public bool UpdateRequiresTransaction()
        {
            return false;
        }
    }
}