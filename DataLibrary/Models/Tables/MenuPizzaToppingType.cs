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
    [Table("MenuPizzaToppingType")]
    public class MenuPizzaToppingType : IRecord
    {
        [Key]
        public int Id { get; set; }
        public int SortOrder { get; set; }
        public bool AvailableForPurchase { get; set; }
        public string Name { get; set; }
        public decimal PriceLight { get; set; }
        public decimal PriceRegular { get; set; }
        public decimal PriceExtra { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public bool HasMenuIcon { get; set; }
        public string MenuIconFile { get; set; }
        public bool HasPizzaBuilderImage { get; set; }
        public string PizzaBuilderImageFile { get; set; }

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