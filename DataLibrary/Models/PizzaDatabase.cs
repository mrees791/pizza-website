using Dapper;
using DataLibrary.Models.Filters;
using DataLibrary.Models.Tables;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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

        public async Task<List<T>> GetListAsync<T>()
        {
            IEnumerable<T> list = await connection.GetListAsync<T>();
            return list.ToList();
        }

        public async Task<List<T>> GetListAsync<T>(object whereConditions)
        {
            IEnumerable<T> list = await connection.GetListAsync<T>(whereConditions);
            return list.ToList();
        }

        public async Task<List<T>> GetListAsync<T>(SearchFilter searchFilter, object parameters)
        {
            IEnumerable<T> list = await connection.GetListAsync<T>(searchFilter.GetSqlConditionClause(), parameters);
            return list.ToList();
        }

        public async Task<List<T>> GetListPagedAsync<T>(SearchFilter searchFilter, int pageNumber, int rowsPerPage, string orderby)
        {
            string conditions = searchFilter.GetSqlConditionClause();
            IEnumerable<T> list = await connection.GetListPagedAsync<T>(pageNumber, rowsPerPage, conditions, orderby, searchFilter);
            return list.ToList();
        }

        public async Task<int> GetNumberOfPagesAsync<T>(SearchFilter searchFilter, int rowsPerPage)
        {
            if (rowsPerPage == 0)
            {
                return 0;
            }
            int recordCount = await connection.RecordCountAsync<T>(searchFilter.GetSqlConditionClause(), searchFilter);
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

        /*public async Task<List<T>> GetListPagedAsync<T>(int pageNumber, int rowsPerPage, string orderby, object parameters)
        {
            IEnumerable<T> list = await connection.GetListPagedAsync<T>(pageNumber, rowsPerPage, "", orderby, parameters);
            return list.ToList();
        }*/

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
        public int Insert(Employee employee, IDbTransaction transaction = null)
        {
            return connection.Insert(employee, transaction).Value;
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
