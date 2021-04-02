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
        public async Task<TEntity> GetAsync<TEntity>(object id, IDbTransaction transaction = null) where TEntity : class, new()
        {
            TEntity entity = await connection.GetAsync<TEntity>(id, transaction);
            MapEntityListProperties(entity);
            return entity;
        }

        public TEntity Get<TEntity>(object id, IDbTransaction transaction = null) where TEntity : class, new()
        {
            TEntity entity = connection.Get<TEntity>(id, transaction);
            MapEntityListProperties(entity);
            return entity;
        }

        public List<TEntity> GetList<TEntity>() where TEntity : class, new()
        {
            List<TEntity> list = connection.GetList<TEntity>().ToList();
            foreach (TEntity entity in list)
            {
                MapEntityListProperties(entity);
            }
            return list;
        }

        public List<TEntity> GetList<TEntity>(object parameters, string orderByColumn) where TEntity : class, new()
        {
            string conditions = GetSqlWhereConditions(parameters, orderByColumn);
            List<TEntity> list = connection.GetList<TEntity>(conditions, parameters).ToList();
            foreach (TEntity entity in list)
            {
                MapEntityListProperties(entity);
            }
            return list;
        }

        public async Task<List<TEntity>> GetListAsync<TEntity>() where TEntity : class, new()
        {
            IEnumerable<TEntity> list = await connection.GetListAsync<TEntity>();
            foreach (TEntity entity in list)
            {
                MapEntityListProperties(entity);
            }
            return list.ToList();
        }

        public async Task<List<TEntity>> GetListAsync<TEntity>(object whereConditions) where TEntity : class, new()
        {
            IEnumerable<TEntity> list = await connection.GetListAsync<TEntity>(whereConditions);
            foreach (TEntity entity in list)
            {
                MapEntityListProperties(entity);
            }
            return list.ToList();
        }

        public async Task<List<TEntity>> GetListAsync<TEntity>(object searchFilter, object parameters) where TEntity : class, new()
        {
            IEnumerable<TEntity> list = await connection.GetListAsync<TEntity>(GetSqlWhereFilterConditions(searchFilter), parameters);
            foreach (TEntity entity in list)
            {
                MapEntityListProperties(entity);
            }
            return list.ToList();
        }

        public async Task<List<TEntity>> GetListPagedAsync<TEntity>(object searchFilter, int pageNumber, int rowsPerPage, string orderby) where TEntity : class, new()
        {
            string conditions = GetSqlWhereFilterConditions(searchFilter);
            IEnumerable<TEntity> list = await connection.GetListPagedAsync<TEntity>(pageNumber, rowsPerPage, conditions, orderby, searchFilter);
            foreach (TEntity entity in list)
            {
                MapEntityListProperties(entity);
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

        public int Insert(CartItem cartItem, CartPizza cartPizza)
        {
            int cartItemId = 0;

            using (var transaction = connection.BeginTransaction())
            {
                cartItemId = connection.Insert<CartItem>(cartItem, transaction).Value;

                cartPizza.CartItemId = cartItemId;
                connection.Query(@"INSERT INTO
                                   CartPizza (CartItemId, Size, MenuPizzaCrustId, MenuPizzaSauceId, SauceAmount, MenuPizzaCheeseId, CheeseAmount, MenuPizzaCrustFlavorId)
                                   VALUES (@CartItemId, @Size, @MenuPizzaCrustId, @MenuPizzaSauceId, @SauceAmount, @MenuPizzaCheeseId, @CheeseAmount, @MenuPizzaCrustFlavorId)",
                                   cartPizza, transaction);

                foreach (CartPizzaTopping topping in cartPizza.Toppings)
                {
                    topping.CartItemId = cartPizza.CartItemId;
                    connection.Insert<CartPizzaTopping>(topping, transaction);
                }

                transaction.Commit();
            }

            return cartItemId;
        }

        public int Update<TEntity>(TEntity entity, IDbTransaction transaction = null) where TEntity : class, new()
        {
            if (entity is MenuPizza)
            {
                return UpdateMenuPizza(entity as MenuPizza);
            }
            return connection.Update<TEntity>(entity, transaction);
        }

        public int Delete<TEntity>(TEntity entity, IDbTransaction transaction = null) where TEntity : class, new()
        {
            return connection.Delete<TEntity>(entity, transaction);
        }

        // Properly maps a table's list properties.
        internal void MapEntityListProperties<TEntity>(TEntity entity) where TEntity : class, new()
        {
            if (entity is MenuPizza)
            {
                MenuPizza menuPizza = entity as MenuPizza;
                menuPizza.Toppings = GetList<MenuPizzaTopping>(new { MenuPizzaId = menuPizza.Id }, "Id");
            }
            else if (entity is CartPizza)
            {
                CartPizza cartPizza = entity as CartPizza;
                cartPizza.Toppings = GetList<CartPizzaTopping>(new { CartItemId = cartPizza.CartItemId }, "Id");
            }
        }

        // Cart CRUD
        private int InsertCart(IDbTransaction transaction = null)
        {
            // We had to use Query<int> instead of Insert because the Insert method will not work with SQL DEFAULT VALUES.
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

                // Insert toppings
                foreach (MenuPizzaTopping topping in entity.Toppings)
                {
                    topping.MenuPizzaId = id;
                    connection.Insert<MenuPizzaTopping>(topping, transaction);
                }

                transaction.Commit();

                return id;
            }
        }

        private int UpdateMenuPizza(MenuPizza entity)
        {
            using (var transaction = connection.BeginTransaction())
            {
                // Delete previous toppings
                connection.DeleteList<MenuPizzaTopping>(new { MenuPizzaId = entity.Id }, transaction);

                // Insert new toppings
                foreach (MenuPizzaTopping topping in entity.Toppings)
                {
                    connection.Insert<MenuPizzaTopping>(topping, transaction);
                }

                // Update pizza record
                int rowsAffected = connection.Update<MenuPizza>(entity, transaction);

                transaction.Commit();

                return rowsAffected;
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