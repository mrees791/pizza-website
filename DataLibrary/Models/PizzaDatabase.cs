using Dapper;
using DataLibrary.Models.Joins;
using DataLibrary.Models.QueryFilters;
using DataLibrary.Models.Tables;
using DataLibrary.Models.Utility;
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
                await customerOrder.MapEntityAsync(this);
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

            object parameters = new
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
                }, param: parameters, splitOn: "CartItemId");

            foreach (CartItemJoin cartPizza in cartPizzaList)
            {
                await cartPizza.MapEntityAsync(this);
            }

            return cartPizzaList;
        }

        public async Task<TRecord> GetAsync<TRecord>(object id, IDbTransaction transaction = null) where TRecord : Record
        {
            TRecord record = await connection.GetAsync<TRecord>(id, transaction);
            await record.MapEntityAsync(this);
            return record;
        }

        public async Task<int> RemoveLoginAsync(int userId, string loginProvider, string providerKey, IDbTransaction transaction = null)
        {
            string sql = @"delete from dbo.UserLogin where UserId = @UserId and LoginProvider = @LoginProvider and ProviderKey = @ProviderKey";

            object parameters = new
            {
                UserId = userId,
                LoginProvider = loginProvider,
                ProviderKey = providerKey
            };

            return await connection.ExecuteAsync(sql, parameters, transaction);
        }

        public async Task<int> RemoveClaimAsync(int userId, string claimType, string claimValue, IDbTransaction transaction = null)
        {
            string sql = @"delete from dbo.UserClaim where UserId = @UserId and ClaimType = @ClaimType and ClaimValue = @ClaimValue";

            object parameters = new
            {
                UserId = userId,
                ClaimType = claimType,
                ClaimValue = claimValue
            };

            return await connection.ExecuteAsync(sql, parameters, transaction);
        }

        public async Task<int> RemoveFromRoleAsync(int userId, string roleName, IDbTransaction transaction = null)
        {
            SiteRole siteRole = await GetSiteRoleByNameAsync(roleName);

            string sql = @"delete from dbo.UserRole where UserId = @UserId and RoleId = @RoleId";

            object parameters = new
            {
                UserId = userId,
                RoleId = siteRole.Id
            };

            return await connection.ExecuteAsync(sql, parameters, transaction);
        }

        public async Task<IEnumerable<string>> GetRolesAsync(int userId)
        {
            string joinQuery = @"select u.Id, u.UserId, u.RoleId, r.Id, r.Name
                                from UserRole u
                                inner join SiteRole r on u.RoleId = r.Id
                                where u.UserId = @UserId";

            object parameters = new
            {
                UserId = userId
            };

            IEnumerable<string> userRoleList = await connection.QueryAsync<UserRole, SiteRole, string>(
                joinQuery,
                (userRole, siteRole) =>
                {
                    return siteRole.Name;
                }, param: parameters, splitOn: "RoleId");

            return userRoleList;
        }

        public async Task<SiteRole> GetSiteRoleByNameAsync(string name, IDbTransaction transaction = null)
        {
            string sql = SqlUtility.GetSiteRoleSelectSql() + "where Name = @Name";

            object parameters = new
            {
                Name = name
            };

            return await GetSiteRoleAsync(sql, parameters, transaction);
        }

        public async Task<SiteRole> GetSiteRoleByIdAsync(int id, IDbTransaction transaction = null)
        {
            string sql = SqlUtility.GetSiteRoleSelectSql() + "where Id = @Id";

            object parameters = new
            {
                Id = id
            };

            return await GetSiteRoleAsync(sql, parameters, transaction);
        }

        private async Task<SiteRole> GetSiteRoleAsync(string sql, object parameters, IDbTransaction transaction = null)
        {
            SiteRole role = await connection.QuerySingleAsync<SiteRole>(sql, parameters, transaction);

            if (role != null)
            {
                await role.MapEntityAsync(this, transaction);
            }

            return role;
        }

        public async Task<SiteUser> GetSiteUserByIdAsync(int id, IDbTransaction transaction = null)
        {
            string sql = SqlUtility.GetSiteUserSelectSql() + "where Id = @Id";

            object parameters = new
            {
                Id = id
            };

            return await GetSiteUserAsync(sql, parameters, transaction);
        }

        public async Task<SiteUser> GetSiteUserByEmailAsync(string email, IDbTransaction transaction = null)
        {
            string sql = SqlUtility.GetSiteUserSelectSql() + "where Email = @Email";

            object parameters = new
            {
                Email = email
            };

            return await GetSiteUserAsync(sql, parameters, transaction);
        }

        public async Task<SiteUser> GetSiteUserByNameAsync(string name, IDbTransaction transaction = null)
        {
            string sql = SqlUtility.GetSiteUserSelectSql() + "where UserName = @UserName";

            object parameters = new
            {
                UserName = name
            };

            return await GetSiteUserAsync(sql, parameters, transaction);
        }

        private async Task<SiteUser> GetSiteUserAsync(string sql, object parameters, IDbTransaction transaction = null)
        {
            SiteUser user = await connection.QuerySingleOrDefaultAsync<SiteUser>(sql, parameters, transaction);

            if (user != null)
            {
                await user.MapEntityAsync(this, transaction);
            }

            return user;
        }

        public async Task<UserLogin> GetLoginAsync(string loginProvider, string providerKey, IDbTransaction transaction = null)
        {
            string sql = SqlUtility.GetUserLoginSelectSql() + "where LoginProvider = @LoginProvider and ProviderKey = @ProviderKey";

            object parameters = new
            {
                LoginProvider = loginProvider,
                ProviderKey = providerKey
            };

            UserLogin userLogin = await connection.QuerySingleOrDefaultAsync<UserLogin>(sql, parameters, transaction);
            if (userLogin != null)
            {
                await userLogin.MapEntityAsync(this, transaction);
            }

            return userLogin;
        }

        public async Task<IEnumerable<TRecord>> GetListAsync<TRecord>(IDbTransaction transaction = null) where TRecord : Record
        {
            return await GetListAsync<TRecord>(new { }, transaction);
        }

        internal async Task<IEnumerable<TRecord>> GetListAsync<TRecord>(string conditions, object parameters, IDbTransaction transaction = null) where TRecord : Record
        {
            return await connection.GetListAsync<TRecord>(conditions, parameters, transaction);
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

        public async Task<int> DeleteByIdAsync<TRecord>(object id, IDbTransaction transaction = null) where TRecord : Record
        {
            return await connection.DeleteAsync<TRecord>(id, null);
        }

        public async Task<int> DeleteAsync(Record record, IDbTransaction transaction = null)
        {
            return await connection.DeleteAsync(record, transaction);
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