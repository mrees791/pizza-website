using Dapper;
using DataLibrary.Models.Interfaces;
using DataLibrary.Models.Joins;
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

        public async Task<List<CartItemJoin>> GetJoinedCartItemsAsync(int cartId)
        {
            List<CartItemJoin> items = new List<CartItemJoin>();

            // One join is required for each product category.
            List<Task<IEnumerable<CartItemJoin>>> joinQueryTasks = new List<Task<IEnumerable<CartItemJoin>>>()
            {
                GetJoinedPizzaCartItemsAsync(cartId)
            };

            while (joinQueryTasks.Any())
            {
                Task<IEnumerable<CartItemJoin>> finishedTask = await Task.WhenAny(joinQueryTasks);
                items.AddRange(finishedTask.Result);
                joinQueryTasks.Remove(finishedTask);
            }

            items.Sort();

            return items;
        }

        private async Task<IEnumerable<CartItemJoin>> GetJoinedPizzaCartItemsAsync(int cartId)
        {
            string joinQuerySql = @"select c.Id, c.CartId, c.PricePerItem, c.Quantity, c.ProductCategory, c.Quantity,
                                    p.CartItemId, p.CheeseAmount, p.MenuPizzaCheeseId, p.MenuPizzaCrustFlavorId, p.MenuPizzaCrustId, p.MenuPizzaSauceId, p.SauceAmount, p.size
	                            from CartItem c
	                            inner join CartPizza p on c.Id = p.CartItemId
                                where c.CartId = @CartId";

            object queryParameters = new
            {
                CartId = cartId
            };

            IEnumerable<CartItemJoin> cartPizzaList = await connection.QueryAsync<CartItem, CartPizza, CartItemJoin>(
                joinQuerySql,
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

        public async Task<List<TEntity>> GetListAsync<TEntity>(object parameters, string orderByColumn) where TEntity : IRecord
        {
            string conditions = GetSqlWhereConditions(parameters, orderByColumn);
            IEnumerable<TEntity> list = await connection.GetListAsync<TEntity>(conditions, parameters);
            foreach (TEntity entity in list)
            {
                entity.MapEntity(this);
            }
            return list.ToList();
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

        // Special commands (confirm order, update cart item quantity, etc)
        public int CmdUpdateCartItemQuantity(int cartItemId, int quantity)
        {
            string updateQuerySql = @"update dbo.CartItem set quantity = @Quantity where Id = @Id;";

            object queryParameters = new
            {
                Id = cartItemId,
                Quantity = quantity
            };

            return connection.Execute(updateQuerySql, queryParameters);
        }

        // CRUD
        public dynamic Insert(IRecord entity)
        {
            if (entity.InsertRequiresTransaction())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    entity.Insert(this, transaction);
                    transaction.Commit();
                }
            }
            else
            {
                entity.Insert(this);
            }

            return entity.GetId();
        }

        public int Update(IRecord entity)
        {
            int rowsUpdated = 0;

            if (entity.UpdateRequiresTransaction())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    rowsUpdated = entity.Update(this, transaction);
                    transaction.Commit();
                }
            }
            else
            {
                rowsUpdated = entity.Update(this);
            }

            return rowsUpdated;
        }

        public int Delete<TEntity>(TEntity entity, IDbTransaction transaction = null) where TEntity : class, new()
        {
            return connection.Delete<TEntity>(entity, transaction);
        }
    }
}