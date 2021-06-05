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
    [Table("SiteUser")]
    public class SiteUser : Record
    {
        [Key]
        public string Id { get; set; }
        public int CurrentCartId { get; set; }
        public int ConfirmOrderCartId { get; set; }
        public int OrderConfirmationId { get; set; }
        public bool IsBanned { get; set; }
        public string ZipCode { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }

        public override dynamic GetId()
        {
            return Id;
        }

        internal override async Task<dynamic> InsertAsync(PizzaDatabase pizzaDb, IDbTransaction transaction = null)
        {
            // Create user's carts
            Cart currentCart = new Cart();
            Cart confirmOrderCart = new Cart();
            CurrentCartId = await currentCart.InsertAsync(pizzaDb, transaction);
            ConfirmOrderCartId = await confirmOrderCart.InsertAsync(pizzaDb, transaction);

            // QueryAsync method was used since connection.InsertAsync was having an issue with its string Id field.
            return await pizzaDb.Connection.QueryAsync(GetInsertQuery(), this, transaction);
        }

        internal override bool InsertRequiresTransaction()
        {
            return true;
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
            return @"INSERT INTO SiteUser (Id, CurrentCartId, ConfirmOrderCartId, OrderConfirmationId, IsBanned, ZipCode, Email, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEndDateUtc, LockoutEnabled, AccessFailedCount)
                     VALUES (@Id, @CurrentCartId, @ConfirmOrderCartId, @OrderConfirmationId, @IsBanned, @ZipCode, @Email, @EmailConfirmed, @PasswordHash, @SecurityStamp, @PhoneNumber, @PhoneNumberConfirmed, @TwoFactorEnabled, @LockoutEndDateUtc, @LockoutEnabled, @AccessFailedCount)";
        }
    }
}