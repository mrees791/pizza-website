using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using DataLibrary.Models.Services;
using DataLibrary.Models.Sql;
using DataLibrary.Models.Tables;

namespace DataLibrary.Models
{
    public class PizzaDatabase : IDisposable
    {
        private readonly PagedListServices _pagedListServices;
        private readonly SelectQueryServices _selectQueryServices;

        public PizzaDatabase(string connectionName = "PizzaDatabase")
        {
            string connectionString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
            Connection = new SqlConnection(connectionString);
            Connection.Open();
            Commands = new PizzaDatabaseCommands(this);
            _pagedListServices = new PagedListServices();
            _selectQueryServices = new SelectQueryServices();
        }

        public PizzaDatabaseCommands Commands { get; }

        internal IDbConnection Connection { get; }

        public void Dispose()
        {
            Connection.Close();
            Connection.Dispose();
        }

        // CRUD
        public async Task<TRecord> GetAsync<TRecord>(object id, IDbTransaction transaction = null)
            where TRecord : Record
        {
            TRecord record = await Connection.GetAsync<TRecord>(id, transaction);

            if (record != null)
            {
                await record.MapEntityAsync(this);
            }

            return record;
        }

        public async Task<int> DeleteLoginAsync(string userId, string loginProvider, string providerKey,
            IDbTransaction transaction = null)
        {
            string sql = @"DELETE FROM UserLogin
                           WHERE UserId = @UserId
                           AND LoginProvider = @LoginProvider
                           AND ProviderKey = @ProviderKey";

            object parameters = new
            {
                UserId = userId,
                LoginProvider = loginProvider,
                ProviderKey = providerKey
            };

            return await Connection.ExecuteAsync(sql, parameters, transaction);
        }

        public async Task<int> RemoveClaimAsync(string userId, string claimType, string claimValue,
            IDbTransaction transaction = null)
        {
            string sql = @"DELETE FROM UserClaim
                           WHERE UserId = @UserId
                           AND ClaimType = @ClaimType
                           AND ClaimValue = @ClaimValue";

            object parameters = new
            {
                UserId = userId,
                ClaimType = claimType,
                ClaimValue = claimValue
            };

            return await Connection.ExecuteAsync(sql, parameters, transaction);
        }

        public async Task<bool> UserIsInRole(SiteUser siteUser, SiteRole siteRole, IDbTransaction transaction = null)
        {
            return await GetUserRoleAsync(siteUser, siteRole, transaction) != null;
        }

        public async Task<IEnumerable<UserRole>> GetUserRoleListAsync(string userId)
        {
            return await GetListAsync<UserRole>(new {UserId = userId});
        }

        public async Task<SiteRole> GetSiteRoleByNameAsync(string name, IDbTransaction transaction = null)
        {
            return await GetAsync<SiteRole>(name);
        }

        public async Task<SiteUser> GetSiteUserByIdAsync(string id, IDbTransaction transaction = null)
        {
            string whereClause = "WHERE Id = @Id";

            object parameters = new
            {
                Id = id
            };

            return await GetSiteUserAsync(whereClause, parameters, transaction);
        }

        public async Task<SiteUser> GetSiteUserByEmailAsync(string email, IDbTransaction transaction = null)
        {
            string whereClause = "WHERE Email = @Email";

            object parameters = new
            {
                Email = email
            };

            return await GetSiteUserAsync(whereClause, parameters, transaction);
        }

        public async Task<SiteUser> GetSiteUserByNameAsync(string name, IDbTransaction transaction = null)
        {
            string whereClause = "WHERE Id = @UserName";

            object parameters = new
            {
                UserName = name
            };

            return await GetSiteUserAsync(whereClause, parameters, transaction);
        }

        private async Task<SiteUser> GetSiteUserAsync(string whereClause, object parameters,
            IDbTransaction transaction = null)
        {
            string sql = $"{_selectQueryServices.GetSiteUserSelectQuery(true)} {whereClause}";
            SiteUser user = await Connection.QuerySingleOrDefaultAsync<SiteUser>(sql, parameters, transaction);

            if (user != null)
            {
                await user.MapEntityAsync(this, transaction);
            }

            return user;
        }

