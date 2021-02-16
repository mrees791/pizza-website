﻿using System;
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
    internal static class SqlDataAccess
    {
        internal static string GetConnectiongString(string connectionName = "PizzaDatabase")
        {
            return ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        }

        /// <summary>
        /// Loads a list of records from the database.
        /// </summary>
        /// <typeparam name="T">Model data-type</typeparam>
        /// <param name="sql">Select SQL query</param>
        /// <returns>List of records</returns>
        internal static List<T> LoadData<T>(string sql)
        {
            using (IDbConnection connection = new SqlConnection(GetConnectiongString()))
            {
                return connection.Query<T>(sql).ToList();
            }
        }

        internal static T LoadSingleRecord<T>(string selectSql, object parameters)
        {
            using (IDbConnection connection = new SqlConnection(GetConnectiongString()))
            {
                return connection.Query<T>(selectSql, parameters).FirstOrDefault();
            }
        }

        /// <summary>
        /// Updates a record using an updated model.
        /// </summary>
        /// <typeparam name="T">Model data-type</typeparam>
        /// <param name="sql">Update SQL query</param>
        /// <param name="data">Updated model</param>
        /// <returns>Number of rows affected</returns>
        internal static int UpdateRecord<T>(string sql, T data)
        {
            using (IDbConnection connection = new SqlConnection(GetConnectiongString()))
            {
                return connection.Execute(sql, data);
            }
        }

        internal static int UpdateRecord<T>(string sql, T data, IDbConnection connection, IDbTransaction transaction)
        {
            return connection.Execute(sql, data, transaction);
        }

        internal static int UpdateRecord(string sql, object data, IDbConnection connection, IDbTransaction transaction)
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
        internal static int UpdateRecord(string sql, object data)
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
        internal static int SaveNewRecord<T>(string sql, T data)
        {
            using (IDbConnection connection = new SqlConnection(GetConnectiongString()))
            {
                return connection.Query<int>(sql, data).Single();
            }
        }

        internal static int SaveNewRecord<T>(string sql, T data, IDbConnection connection, IDbTransaction transaction)
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
        internal static int SaveNewRecord(string sql, object data, IDbConnection connection)
        {
            return SaveNewRecord(sql, data, connection, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <param name="sql"></param>
        /// <param name="data"></param>
        /// <returns>ID of newly created record</returns>
        internal static int SaveNewRecord(string sql, object data, IDbConnection connection, IDbTransaction transaction)
        {
            return connection.Query<int>(sql, data, transaction).Single();
        }

        internal static int DeleteRecord<T>(string sql, T data, IDbConnection connection, IDbTransaction transaction)
        {
            return connection.Execute(sql, data, transaction);
        }
    }
}
