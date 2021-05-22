using DataLibrary.Models.Tables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Joins
{
    public class CustomerOrderJoin : JoinRecord
    {
        public CustomerOrder CustomerOrder { get; set; }
        public DeliveryInfo DeliveryInfo { get; set; }

        public override dynamic GetId()
        {
            return CustomerOrder.Id;
        }

        internal override async Task<dynamic> InsertAsync(PizzaDatabase pizzaDb, IDbTransaction transaction = null)
        {
            if (DeliveryInfo != null)
            {
                await DeliveryInfo.InsertAsync(pizzaDb, transaction);
                CustomerOrder.DeliveryInfoId = DeliveryInfo.Id;
            }

            await CustomerOrder.InsertAsync(pizzaDb, transaction);

            return CustomerOrder.Id;
        }

        internal override bool InsertRequiresTransaction()
        {
            return true;
        }

        internal override async Task MapEntityAsync(PizzaDatabase pizzaDb, IDbTransaction transaction = null)
        {
            if (DeliveryInfo != null)
            {
                await DeliveryInfo.MapEntityAsync(pizzaDb, transaction);
            }

            await CustomerOrder.MapEntityAsync(pizzaDb, transaction);
        }

        internal override async Task<int> UpdateAsync(PizzaDatabase pizzaDb, IDbTransaction transaction = null)
        {
            int rowsUpdated = 0;

            if (DeliveryInfo != null)
            {
                rowsUpdated += await DeliveryInfo.UpdateAsync(pizzaDb, transaction);
            }
            rowsUpdated += await CustomerOrder.UpdateAsync(pizzaDb, transaction);

            return rowsUpdated;
        }

        internal override bool UpdateRequiresTransaction()
        {
            return true;
        }
    }
}