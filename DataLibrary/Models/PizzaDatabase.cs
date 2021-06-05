using Dapper;
using DataLibrary.Models.QueryFilters;
using DataLibrary.Models.QuerySearches;
using DataLibrary.Models.Sql;
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
        public async Task<TRecord> GetAsync<TRecord>(object id, IDbTransaction transaction = null) where TRecord : Record
        {
            TRecord record = await connection.GetAsync<TRecord>(id, transaction);

            if (record != null)
            {
                await record.MapEntityAsync(this);
            }

            return record;
        }

        public async Task<int> RemoveLoginAsync(string userId, string loginProvider, string providerKey, IDbTransaction transaction = null)
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

        public async Task<int> RemoveClaimAsync(string userId, string claimType, string claimValue, IDbTransaction transaction = null)
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

        public async Task<bool> UserIsInRole(SiteUser siteUser, SiteRole siteRole, IDbTransaction transaction = null)
        {
            UserRole userRole = await GetUserRoleAsync(siteUser, siteRole, transaction);

            return userRole != null;
        }

        public async Task<IEnumerable<UserRole>> GetUserRoleListAsync(string userId)
        {
            IEnumerable<UserRole> userRoles = await GetListAsync<UserRole>(new { UserId = userId });

            return userRoles;
        }

        public async Task<SiteRole> GetSiteRoleByNameAsync(string name, IDbTransaction transaction = null)
        {
            return await GetAsync<SiteRole>(name);
        }

        private async Task<SiteRole> GetSiteRoleAsync(string sql, object parameters, IDbTransaction transaction = null)
        {
            SiteRole role = await connection.QuerySingleOrDefaultAsync<SiteRole>(sql, parameters, transaction);

            if (role != null)
            {
                await role.MapEntityAsync(this, transaction);
            }

            return role;
        }

        public async Task<SiteUser> GetSiteUserByIdAsync(string id, IDbTransaction transaction = null)
        {
            string sql = $"{SelectQueries.GetSiteUserSelectQuery(true)} where Id = @Id";

            object parameters = new
            {
                Id = id
            };

            return await GetSiteUserAsync(sql, parameters, transaction);
        }

        public async Task<SiteUser> GetSiteUserByEmailAsync(string email, IDbTransaction transaction = null)
        {
            string sql = $"{SelectQueries.GetSiteUserSelectQuery(true)} where Email = @Email";

            object parameters = new
            {
                Email = email
            };

            return await GetSiteUserAsync(sql, parameters, transaction);
        }

        public async Task<SiteUser> GetSiteUserByNameAsync(string name, IDbTransaction transaction = null)
        {
            string sql = $"{SelectQueries.GetSiteUserSelectQuery(true)} where Id = @UserName";

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

        public async Task<EmployeeLocation> GetEmployeeLocationAsync(Employee employee, StoreLocation storeLocation, IDbTransaction transaction = null)
        {
            string sql = $"{SelectQueries.GetEmployeeLocationSelectQuery(true)} where EmployeeId = @EmployeeId and StoreId = @StoreId";

            object parameters = new
            {
                EmployeeId = employee.Id,
                StoreId = storeLocation.Id
            };

            return await GetEmployeeLocationAsync(sql, parameters, transaction);
        }

        private async Task<EmployeeLocation> GetEmployeeLocationAsync(string sql, object parameters, IDbTransaction transaction = null)
        {
            EmployeeLocation employeeLocation = await connection.QuerySingleOrDefaultAsync<EmployeeLocation>(sql, parameters, transaction);

            if (employeeLocation != null)
            {
                await employeeLocation.MapEntityAsync(this, transaction);
            }

            return employeeLocation;
        }

        public async Task<UserRole> GetUserRoleAsync(SiteUser siteUser, SiteRole siteRole, IDbTransaction transaction = null)
        {
            string sql = $"{SelectQueries.GetUserRoleSelectQuery(true)} where UserId = @UserId and RoleName = @RoleName";

            object parameters = new
            {
                UserId = siteUser.Id,
                RoleName = siteRole.Name
            };

            return await GetUserRoleAsync(sql, parameters, transaction);
        }

        private async Task<UserRole> GetUserRoleAsync(string sql, object parameters, IDbTransaction transaction = null)
        {
            UserRole userRole = await connection.QuerySingleOrDefaultAsync<UserRole>(sql, parameters, transaction);

            if (userRole != null)
            {
                await userRole.MapEntityAsync(this, transaction);
            }

            return userRole;
        }

        public async Task<UserLogin> GetLoginAsync(string loginProvider, string providerKey, IDbTransaction transaction = null)
        {
            string sql = $"{SelectQueries.GetUserLoginSelectQuery(true)} where LoginProvider = @LoginProvider and ProviderKey = @ProviderKey";

            object parameters = new
            {
                LoginProvider = loginProvider,
                ProviderKey = providerKey
            };

            UserLogin userLogin = await connection.QuerySingleOrDefaultAsync<UserLogin>(sql, parameters, transaction);

            return userLogin;
        }

        public async Task<IEnumerable<TRecord>> GetListAsync<TRecord>(IDbTransaction transaction = null) where TRecord : Record
        {
            IEnumerable<TRecord> list = await connection.GetListAsync<TRecord>(new { }, transaction);

            foreach (TRecord record in list)
            {
                await record.MapEntityAsync(this);
            }

            return list;
        }

        public async Task<IEnumerable<TRecord>> GetListAsync<TRecord>(string orderByColumn, SortOrder sortOrder, QuerySearchBase querySearch, IDbTransaction transaction = null) where TRecord : Record
        {
            OrderBy orderBy = new OrderBy()
            {
                OrderByColumn = orderByColumn,
                SortOrder = sortOrder
            };

            string conditions = $"{querySearch.GetWhereConditions()} order by {orderBy.GetConditions()}";

            IEnumerable<TRecord> list = await connection.GetListAsync<TRecord>(conditions, querySearch, transaction);

            foreach (TRecord record in list)
            {
                await record.MapEntityAsync(this);
            }

            return list;
        }

        public async Task<IEnumerable<TRecord>> GetListAsync<TRecord>(string orderByColumn, SortOrder sortOrder, IDbTransaction transaction = null) where TRecord : Record
        {
            OrderBy orderBy = new OrderBy()
            {
                OrderByColumn = orderByColumn,
                SortOrder = sortOrder
            };

            string conditions = $"order by {orderBy.GetConditions()}";

            IEnumerable<TRecord> list = await GetListAsync<TRecord>(conditions, null, transaction);

            foreach (TRecord record in list)
            {
                await record.MapEntityAsync(this);
            }

            return list;
        }

        internal async Task<IEnumerable<TRecord>> GetListAsync<TRecord>(string conditions, object parameters, IDbTransaction transaction = null) where TRecord : Record
        {
            IEnumerable<TRecord> list = await connection.GetListAsync<TRecord>(conditions, parameters, transaction);

            foreach (TRecord record in list)
            {
                await record.MapEntityAsync(this);
            }

            return list;
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

        public async Task<IEnumerable<TRecord>> GetPagedListAsync<TRecord>(int pageNumber, int rowsPerPage, string orderByColumn, SortOrder sortOrder, QueryFilterBase filter,
            IDbTransaction transaction = null) where TRecord : Record
        {
            OrderBy orderBy = new OrderBy()
            {
                OrderByColumn = orderByColumn,
                SortOrder = sortOrder
            };

            IEnumerable<TRecord> list = await connection.GetListPagedAsync<TRecord>(pageNumber, rowsPerPage, filter.GetWhereConditions(), orderBy.GetConditions(), filter, transaction);

            foreach (TRecord record in list)
            {
                await record.MapEntityAsync(this, transaction);
            }

            return list;
        }

        public async Task<IEnumerable<TRecord>> GetPagedListAsync<TRecord>(int pageNumber, int rowsPerPage, string orderByColumn, SortOrder sortOrder,
            QuerySearchBase querySearch, IDbTransaction transaction = null) where TRecord : Record
        {
            OrderBy orderBy = new OrderBy()
            {
                OrderByColumn = orderByColumn,
                SortOrder = sortOrder
            };

            IEnumerable<TRecord> list = await connection.GetListPagedAsync<TRecord>(pageNumber, rowsPerPage, querySearch.GetWhereConditions(), orderBy.GetConditions(),
                querySearch, transaction);

            foreach (TRecord record in list)
            {
                await record.MapEntityAsync(this, transaction);
            }

            return list;
        }

        public async Task<int> GetNumberOfRecordsAsync<TRecord>(QueryBase query, IDbTransaction transaction = null) where TRecord : Record
        {
            string conditions = query.GetWhereConditions();
            return await connection.RecordCountAsync<TRecord>(conditions, parameters: query, transaction: transaction);
        }

        public async Task<int> GetNumberOfPagesAsync<TRecord>(int rowsPerPage, QueryBase query, IDbTransaction transaction = null) where TRecord : Record
        {
            if (rowsPerPage == 0)
            {
                return 0;
            }

            int recordCount = await GetNumberOfRecordsAsync<TRecord>(query);

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