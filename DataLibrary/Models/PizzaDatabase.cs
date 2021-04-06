using Dapper;
using DataLibrary.Models.Interfaces;
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

        internal IDbConnection Connection
        {
            get
            {
                return connection;
            }
        }

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
        public async Task<TEntity> GetAsync<TEntity>(object id, IDbTransaction transaction = null) where TEntity : class, IRecord, new()
        {
            TEntity entity = await connection.GetAsync<TEntity>(id, transaction);
            entity.MapEntity(this);
            return entity;
        }

        public TEntity Get<TEntity>(object id, IDbTransaction transaction = null) where TEntity : class, IRecord, new()
        {
            TEntity entity = connection.Get<TEntity>(id, transaction);
            entity.MapEntity(this);
            return entity;
        }

        public List<TEntity> GetList<TEntity>() where TEntity : IRecord
        {
            List<TEntity> list = connection.GetList<TEntity>().ToList();
            foreach (TEntity entity in list)
            {
                entity.MapEntity(this);
            }
            return list;
        }

        public List<TEntity> GetList<TEntity>(object parameters, string orderByColumn) where TEntity : IRecord
        {
            string conditions = GetSqlWhereConditions(parameters, orderByColumn);
            List<TEntity> list = connection.GetList<TEntity>(conditions, parameters).ToList();
            foreach (TEntity entity in list)
            {
                entity.MapEntity(this);
            }
            return list;
        }

        public async Task<List<TEntity>> GetListAsync<TEntity>() where TEntity : IRecord
        {
            IEnumerable<TEntity> list = await connection.GetListAsync<TEntity>();
            foreach (TEntity entity in list)
            {
                entity.MapEntity(this);
            }
            return list.ToList();
        }

        public async Task<List<TEntity>> GetListAsync<TEntity>(object whereConditions) where TEntity : IRecord
        {
            IEnumerable<TEntity> list = await connection.GetListAsync<TEntity>(whereConditions);
            foreach (TEntity entity in list)
            {
                entity.MapEntity(this);
            }
            return list.ToList();
        }

        public async Task<List<TEntity>> GetListAsync<TEntity>(object searchFilter, object parameters) where TEntity : IRecord
        {
            IEnumerable<TEntity> list = await connection.GetListAsync<TEntity>(GetSqlWhereFilterConditions(searchFilter), parameters);
            foreach (TEntity entity in list)
            {
                entity.MapEntity(this);
            }
            return list.ToList();
        }

        public async Task<List<TEntity>> GetListPagedAsync<TEntity>(object searchFilter, int pageNumber, int rowsPerPage, string orderby) where TEntity : IRecord
        {
            string conditions = GetSqlWhereFilterConditions(searchFilter);
            IEnumerable<TEntity> list = await connection.GetListPagedAsync<TEntity>(pageNumber, rowsPerPage, conditions, orderby, searchFilter);
            foreach (TEntity entity in list)
            {
                entity.MapEntity(this);
            }
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

        internal string GetSqlWhereConditions(object parameters, string orderByColumn)
        {
            string sqlWhereConditions = string.Empty;
            bool queriesAdded = false;
            PropertyInfo[] properties = parameters.GetType().GetProperties();

            foreach (PropertyInfo propertyInfo in properties)
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

            // Order by column is never set by user input to avoid SQL injections.
            sqlWhereConditions += $"order by {orderByColumn}";

            return sqlWhereConditions;
        }

        /// <summary>
        /// Creates a where clause which can be used to run queries with filters using the like operator.
        /// </summary>
        /// <param name="searchFilter"></param>
        /// <returns>An SQL where clause.</returns>
        internal string GetSqlWhereFilterConditions(object searchFilter)
        {
            string sqlWhereConditions = string.Empty;
            bool queriesAdded = false;
            PropertyInfo[] properties = searchFilter.GetType().GetProperties();

            foreach (PropertyInfo propertyInfo in properties)
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
        // todo: Remove TOP and set proper name.
        public dynamic Insert(IInsertable entity)
        {
            List<IInsertable> itemsList = new List<IInsertable>();
            entity.AddInsertItems(itemsList);

            if (itemsList.Count == 1)
            {
                entity.Insert(connection);
            }
            else if (itemsList.Count > 1)
            {
                // Create a transaction since there is more than one record to insert.
                using (var transaction = connection.BeginTransaction())
                {
                    InsertMultipleItems(itemsList, transaction);
                    transaction.Commit();
                }
            }

            return entity.GetId();
        }

        private void InsertMultipleItems(List<IInsertable> itemsList, IDbTransaction transaction)
        {
            foreach (IInsertable item in itemsList)
            {
                item.Insert(connection, transaction);
            }
        }

        public int Update(IUpdatable entity)
        {
            return entity.Update(this);
        }

        public int Delete<TEntity>(TEntity entity, IDbTransaction transaction = null) where TEntity : class, new()
        {
            return connection.Delete<TEntity>(entity, transaction);
        }
    }
}