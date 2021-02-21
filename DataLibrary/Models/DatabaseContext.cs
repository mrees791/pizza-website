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
    // todo: Complete database context implementation.
    /// <summary>
    /// Encapsulates a database connection and CRUD operations.
    /// </summary>
    public class DatabaseContext : IDisposable
    {
        private IDbConnection _connection;

        public DatabaseContext(string connectionName = "PizzaDatabase")
        {
            string connectionString = GetConnectiongString(connectionName);
            _connection = new SqlConnection(connectionString);
            _connection.Open();
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        private string GetConnectiongString(string connectionName)
        {
            return ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        }
    }
}
