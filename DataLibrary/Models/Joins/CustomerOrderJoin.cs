using DataLibrary.Models.Interfaces;
using DataLibrary.Models.Tables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Joins
{
    public class CustomerOrderJoin : IRecord
    {
        public CustomerOrder CustomerOrder { get; set; }
        public DeliveryAddress DeliveryAddress { get; set; }

        public dynamic GetId()
        {
            return CustomerOrder.Id;
        }

        public void Insert(PizzaDatabase pizzaDb, IDbTransaction transaction = null)
        {
            CustomerOrder.Insert(pizzaDb, transaction);
            if (DeliveryAddress != null)
            {
                DeliveryAddress.Insert(pizzaDb, transaction);
            }
        }

        public bool InsertRequiresTransaction()
        {
            return false;
        }

        public void MapEntity(PizzaDatabase pizzaDb)
        {
            CustomerOrder.MapEntity(pizzaDb);
            if (DeliveryAddress != null)
            {
                DeliveryAddress.MapEntity(pizzaDb);
            }
        }

        public int Update(PizzaDatabase pizzaDb, IDbTransaction transaction = null)
        {
            int rowsUpdated = 0;

            rowsUpdated += CustomerOrder.Update(pizzaDb, transaction);
            if (DeliveryAddress != null)
            {
                rowsUpdated += DeliveryAddress.Update(pizzaDb, transaction);
            }

            return rowsUpdated;
        }

        public bool UpdateRequiresTransaction()
        {
            return false;
        }
    }
}