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

        public static List<T> LoadData<T>(string sql)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectiongString()))
            {
                return cnn.Query<T>(sql).ToList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="data"></param>
        /// <returns>Number of rows affected.</returns>
        public static int UpdateRecord<T>(string sql, T data)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectiongString()))
            {
                return cnn.Execute(sql, data);
            }
        }

        /// <summary>
        /// Adds a new record using an SQL insert query and returns the ID of the newly created record.
        /// The insert query must include "output Inserted.Id" in order to return the newly created ID.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="data"></param>
        /// <returns>Id of newly created record.</returns>
        public static int SaveNewRecord<T>(string sql, T data)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectiongString()))
            {
                return cnn.Query<int>(sql, data).Single();
            }
        }
    }
}
