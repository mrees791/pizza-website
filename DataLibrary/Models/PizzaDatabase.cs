using Dapper;
using DataLibrary.Models.BaseClasses;
using DataLibrary.Models.Interfaces;
using DataLibrary.Models.OldJoins;
using DataLibrary.Models.OldTables;
using DataLibrary.Models.QueryFilters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    public class PizzaDatabase : IDisposable
    {
        private IDbConnection connection;
        private PizzaDatabaseCommands commands;

        public PizzaDatabase(string connectionName = "PizzaDatabase")
        {
            string connectionString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
            connection = new SqlConnection(connectionString);
            connection.Open();

            commands = new PizzaDatabaseCommands(this);
        }

        public void Dispose()
        {
            connection.Close();
            connection.Dispose();
        }

        // CRUD
        public async Task<List<CustomerOrderJoin>> GetJoinedCustomerOrderListAsync(int userId)
        {
            string whereClause = "where c.UserId = @UserId";

            object parameters = new
            {
                UserId = userId
            };

            return await GetJoinedCustomerOrderListAsync(whereClause, parameters);
        }

        private async Task<List<CustomerOrderJoin>> GetJoinedCustomerOrderListAsync(string whereClause, object parameters)
        {
            string joinQuery = @"select c.Id, c.UserId, c.StoreId, c.CartId, c.IsCancelled, 
                                 c.OrderSubtotal, c.OrderTax, c.OrderTotal, c.OrderPhase,
                                 c.OrderCompleted, c.DateOfOrder, c.IsDelivery, c.DeliveryInfoId,
                                 d.Id, d.DateOfDelivery, d.DeliveryAddressType, d.DeliveryAddressName,
                                 d.DeliveryStreetAddress, d.DeliveryCity, d.DeliveryState, d.DeliveryZipCode,
                                 d.DeliveryPhoneNumber
                                 from CustomerOrder c
                                 left join DeliveryInfo d on c.DeliveryInfoId = d.Id " +
                                 whereClause;

            IEnumerable<CustomerOrderJoin> customerOrderList = await connection.QueryAsync<CustomerOrder, DeliveryInfo, CustomerOrderJoin>(
                joinQuery,
                (customerOrder, deliveryInfo) =>
                {
                    CustomerOrderJoin customerOrderJoin = new CustomerOrderJoin();
                    customerOrderJoin.CustomerOrder = customerOrder;
                    customerOrderJoin.DeliveryInfo = deliveryInfo;
                    return customerOrderJoin;
                }, param: parameters, splitOn: "Id");

            foreach (CustomerOrderJoin customerOrder in customerOrderList)
            {
                customerOrder.MapEntity(this);
            }

            return customerOrderList.ToList();
        }

        public async Task<List<CartItemJoin>> GetJoinedCartItemListAsync(int cartId)
        {
            List<CartItemJoin> items = new List<CartItemJoin>();

            // One join is required for each product category.
            items.AddRange(await GetJoinedPizzaCartItemsAsync(cartId));
            items.Sort();

            return items;
        }

        private async Task<IEnumerable<CartItemJoin>> GetJoinedPizzaCartItemsAsync(int cartId)
        {
            string joinQuery = @"select c.Id, c.CartId, c.PricePerItem, c.Quantity, c.ProductCategory, c.Quantity,
                                    p.CartItemId, p.CheeseAmount, p.MenuPizzaCheeseId, p.MenuPizzaCrustFlavorId, p.MenuPizzaCrustId, p.MenuPizzaSauceId, p.SauceAmount, p.size
	                                from CartItem c
	                                inner join CartPizza p on c.Id = p.CartItemId
                                    where c.CartId = @CartId";

            object queryParameters = new
            {
                CartId = cartId
            };

            IEnumerable<CartItemJoin> cartPizzaList = await connection.QueryAsync<CartItem, CartPizza, CartItemJoin>(
                joinQuery,
                (cartItem, cartPizza) =>
                {
                    CartItemJoin cartPizzaJoin = new CartItemJoin();
                    cartPizzaJoin.CartItem = cartItem;
                    cartPizzaJoin.CartItemType = cartPizza;
                    return cartPizzaJoin;
                }, param: queryParameters, splitOn: "CartItemId");

            foreach (CartItemJoin cartPizza in cartPizzaList)
            {
                cartPizza.MapEntity(this);
            }

            return cartPizzaList;
        }

        public async Task<TRecord> GetAsync<TRecord>(object id, IDbTransaction transaction = null) where TRecord : Record
        {
            TRecord record = await connection.GetAsync<TRecord>(id, transaction);
            await record.MapEntityAsync(this);
            return record;
        }

        public async Task<IEnumerable<TRecord>> GetListAsync<TRecord>(IDbTransaction transaction = null) where TRecord : Record
        {
            return await GetListAsync<TRecord>(new { }, transaction);
        }

        public async Task<IEnumerable<TRecord>> GetListAsync<TRecord>(object whereConditions, IDbTransaction transaction = null) where TRecord : Record
        {
            IEnumerable<TRecord> list = await connection.GetListAsync<TRecord>(whereConditions, transaction);

            foreach (TRecord record in list)
            {
                await record.MapEntityAsync(this);
            }

            return list;
        }

        public async Task<IEnumerable<TRecord>> GetPagedListAsync<TRecord>(int pageNumber, int rowsPerPage, string orderByColumn, QueryFilterBase filter,
            IDbTransaction transaction = null) where TRecord : Record
        {
            string conditions = filter.GetWhereConditions(orderByColumn);
            IEnumerable<TRecord> list = await connection.GetListPagedAsync<TRecord>(pageNumber, rowsPerPage, conditions, orderByColumn, filter, transaction);

            foreach (TRecord record in list)
            {
                await record.MapEntityAsync(this, transaction);
            }

            return list;
        }

        public async Task<int> GetNumberOfRecordsAsync<TRecord>(QueryFilterBase filter, IDbTransaction transaction = null)
        {
            string conditions = filter.GetWhereConditions();
            return await connection.RecordCountAsync<TRecord>(conditions, parameters: filter, transaction: transaction);
        }

        public async Task<int> GetNumberOfPagesAsync<TRecord>(QueryFilterBase filter, int rowsPerPage, IDbTransaction transaction = null)
        {
            if (rowsPerPage == 0)
            {
                return 0;
            }
            int recordCount = await GetNumberOfRecordsAsync<TRecord>(filter);
            if (recordCount == 0)
            {
                return 0;
            }
            int pages = recordCount / rowsPerPage;
            int remainder = recordCount % rowsPerPage;
            if (remainder != 0)
            {
                pages += 1;
            }
            return pages;
        }

        public async Task<dynamic> InsertAsync(Record record)
        {
            if (record.InsertRequiresTransaction())
            {
                using (IDbTransaction transaction = Connection.BeginTransaction())
                {
                    await record.InsertAsync(this, transaction);
                    transaction.Commit();
                }
            }
            else
            {
                await record.InsertAsync(this);
            }

            return record.GetId();
        }

        public async Task<int> UpdateAsync(Record record)
        {
            int rowsUpdated = 0;

            if (record.UpdateRequiresTransaction())
            {
                using (IDbTransaction transaction = Connection.BeginTransaction())
                {
                    rowsUpdated = await record.UpdateAsync(this, transaction);
                    transaction.Commit();
                }
            }
            else
            {
                rowsUpdated = await record.UpdateAsync(this);
            }

            return rowsUpdated;
        }

        internal async Task<int> DeleteAsync<TRecord>(object id, IDbTransaction transaction = null) where TRecord : Record
        {
            return await connection.DeleteAsync<TRecord>(id, transaction);
        }

        public async Task<int> DeleteAsync<TRecord>(object id) where TRecord : Record
        {
            return await connection.DeleteAsync<TRecord>(id, null);
        }

        internal async Task<int> DeleteListAsync<TRecord>(object whereConditions, IDbTransaction transaction = null) where TRecord : Record
        {
            return await connection.DeleteListAsync<TRecord>(whereConditions, transaction);
        }

        public async Task<int> DeleteListAsync<TRecord>(object whereConditions) where TRecord : Record
        {
            return await connection.DeleteListAsync<TRecord>(whereConditions, null);
        }

        public PizzaDatabaseCommands Commands { get => commands; }
        internal IDbConnection Connection { get => connection; }
    }
}