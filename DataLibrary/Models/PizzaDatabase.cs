﻿using Dapper;
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
        public async Task<TEntity> GetAsync<TEntity>(object id, IDbTransaction transaction = null) where TEntity : class
        {
            return await connection.GetAsync<TEntity>(id, transaction);
        }

        public List<TEntity> GetList<TEntity>() where TEntity : class
        {
            return connection.GetList<TEntity>().ToList();
        }

        public List<TEntity> GetList<TEntity>(object parameters, string orderby) where TEntity : class
        {
            string conditions = GetSqlWhereConditions(parameters, orderby);
            return connection.GetList<TEntity>(conditions, parameters).ToList();
        }

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
            IEnumerable<TEntity> list = await connection.GetListAsync<TEntity>(GetSqlWhereFilterConditions(searchFilter), parameters);
            return list.ToList();
        }

        public async Task<List<TEntity>> GetListPagedAsync<TEntity>(object searchFilter, int pageNumber, int rowsPerPage, string orderby) where TEntity : class
        {
            string conditions = GetSqlWhereFilterConditions(searchFilter);
            IEnumerable<TEntity> list = await connection.GetListPagedAsync<TEntity>(pageNumber, rowsPerPage, conditions, orderby, searchFilter);
            return list.ToList();
        }

        public async Task<int> GetNumberOfRecords<TEntity>(object searchFilter)
        {
            int recordCount = await connection.RecordCountAsync<TEntity>(GetSqlWhereFilterConditions(searchFilter), searchFilter);
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

        internal string GetSqlWhereConditions(object parameters, string orderby)
        {
            string sqlWhereConditions = string.Empty;
            bool queriesAdded = false;

            foreach (PropertyInfo propertyInfo in parameters.GetType().GetProperties())
            {
                Type propertyType = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
                object propertyValue = propertyInfo.GetValue(parameters);

                if (propertyValue != null)
                {
                    string columnName = propertyInfo.Name;

                    if (!queriesAdded)
                    {
                        sqlWhereConditions += "where ";
                    }
                    else
                    {
                        sqlWhereConditions += "and ";
                    }

                    // Only uses the column name with a placeholder to avoid SQL injections.
                    // The column name variable is never set by user input.
                    sqlWhereConditions += $"{columnName} = @{columnName} ";
                    queriesAdded = true;
                }
            }

            // The orderby variable is never set by user input.
            sqlWhereConditions += $"order by {orderby}";

            return sqlWhereConditions;
        }

        /// <summary>
        /// Creates a where clause which can be used to run queries with filters using the like operator.
        /// This is used by Simple Dapper's get list methods.
        /// </summary>
        /// <param name="searchFilter"></param>
        /// <returns>An SQL where clause.</returns>
        internal string GetSqlWhereFilterConditions(object searchFilter)
        {
            string sqlWhereConditions = string.Empty;
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
                        sqlWhereConditions += "where ";
                    }
                    else
                    {
                        sqlWhereConditions += " and ";
                    }

                    // Only uses the column name with a placeholder to avoid SQL injections.
                    // The column name variable is never set by user input.
                    sqlWhereConditions += $"{columnName} like '%' + @{columnName} + '%'";
                    queriesAdded = true;
                }
            }

            return sqlWhereConditions;
        }

        // CRUD
        public int Insert<TEntity>(TEntity entity, IDbTransaction transaction = null) where TEntity : class, new()
        {
            if (entity is Employee)
            {
                InsertEmployee(entity as Employee, transaction);
                return 0; // Returns 0 since the Employee ID primary key is a string manually entered by a manager or admin.
            }
            else if (entity is Cart)
            {
                return InsertCart(transaction);
            }
            else if (entity is SiteUser)
            {
                return InsertSiteUser(entity as SiteUser);
            }
            else if (entity is MenuPizza)
            {
                return InsertMenuPizza(entity as MenuPizza);
            }
            return connection.Insert<TEntity>(entity, transaction).Value;
        }

        public int Update<TEntity>(TEntity entity, IDbTransaction transaction = null) where TEntity : class, new()
        {
            return connection.Update<TEntity>(entity, transaction);
        }
        public int Delete<TEntity>(TEntity entity, IDbTransaction transaction = null) where TEntity : class, new()
        {
            return connection.Delete<TEntity>(entity, transaction);
        }

        // Cart CRUD
        private int InsertCart(IDbTransaction transaction = null)
        {
            // We had to use Query<int> instead of Insert because the Insert method will not work with DEFAULT VALUES.
            return connection.Query<int>("INSERT INTO Cart OUTPUT Inserted.Id DEFAULT VALUES;", null, transaction).Single();
        }

        // Employee CRUD
        private void InsertEmployee(Employee entity, IDbTransaction transaction = null)
        {
            // Query method was used since connection.Insert was having an issue with its string ID field.
            connection.Query("INSERT INTO Employee (Id, UserId, CurrentlyEmployed) VALUES (@Id, @UserId, @CurrentlyEmployed)", entity, transaction);
        }

        // MenuPizza CRUD
        private int InsertMenuPizza(MenuPizza entity)
        {
            using (var transaction = connection.BeginTransaction())
            {
                int id = connection.Insert<MenuPizza>(entity, transaction).Value;

                // Insert topping records
                foreach (MenuPizzaTopping topping in entity.Toppings)
                {
                    connection.Insert<MenuPizzaTopping>(topping, transaction);
                }

                transaction.Commit();

                return id;
            }
        }

        // SiteUser CRUD
        private int InsertSiteUser(SiteUser entity)
        {
            using (var transaction = connection.BeginTransaction())
            {
                entity.CurrentCartId = InsertCart(transaction);
                entity.ConfirmOrderCartId = InsertCart(transaction);
                int? userId = connection.Insert(entity, transaction);

                transaction.Commit();

                return userId.Value;
            }
        }
    }
}