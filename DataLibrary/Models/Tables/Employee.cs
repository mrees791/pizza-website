using Dapper;
using DataLibrary.Models.Sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Tables
{
    [Table("Employee")]
    public class Employee : Record
    {
        [Key]
        public string Id { get; set; }
        public string UserId { get; set; }

        public override dynamic GetId()
        {
            return Id;
        }

        internal override async Task<dynamic> InsertAsync(PizzaDatabase pizzaDb, IDbTransaction transaction = null)
        {
            // QueryAsync method was used since connection.InsertAsync was having an issue with its string Id field.
            return await pizzaDb.Connection.QueryAsync(GetInsertQuery(), this, transaction);
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

        private string GetInsertQuery()
        {
            return @"INSERT INTO Employee (Id, UserId)
                     VALUES (@Id, @UserId)";
        }
    }
}