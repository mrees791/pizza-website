using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace DataLibrary.Models.Tables
{
    [Table("Cart")]
    public class Cart : Record
    {
        [Key]
        public int Id { get; set; }

        public override dynamic GetId()
        {
            return Id;
        }

        internal override async Task<dynamic> InsertAsync(PizzaDatabase pizzaDb, IDbTransaction transaction = null)
        {
            // We had to use QueryAsync<int> instead of InsertAsync because the InsertAsync method will not work with SQL DEFAULT VALUES.
            IEnumerable<int> result =
                await pizzaDb.Connection.QueryAsync<int>("INSERT INTO Cart OUTPUT Inserted.Id DEFAULT VALUES;", null,
                    transaction);
            Id = result.First();

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