        public async Task<EmployeeLocation> GetEmployeeLocationAsync(Employee employee, StoreLocation storeLocation,
            IDbTransaction transaction = null)
        {
            string whereClause = @"WHERE EmployeeId = @EmployeeId
                                   AND StoreId = @StoreId";

            object parameters = new
            {
                EmployeeId = employee.Id,
                StoreId = storeLocation.Id
            };

            return await GetEmployeeLocationAsync(whereClause, parameters, transaction);
        }

        private async Task<EmployeeLocation> GetEmployeeLocationAsync(string whereClause, object parameters,
            IDbTransaction transaction = null)
        {
            string sql = $"{_selectQueryServices.GetEmployeeLocationSelectQuery(true)} {whereClause}";
            EmployeeLocation employeeLocation =
                await Connection.QuerySingleOrDefaultAsync<EmployeeLocation>(sql, parameters, transaction);

            if (employeeLocation != null)
            {
                await employeeLocation.MapEntityAsync(this, transaction);
            }

            return employeeLocation;
        }

        public async Task<Employee> GetEmployeeAsync(SiteUser siteUser, IDbTransaction transaction = null)
        {
            string whereClause = @"WHERE UserId = @UserId";

            object parameters = new
            {
                UserId = siteUser.Id
            };

            return await GetEmployeeAsync(whereClause, parameters, transaction);
        }

        private async Task<Employee> GetEmployeeAsync(string whereClause, object parameters,
            IDbTransaction transaction = null)
        {
            string sql = $"{_selectQueryServices.GetEmployeeSelectQuery(true)} {whereClause}";
            Employee employee = await Connection.QuerySingleOrDefaultAsync<Employee>(sql, parameters, transaction);

            if (employee != null)
            {
                await employee.MapEntityAsync(this, transaction);
            }

            return employee;
        }

        public async Task<UserRole> GetUserRoleAsync(SiteUser siteUser, SiteRole siteRole,
            IDbTransaction transaction = null)
        {
            string whereClause = @"WHERE UserId = @UserId
                                   AND RoleName = @RoleName";

            object parameters = new
            {
                UserId = siteUser.Id,
                RoleName = siteRole.Name
            };

            return await GetUserRoleAsync(whereClause, parameters, transaction);
        }

        private async Task<UserRole> GetUserRoleAsync(string whereClause, object parameters,
            IDbTransaction transaction = null)
        {
            string sql = $"{_selectQueryServices.GetUserRoleSelectQuery(true)} {whereClause}";

            UserRole userRole = await Connection.QuerySingleOrDefaultAsync<UserRole>(sql, parameters, transaction);

            if (userRole != null)
            {
                await userRole.MapEntityAsync(this, transaction);
            }

            return userRole;
        }

        public async Task<UserLogin> GetLoginAsync(string loginProvider, string providerKey,
            IDbTransaction transaction = null)
        {
            string whereClause = @"WHERE LoginProvider = @LoginProvider
                                   AND ProviderKey = @ProviderKey";

            object parameters = new
            {
                LoginProvider = loginProvider,
                ProviderKey = providerKey
            };

            return await GetLoginAsync(whereClause, parameters, transaction);
        }

        private async Task<UserLogin> GetLoginAsync(string whereClause, object parameters,
            IDbTransaction transaction = null)
        {
            string sql = $"{_selectQueryServices.GetUserLoginSelectQuery(true)} {whereClause}";

            UserLogin userLogin = await Connection.QuerySingleOrDefaultAsync<UserLogin>(sql, parameters, transaction);

            if (userLogin != null)
            {
                await userLogin.MapEntityAsync(this, transaction);
            }

            return userLogin;
        }

        public async Task<IEnumerable<TRecord>> GetListAsync<TRecord>(IDbTransaction transaction = null)
            where TRecord : Record
        {
            IEnumerable<TRecord> list = await Connection.GetListAsync<TRecord>(new { }, transaction);

            foreach (TRecord record in list)
            {
                await record.MapEntityAsync(this);
            }

            return list;
        }

        public async Task<IEnumerable<TRecord>> GetListAsync<TRecord>(string orderByColumn, SortOrder sortOrder,
            WhereClauseBase whereClauseBase, IDbTransaction transaction = null) where TRecord : Record
        {
            OrderBy orderBy = new OrderBy
            {
                OrderByColumn = orderByColumn,
                SortOrder = sortOrder
            };

            string conditions = $"{whereClauseBase.GetWhereConditions()} ORDER BY {orderBy.GetConditions()}";

            IEnumerable<TRecord> list =
                await Connection.GetListAsync<TRecord>(conditions, whereClauseBase, transaction);

            foreach (TRecord record in list)
            {
                await record.MapEntityAsync(this);
            }

            return list;
        }

