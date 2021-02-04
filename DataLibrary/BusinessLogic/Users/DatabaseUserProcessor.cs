using DataLibrary.BusinessLogic.Carts;
using DataLibrary.DataAccess;
using DataLibrary.Models.Users;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic.Users
{
    public static class DatabaseUserProcessor
    {
        internal static int AddNewUser(string email, string passwordHash, string phoneNumber, string zipCode, IDbConnection connection, IDbTransaction transaction)
        {
            // Add new cart record
            int newCartId = DatabaseCartProcessor.AddNewCart(connection, transaction);

            // Add new user record
            string insertUserSql = @"insert into [dbo].[User] (CurrentCartId, Email, PasswordHash, IsBanned, EmailConfirmed, PhoneNumber, PhoneNumberConfirmed, ZipCode)
                                     output Inserted.Id values (@CurrentCartId, @Email, @PasswordHash, @IsBanned, @EmailConfirmed, @PhoneNumber, @PhoneNumberConfirmed, @ZipCode);";

            object queryParameters = new
            {
                CurrentCartId = newCartId,
                Email = email,
                PasswordHash = passwordHash,
                IsBanned = false,
                EmailConfirmed = false,
                PhoneNumber = phoneNumber,
                PhoneNumberConfirmed = false,
                ZipCode = zipCode
            };

            int newUserId = SqlDataAccess.SaveNewRecord(insertUserSql, queryParameters, connection, transaction);

            // Add new user role record
            var userRole = new UserRoleModel()
            {
                Name = "User",
                UserId = newUserId
            };

            userRole.Id = AddUserRole(userRole, connection, transaction);

            return newUserId;
        }

        public static int AddNewUser(string email, string passwordHash, string phoneNumber, string zipCode)
        {
            int newUserId = 0;

            using (IDbConnection connection = new SqlConnection(SqlDataAccess.GetConnectiongString()))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        newUserId = AddNewUser(email, passwordHash, phoneNumber, zipCode, connection, transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            return newUserId;
        }

        internal static int AddUserRole(UserRoleModel userRole, IDbConnection connection, IDbTransaction transaction)
        {
            string insertSql = @"insert into dbo.UserRole (UserId, Name) output Inserted.Id values (@UserId, @Name);";

            userRole.Id = SqlDataAccess.SaveNewRecord(insertSql, userRole, connection, transaction);

            return userRole.Id;
        }
    }
}
