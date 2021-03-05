using Dapper;
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

        // Cart CRUD
        private int InsertCart(IDbTransaction transaction = null)
        {
            // We had to use Query<int> instead of Insert because the Insert method will not work with DEFAULT VALUES.
            return connection.Query<int>("INSERT INTO Cart OUTPUT Inserted.Id DEFAULT VALUES;", null, transaction).Single();
        }

        public async Task<List<Cart>> GetCartListAsync()
        {
            return new List<Cart>(await connection.GetListAsync<Cart>());
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

        public async Task<List<Employee>> GetEmployeeListAsync(object whereConditions = null)
        {
            return new List<Employee>(await connection.GetListAsync<Employee>(whereConditions));
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

        public async Task<List<EmployeeLocation>> GetEmployeeLocationListAsync(object whereConditions = null)
        {
            return new List<EmployeeLocation>(await connection.GetListAsync<EmployeeLocation>(whereConditions));
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

        public async Task<List<SiteRole>> GetSiteRoleListAsync(object whereConditions = null)
        {
            return new List<SiteRole>(await connection.GetListAsync<SiteRole>(whereConditions));
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

        public async Task<List<SiteUser>> GetSiteUserListAsync(object whereConditions = null)
        {
            return new List<SiteUser>(await connection.GetListAsync<SiteUser>(whereConditions));
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

        public async Task<List<StoreLocation>> GetStoreLocationListAsync(object whereConditions = null)
        {
            return new List<StoreLocation>(await connection.GetListAsync<StoreLocation>(whereConditions));
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

        public async Task<List<UserClaim>> GetUserClaimListAsync(object whereConditions = null)
        {
            return new List<UserClaim>(await connection.GetListAsync<UserClaim>(whereConditions));
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

        public async Task<List<UserLogin>> GetUserLoginListAsync(object whereConditions = null)
        {
            return new List<UserLogin>(await connection.GetListAsync<UserLogin>(whereConditions));
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

        public async Task<List<UserRole>> GetUserRoleListAsync(object whereConditions = null)
        {
            return new List<UserRole>(await connection.GetListAsync<UserRole>(whereConditions));
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