        public async Task<IEnumerable<TRecord>> GetListAsync<TRecord>(string orderByColumn, SortOrder sortOrder,
            IDbTransaction transaction = null) where TRecord : Record
        {
            OrderBy orderBy = new OrderBy
            {
                OrderByColumn = orderByColumn,
                SortOrder = sortOrder
            };

            string conditions = $"ORDER BY {orderBy.GetConditions()}";

            IEnumerable<TRecord> list = await GetListAsync<TRecord>(conditions, null, transaction);

            foreach (TRecord record in list)
            {
                await record.MapEntityAsync(this);
            }

            return list;
        }

        internal async Task<IEnumerable<TRecord>> GetListAsync<TRecord>(string conditions, object parameters,
            IDbTransaction transaction = null) where TRecord : Record
        {
            IEnumerable<TRecord> list = await Connection.GetListAsync<TRecord>(conditions, parameters, transaction);

            foreach (TRecord record in list)
            {
                await record.MapEntityAsync(this);
            }

            return list;
        }

        public async Task<IEnumerable<TRecord>> GetListAsync<TRecord>(object whereConditions,
            IDbTransaction transaction = null) where TRecord : Record
        {
            IEnumerable<TRecord> list = await Connection.GetListAsync<TRecord>(whereConditions, transaction);

            foreach (TRecord record in list)
            {
                await record.MapEntityAsync(this);
            }

            return list;
        }

        public async Task<IEnumerable<TRecord>> GetPagedListAsync<TRecord>(int pageNumber, int rowsPerPage,
            string orderByColumn, SortOrder sortOrder, WhereClauseBase whereClauseBase,
            IDbTransaction transaction = null) where TRecord : Record
        {
            OrderBy orderBy = new OrderBy
            {
                OrderByColumn = orderByColumn,
                SortOrder = sortOrder
            };

            IEnumerable<TRecord> list = await Connection.GetListPagedAsync<TRecord>(pageNumber, rowsPerPage,
                whereClauseBase.GetWhereConditions(), orderBy.GetConditions(), whereClauseBase, transaction);

            foreach (TRecord record in list)
            {
                await record.MapEntityAsync(this, transaction);
            }

            return list;
        }

        public async Task<int> GetNumberOfRecordsAsync<TRecord>(WhereClauseBase whereClauseBase,
            IDbTransaction transaction = null) where TRecord : Record
        {
            string conditions = whereClauseBase.GetWhereConditions();
            return await Connection.RecordCountAsync<TRecord>(conditions, whereClauseBase,
                transaction);
        }

        public async Task<int> GetNumberOfPagesAsync<TRecord>(int rowsPerPage, WhereClauseBase whereClauseBase,
            IDbTransaction transaction = null) where TRecord : Record
        {
            int resultCount = await GetNumberOfRecordsAsync<TRecord>(whereClauseBase);
            return _pagedListServices.GetNumberOfPages(rowsPerPage, resultCount);
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
            int rowsAffected = 0;

            if (record.UpdateRequiresTransaction())
            {
                using (IDbTransaction transaction = Connection.BeginTransaction())
                {
                    rowsAffected = await record.UpdateAsync(this,
                        transaction);
                    transaction.Commit();
                }
            }
            else
            {
                rowsAffected = await record.UpdateAsync(this);
            }

            return rowsAffected;
        }

        public async Task<int> DeleteByIdAsync<TRecord>(object id, IDbTransaction transaction = null)
            where TRecord : Record
        {
            return await Connection.DeleteAsync<TRecord>(id);
        }

        public async Task<int> DeleteAsync(Record record, IDbTransaction transaction = null)
        {
            return await Connection.DeleteAsync(record, transaction);
        }

        internal async Task<int> DeleteListAsync<TRecord>(object whereConditions, IDbTransaction transaction = null)
            where TRecord : Record
        {
            return await Connection.DeleteListAsync<TRecord>(whereConditions, transaction);
        }

        public async Task<int> DeleteListAsync<TRecord>(object whereConditions) where TRecord : Record
        {
            return await Connection.DeleteListAsync<TRecord>(whereConditions);
        }
    }
}