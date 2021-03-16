using Dapper;
using DataLibrary.Models.Tables;
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

        public PizzaDatabase(string connectionName = "PizzaDatabase")
        {
            string connectionString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
            connection = new SqlConnection(connectionString);
            connection.Open();
        }

        public void Dispose()
        {
            connection.Close();
            connection.Dispose();
        }

        // CRUD Table Operations
        // todo: Remove this non async get list method
        /*public List<TEntity> GetList<TEntity>() where TEntity : class
        {
            IEnumerable<TEntity> list = connection.GetList<TEntity>();
            return list.ToList();
        }*/

        public async Task<List<TEntity>> GetListAsync<TEntity>() where TEntity : class
        {
            IEnumerable<TEntity> list = await connection.GetListAsync<TEntity>();
            return list.ToList();
        }

        public async Task<List<TEntity>> GetListAsync<TEntity>(object whereConditions) where TEntity : class
        {
            IEnumerable<TEntity> list = await connection.GetListAsync<TEntity>(whereConditions);
            return list.ToList();
        }

        public async Task<List<TEntity>> GetListAsync<TEntity>(object searchFilter, object parameters) where TEntity : class
        {
            IEnumerable<TEntity> list = await connection.GetListAsync<TEntity>(GetSqlWhereFilterClause(searchFilter), parameters);
            return list.ToList();
        }

        public async Task<List<TEntity>> GetListPagedAsync<TEntity>(object searchFilter, int pageNumber, int rowsPerPage, string orderby) where TEntity : class
        {
            string conditions = GetSqlWhereFilterClause(searchFilter);
            IEnumerable<TEntity> list = await connection.GetListPagedAsync<TEntity>(pageNumber, rowsPerPage, conditions, orderby, searchFilter);
            return list.ToList();
        }

        public async Task<int> GetNumberOfRecords<TEntity>(object searchFilter)
        {
            int recordCount = await connection.RecordCountAsync<TEntity>(GetSqlWhereFilterClause(searchFilter), searchFilter);
            return recordCount;
        }

        public async Task<int> GetNumberOfPagesAsync<TEntity>(object searchFilter, int rowsPerPage)
        {
            if (rowsPerPage == 0)
            {
                return 0;
            }
            int recordCount = await GetNumberOfRecords<TEntity>(searchFilter);
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

        /// <summary>
        /// Creates a where clause which can be used to run queries with filters using the like operator.
        /// This is used by Simple Dapper's get list methods.
        /// </summary>
        /// <param name="searchFilter"></param>
        /// <returns>An SQL where clause.</returns>
        internal string GetSqlWhereFilterClause(object searchFilter)
        {
            string sqlWhereClause = string.Empty;
            bool queriesAdded = false;

            foreach (PropertyInfo propertyInfo in searchFilter.GetType().GetProperties())
            {
                Type propertyType = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
                object propertyValue = propertyInfo.GetValue(searchFilter);

                if (propertyValue != null)
                {
                    string columnName = propertyInfo.Name;

                    if (!queriesAdded)
                    {
                        sqlWhereClause += "where ";
                    }
                    else
                    {
                        sqlWhereClause += "and ";
                    }

                    // Only uses the column name with a placeholder to avoid SQL injections.
                    // The column name variable is never set by user input.
                    sqlWhereClause += $"{columnName} like '%' + @{columnName} + '%'";
                    queriesAdded = true;
                }
            }

            return sqlWhereClause;
        }

        // Cart CRUD
        private int InsertCart(IDbTransaction transaction = null)
        {
            // We had to use Query<int> instead of Insert because the Insert method will not work with DEFAULT VALUES.
            return connection.Query<int>("INSERT INTO Cart OUTPUT Inserted.Id DEFAULT VALUES;", null, transaction).Single();
        }

        public void Update(Cart cart, IDbTransaction transaction = null)
        {
            connection.Update(cart, transaction);
        }

        // Employee CRUD
        public void Insert(Employee employee, IDbTransaction transaction = null)
        {
            // Query method was used since connection.Insert was having an issue with its string ID field.
            connection.Query("INSERT INTO Employee (Id, UserId, CurrentlyEmployed) VALUES (@Id, @UserId, @CurrentlyEmployed)", employee, transaction);
        }

        public void Update(Employee employee, IDbTransaction transaction = null)
        {
            connection.Update(employee, transaction);
        }

        // EmployeeLocation CRUD
        public int Insert(EmployeeLocation employeeLocation, IDbTransaction transaction = null)
        {
            return connection.Insert(employeeLocation, transaction).Value;
        }

        public void Update(EmployeeLocation employeeLocation, IDbTransaction transaction = null)
        {
            connection.Update(employeeLocation, transaction);
        }

        // SiteRole CRUD
        public int Insert(SiteRole siteRole, IDbTransaction transaction = null)
        {
            return connection.Insert(siteRole, transaction).Value;
        }

        public void Update(SiteRole siteRole, IDbTransaction transaction = null)
        {
            connection.Update(siteRole, transaction);
        }
        
        // SiteUser CRUD
        public int Insert(SiteUser siteUser)
        {
            using (var transaction = connection.BeginTransaction())
            {
                siteUser.CurrentCartId = InsertCart(transaction);
                siteUser.ConfirmOrderCartId = InsertCart(transaction);
                int? userId = connection.Insert(siteUser, transaction);

                transaction.Commit();

                return userId.Value;
            }
        }

        public void Update(SiteUser siteUser, IDbTransaction transaction = null)
        {
            connection.Update(siteUser, transaction);
        }

        // StoreLocation CRUD
        public int Insert(StoreLocation storeLocation, IDbTransaction transaction = null)
        {
            return connection.Insert(storeLocation, transaction).Value;
        }

        public void Update(StoreLocation storeLocation, IDbTransaction transaction = null)
        {
            connection.Update(storeLocation, transaction);
        }

        // UserClaim CRUD
        public int Insert(UserClaim userClaim, IDbTransaction transaction = null)
        {
            return connection.Insert(userClaim, transaction).Value;
        }

        public void Update(UserClaim userClaim, IDbTransaction transaction = null)
        {
            connection.Update(userClaim, transaction);
        }

        public void Delete(UserClaim userClaim, IDbTransaction transaction = null)
        {
            connection.Delete(userClaim, transaction);
        }

        // UserLogin CRUD
        public int Insert(UserLogin userLogin, IDbTransaction transaction = null)
        {
            return connection.Insert(userLogin, transaction).Value;
        }

        public void Update(UserLogin userLogin, IDbTransaction transaction = null)
        {
            connection.Update(userLogin, transaction);
        }

        public void Delete(UserLogin userLogin, IDbTransaction transaction = null)
        {
            connection.Delete(userLogin, transaction);
        }

        // UserRole CRUD
        public int Insert(UserRole userRole, IDbTransaction transaction = null)
        {
            return connection.Insert(userRole, transaction).Value;
        }

        public void Update(UserRole userRole, IDbTransaction transaction = null)
        {
            connection.Update(userRole, transaction);
        }

        public void Delete(UserRole userRole, IDbTransaction transaction = null)
        {
            connection.Delete(userRole, transaction);
        }
    }
}
