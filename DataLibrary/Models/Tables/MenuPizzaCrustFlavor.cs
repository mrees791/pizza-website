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
    [Table("MenuPizzaCrustFlavor")]
    public class MenuPizzaCrustFlavor : ITableBase
    {
        [Key]
        public int Id { get; set; }
        public int SortOrder { get; set; }
        public bool AvailableForPurchase { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasMenuIcon { get; set; }
        public string MenuIconFile { get; set; }
        public bool HasPizzaBuilderImage { get; set; }
        public string PizzaBuilderImageFile { get; set; }

        public void AddInsertItems(List<IInsertable> itemsList)
        {
            itemsList.Add(this);
        }

        public dynamic GetId()
        {
            return Id;
        }

        public void Insert(IDbConnection connection, IDbTransaction transaction = null)
        {
            Id = connection.Insert(this, transaction).Value;
        }

        public void MapEntity(PizzaDatabase pizzaDb)
        {
        }

        public int Update(PizzaDatabase pizzaDb)
        {
            return pizzaDb.Connection.Update(this);
        }
    }
}