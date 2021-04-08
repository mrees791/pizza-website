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
    [Table("SiteUser")]
    public class SiteUser : ITable
    {
        [Key]
        public int Id { get; set; }
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
        public string UserName { get; set; }

        public Cart CurrentCart { get; set; }
        public Cart ConfirmOrderCart { get; set; }

        public SiteUser()
        {
            CurrentCart = new Cart();
            ConfirmOrderCart = new Cart();
        }

        public void AddInsertItems(List<IInsertable> insertItems)
        {
            CurrentCart.AddInsertItems(insertItems);
            ConfirmOrderCart.AddInsertItems(insertItems);
            insertItems.Add(this);
        }

        public void Insert(IDbConnection connection, IDbTransaction transaction = null)
        {
            CurrentCartId = CurrentCart.Id;
            ConfirmOrderCartId = ConfirmOrderCart.Id;
            Id = connection.Insert(this, transaction).Value;
        }

        public dynamic GetId()
        {
            return Id;
        }

        public void MapEntity(PizzaDatabase pizzaDb)
        {
            CurrentCart = pizzaDb.Get<Cart>(CurrentCartId);
            ConfirmOrderCart = pizzaDb.Get<Cart>(ConfirmOrderCartId);
        }

        public bool InsertRequiresTransaction()
        {
            return true;
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
