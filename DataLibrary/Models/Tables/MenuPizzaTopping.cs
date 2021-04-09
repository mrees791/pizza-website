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
    [Table("MenuPizzaTopping")]
    public class MenuPizzaTopping : IRecord
    {
        [Key]
        public int Id { get; set; }
        public int MenuPizzaId { get; set; }
        public string ToppingHalf { get; set; }
        public string ToppingAmount { get; set; }
        public int MenuPizzaToppingTypeId { get; set; }

        public void AddInsertItems(List<IRecord> itemsList)
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

        public CartPizzaTopping CreateCartTopping()
        {
            return new CartPizzaTopping()
            {
                MenuPizzaToppingTypeId = MenuPizzaToppingTypeId,
                ToppingAmount = ToppingAmount,
                ToppingHalf = ToppingHalf
            };
        }
    }
}