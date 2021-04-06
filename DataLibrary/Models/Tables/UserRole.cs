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
    [Table("UserRole")]
    public class UserRole : ITableBase
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }

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