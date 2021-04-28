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
        public DeliveryInfo DeliveryInfo { get; set; }

        public dynamic GetId()
        {
            return CustomerOrder.Id;
        }

        public void Insert(PizzaDatabase pizzaDb, IDbTransaction transaction = null)
        {
            if (DeliveryInfo != null)
            {
                DeliveryInfo.Insert(pizzaDb, transaction);
                CustomerOrder.DeliveryInfoId = DeliveryInfo.Id;
            }
            CustomerOrder.Insert(pizzaDb, transaction);
        }

        public bool InsertRequiresTransaction()
        {
            return false;
        }

        public void MapEntity(PizzaDatabase pizzaDb)
        {
            if (DeliveryInfo != null)
            {
                DeliveryInfo.MapEntity(pizzaDb);
            }
            CustomerOrder.MapEntity(pizzaDb);
        }

        public int Update(PizzaDatabase pizzaDb, IDbTransaction transaction = null)
        {
            int rowsUpdated = 0;

            if (DeliveryInfo != null)
            {
                rowsUpdated += DeliveryInfo.Update(pizzaDb, transaction);
            }
            rowsUpdated += CustomerOrder.Update(pizzaDb, transaction);

            return rowsUpdated;
        }

        public bool UpdateRequiresTransaction()
        {
            return false;
        }
    }
}