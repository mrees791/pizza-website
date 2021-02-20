using Dapper;
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
        internal static int AddNewUser(UserModel user, IDbConnection connection, IDbTransaction transaction)
        {
            // Add current cart record
            int currentCartId = DatabaseCartProcessor.AddNewCart(connection, transaction);

            // Add confirm order cart record
            int confirmOrderCartId = DatabaseCartProcessor.AddNewCart(connection, transaction);

            // Add new user record
            string insertUserSql = @"insert into [dbo].[User] (UserName, Email, PasswordHash, CurrentCartId, ConfirmOrderCartId, OrderConfirmationId, IsBanned, EmailConfirmed, PhoneNumber, PhoneNumberConfirmed, ZipCode)
                                     output Inserted.Id values (@UserName, @Email, @PasswordHash, @CurrentCartId, @ConfirmOrderCartId, @OrderConfirmationId, @IsBanned, @EmailConfirmed, @PhoneNumber, @PhoneNumberConfirmed, @ZipCode);";

            user.CurrentCartId = currentCartId;
            user.ConfirmOrderCartId = confirmOrderCartId;

            user.Id = SqlDataAccess.SaveNewRecord<UserModel>(insertUserSql, user, connection, transaction);

            return user.Id;
        }

        /// <summary>
        /// Adds a new user record and cart records.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="passwordHash"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="zipCode"></param>
        /// <returns>ID of the newly created user.</returns>
        public static int AddNewUser(UserModel user)
        {
            using (IDbConnection connection = new SqlConnection(SqlDataAccess.GetConnectiongString()))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        user.Id = AddNewUser(user, connection, transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            return user.Id;
        }

        public static int UpdateUser(UserModel user)
        {
            int rowsAffected = 0;

            using (IDbConnection connection = new SqlConnection(SqlDataAccess.GetConnectiongString()))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        rowsAffected = UpdateUser(user, connection, transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            return rowsAffected;
        }

        internal static List<UserModel> LoadUsers()
        {
            string selectUserSql = @"select Id, UserName, Email, PasswordHash, CurrentCartId, ConfirmOrderCartId, OrderConfirmationId, IsBanned, EmailConfirmed, PhoneNumber, PhoneNumberConfirmed, ZipCode
                                     from [dbo].[User] order by Id asc;";

            return SqlDataAccess.LoadData<UserModel>(selectUserSql);
        }

        public static int GetNumberOfUsers()
        {
            string countSql = @"select COUNT(Id) from [dbo].[User];";

            using (IDbConnection connection = new SqlConnection(SqlDataAccess.GetConnectiongString()))
            {
                return connection.Query<int>(countSql).Single();
            }
        }

        /// <summary>
        /// Attempts to return the user's model if their username and password is correct.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passwordHash"></param>
        /// <returns>User's UserModel if it was a successful sign in. Null if it was not a successful sign in.</returns>
        public static UserModel SignInUser(string userName, string passwordHash)
        {
            return LoadUsers().Where(u => u.UserName == userName && u.PasswordHash == passwordHash).FirstOrDefault();
        }

        public static UserModel FindUserById(int userId)
        {
            return LoadUsers().Where(u => u.Id == userId).FirstOrDefault();
        }

        public static UserModel FindUserByName(string userName)
        {
            return LoadUsers().Where(u => u.UserName == userName).FirstOrDefault();
        }

        public static UserModel FindUserByEmail(string email)
        {
            return LoadUsers().Where(u => u.Email == email).FirstOrDefault();
        }

        public static UserModel FindUserByPhoneNumber(string phoneNumber)
        {
            return LoadUsers().Where(u => u.PhoneNumber == phoneNumber).FirstOrDefault();
        }

        public static string GetPasswordHash(int userId)
        {
            return FindUserById(userId).PasswordHash;
        }

        internal static int UpdateUser(UserModel updatedUser, IDbConnection connection, IDbTransaction transaction)
        {
            string updateSql = @"update [dbo].[User] set Email = @Email, PasswordHash = @PasswordHash, OrderConfirmationId = @OrderConfirmationId,
                                 IsBanned = @IsBanned, EmailConfirmed = @EmailConfirmed, PhoneNumber = @PhoneNumber, PhoneNumberConfirmed = @PhoneNumberConfirmed
                                 ZipCode = @ZipCode where Id = @Id;";

            return SqlDataAccess.UpdateRecord<UserModel>(updateSql, updatedUser, connection, transaction);
        }
    }
}
