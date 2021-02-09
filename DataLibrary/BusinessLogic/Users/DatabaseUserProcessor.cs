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
        /// <summary>
        /// Adds a new user record and cart records.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="email"></param>
        /// <param name="passwordHash"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="zipCode"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <returns>ID of the newly created user.</returns>
        internal static int AddNewUser(string userName, string email, string passwordHash, string phoneNumber, string zipCode, IDbConnection connection, IDbTransaction transaction)
        {
            // Add current cart record
            int currentCartId = DatabaseCartProcessor.AddNewCart(connection, transaction);

            // Add confirm order cart record
            int confirmOrderCartId = DatabaseCartProcessor.AddNewCart(connection, transaction);

            // Add new user record
            string insertUserSql = @"insert into [dbo].[User] (UserName, Email, PasswordHash, CurrentCartId, ConfirmOrderCartId, IsBanned, EmailConfirmed, PhoneNumber, PhoneNumberConfirmed, ZipCode)
                                     output Inserted.Id values (@UserName, @Email, @PasswordHash, @CurrentCartId, @ConfirmOrderCartId, @IsBanned, @EmailConfirmed, @PhoneNumber, @PhoneNumberConfirmed, @ZipCode);";

            object queryParameters = new
            {
                UserName = userName,
                Email = email,
                PasswordHash = passwordHash,
                CurrentCartId = currentCartId,
                ConfirmOrderCartId = confirmOrderCartId,
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

        /// <summary>
        /// Adds a new user record and cart records.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="passwordHash"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="zipCode"></param>
        /// <returns>ID of the newly created user.</returns>
        public static int AddNewUser(string userName, string email, string passwordHash, string phoneNumber, string zipCode)
        {
            int newUserId = 0;

            using (IDbConnection connection = new SqlConnection(SqlDataAccess.GetConnectiongString()))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        newUserId = AddNewUser(userName, email, passwordHash, phoneNumber, zipCode, connection, transaction);
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

        /// <summary>
        /// Attempts to return the user's model if their username and password is correct.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passwordHash"></param>
        /// <returns>User's UserModel if it was a successful sign in. Null if it was not a successful sign in.</returns>
        public static UserModel SignInUser(string userName, string passwordHash)
        {
            string selectUserSql = @"select TOP 1 Id, UserName, Email, PasswordHash, CurrentCartId, ConfirmOrderCartId, IsBanned, EmailConfirmed, PhoneNumber, PhoneNumberConfirmed, ZipCode
                                     from [dbo].[user] where UserName=@UserName and PasswordHash=@PasswordHash order by Id asc;";

            object queryParameters = new
            {
                UserName = userName,
                PasswordHash = passwordHash
            };

            return SqlDataAccess.LoadSingleRecord<UserModel>(selectUserSql, queryParameters);
        }
    }
}
