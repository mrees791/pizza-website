﻿using DataLibrary.DataAccess;
using DataLibrary.Models.Carts;
using DataLibrary.Models.Users;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic
{
    public static class DatabaseUserProcessor
    {
        public static int AddNewUser(string email, string passwordHash, string phoneNumber, string zipCode)
        {
            int userId = 0;

            using (IDbConnection connection = new SqlConnection(SqlDataAccess.GetConnectiongString()))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        userId = AddNewUser(email, passwordHash, phoneNumber, zipCode, connection, transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            return userId;
        }

        internal static int AddNewUser(string email, string passwordHash, string phoneNumber, string zipCode, IDbConnection connection, IDbTransaction transaction)
        {
            string insertUserSql = @"insert into [dbo].[User] (CurrentCartId, Email, PasswordHash, IsBanned, EmailConfirmed, PhoneNumber, PhoneNumberConfirmed, ZipCode)
                                 output Inserted.Id values (@CurrentCartId, @Email, @PasswordHash, @IsBanned, @EmailConfirmed, @PhoneNumber, @PhoneNumberConfirmed, @ZipCode);";


            CartModel newCart = new CartModel();

            // Add new cart record
            newCart.Id = DatabaseCartProcessor.AddNewCart(newCart, connection, transaction);

            // Add new user record
            int userId = SqlDataAccess.SaveNewRecord(insertUserSql,
                new
                {
                    CurrentCartId = newCart.Id,
                    Email = email,
                    PasswordHash = passwordHash,
                    IsBanned = false,
                    EmailConfirmed = false,
                    PhoneNumber = phoneNumber,
                    PhoneNumberConfirmed = false,
                    ZipCode = zipCode
                },
                connection, transaction);

            // Add new user role record
            var userRole = new UserRoleModel()
            {
                Name = "User",
                UserId = userId
            };

            userRole.Id = AddUserRole(userRole, connection, transaction);

            return userId;
        }

        public static int AddUserRole(UserRoleModel userRole)
        {
            using (IDbConnection connection = new SqlConnection(SqlDataAccess.GetConnectiongString()))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        userRole.Id = AddUserRole(userRole, connection, transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            return userRole.Id;
        }

        internal static int AddUserRole(UserRoleModel userRole, IDbConnection connection, IDbTransaction transaction)
        {
            string insertSql = @"insert into dbo.UserRole (UserId, Name) output Inserted.Id values (@UserId, @Name);";

            userRole.Id = SqlDataAccess.SaveNewRecord(insertSql, userRole, connection, transaction);

            return userRole.Id;
        }
    }
}
