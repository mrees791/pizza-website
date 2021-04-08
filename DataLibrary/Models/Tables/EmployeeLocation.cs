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
    [Table("EmployeeLocation")]
    public class EmployeeLocation : ITable
    {
        [Key]
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int StoreId { get; set; }

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