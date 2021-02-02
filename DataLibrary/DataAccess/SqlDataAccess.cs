using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DataLibrary.DataAccess
{
    public static class SqlDataAccess
    {
        public static string GetConnectiongString(string connectionName = "PizzaDatabase")
        {
            return ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        }

        /// <summary>
        /// Loads a list of records from the database.
        /// </summary>
        /// <typeparam name="T">Model data-type</typeparam>
        /// <param name="sql">Select SQL query</param>
        /// <returns>List of records</returns>
        public static List<T> LoadData<T>(string sql)
        {
            using (IDbConnection connection = new SqlConnection(GetConnectiongString()))
            {
                return connection.Query<T>(sql).ToList();
            }
        }

        /// <summary>
        /// Updates a record using an updated model.
        /// </summary>
        /// <typeparam name="T">Model data-type</typeparam>
        /// <param name="sql">Update SQL query</param>
        /// <param name="data">Updated model</param>
        /// <returns>Number of rows affected</returns>
        public static int UpdateRecord<T>(string sql, T data)
        {
            using (IDbConnection connection = new SqlConnection(GetConnectiongString()))
            {
                return connection.Execute(sql, data);
            }
        }

        public static int UpdateRecord(IDbConnection connection, IDbTransaction transaction, string sql, object data)
        {
            return connection.Execute(sql, data, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="data"></param>
        /// <returns>Number of rows affected.</returns>
        public static int UpdateRecord(string sql, object data)
        {
            using (IDbConnection connection = new SqlConnection(GetConnectiongString()))
            {
                return connection.Execute(sql, data);
            }
        }

        /// <summary>
        /// Adds a new record using an SQL insert query and returns the ID of the newly created record.
        /// The insert query must include "output Inserted.Id" in order to return the newly created ID.
        /// </summary>
        /// <typeparam name="T">Model data-type</typeparam>
        /// <param name="sql">Insert SQL query</param>
        /// <param name="data">Model</param>
        /// <returns>ID of newly created record</returns>
        public static int SaveNewRecord<T>(string sql, T data)
        {
            using (IDbConnection connection = new SqlConnection(GetConnectiongString()))
            {
                return connection.Query<int>(sql, data).Single();
            }
        }

        public static int SaveNewRecord<T>(string sql, T data, IDbConnection connection, IDbTransaction transaction)
        {
            return connection.Query<int>(sql, data, transaction).Single();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sql"></param>
        /// <param name="data"></param>
        /// <returns>ID of newly created record</returns>
        public static int SaveNewRecord(string sql, object data, IDbConnection connection)
        {
            return connection.Query<int>(sql, data).Single();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <param name="sql"></param>
        /// <param name="data"></param>
        /// <returns>ID of newly created record</returns>
        public static int SaveNewRecord(string sql, object data, IDbConnection connection, IDbTransaction transaction)
        {
            return connection.Query<int>(sql, data, transaction).Single();
        }

        public static int DeleteRecord<T>(string sql, T data, IDbConnection connection, IDbTransaction transaction)
        {
            return connection.Execute(sql, data, transaction);
        }
    }
}
