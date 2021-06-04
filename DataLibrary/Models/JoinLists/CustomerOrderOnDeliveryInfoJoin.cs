using DataLibrary.Models.Sql;
using DataLibrary.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.JoinLists
{
    public class CustomerOrderOnDeliveryInfoJoin : JoinListBase<CustomerOrder, DeliveryInfo>
    {
        public async Task LoadFirstOrDefaultByCustomerOrderIdAsync(int customerOrderId, PizzaDatabase pizzaDb)
        {
            string whereClause = "WHERE c.Id = @Id";

            object parameters = new
            {
                Id = customerOrderId
            };

            await LoadListAsync(whereClause, parameters, true, "c.Id", SortOrder.Ascending, pizzaDb);
        }

        public async Task LoadListByCustomerOderIdAsync(int customerOrderId, PizzaDatabase pizzaDb)
        {
            string whereClause = "WHERE c.Id = @Id";

            object parameters = new
            {
                Id = customerOrderId
            };

            await LoadListAsync(whereClause, parameters, false, "c.Id", SortOrder.Ascending, pizzaDb);
        }

        public async Task LoadListByUserIdAsync(string userId, PizzaDatabase pizzaDb)
        {
            string whereClause = "WHERE c.UserId = @UserId";

            object parameters = new
            {
                UserId = userId
            };

            await LoadListAsync(whereClause, parameters, false, "c.Id", SortOrder.Ascending, pizzaDb);
        }

        protected override string GetSqlJoinQuery(bool onlySelectFirst)
        {
            return $@"SELECT {SqlUtility.CreateTopClause(onlySelectFirst)}
                      c.Id, c.UserId, c.StoreId, c.CartId, c.IsCancelled, c.OrderSubtotal, c.OrderTax, c.OrderTotal, c.OrderPhase,
                      c.OrderCompleted, c.DateOfOrder, c.IsDelivery, c.DeliveryInfoId,
                      d.Id, d.DateOfDelivery, d.DeliveryAddressType, d.DeliveryAddressName,
                      d.DeliveryStreetAddress, d.DeliveryCity, d.DeliveryState, d.DeliveryZipCode, d.DeliveryPhoneNumber
                      FROM CustomerOrder c
                      LEFT JOIN DeliveryInfo d
                      ON c.DeliveryInfoId = d.Id";
        }
    }
}
