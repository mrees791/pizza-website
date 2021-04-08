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
    [Table("Employee")]
    public class Employee : ITable
    {
        [Key]
        public string Id { get; set; }
        public int UserId { get; set; }
        public bool CurrentlyEmployed { get; set; }

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
            // Query method was used since connection.Insert was having an issue with its string ID field.
            connection.Query("INSERT INTO Employee (Id, UserId, CurrentlyEmployed) VALUES (@Id, @UserId, @CurrentlyEmployed)", this, transaction);
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
