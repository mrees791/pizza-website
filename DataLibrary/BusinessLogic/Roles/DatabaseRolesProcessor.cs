using DataLibrary.DataAccess;
using DataLibrary.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic.Roles
{
    public static class DatabaseRolesProcessor
    {

        internal static List<UserRoleModel> LoadAllUserRoles()
        {
            string selectUserSql = @"select Id, UserId, Name from dbo.UserRole;";
            return SqlDataAccess.LoadData<UserRoleModel>(selectUserSql);
        }

        public static IList<string> FindByUserId(int userId)
        {
            return LoadAllUserRoles().Where(r => r.UserId == userId).Select(r => r.Name).ToList();
        }

        public static int GetRoleId(string roleName)
        {
            UserRoleModel role = LoadAllUserRoles().Where(r => r.Name == roleName).FirstOrDefault();
            return role.Id;
        }

        /// <summary>
        /// Adds a user to a role by adding a new record in the UserRole table.
        /// </summary>
        /// <param name="userRole"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <returns>ID of the newly created user role record.</returns>
        /*internal static int AddUserRole(UserModel user, string roleName, IDbConnection connection, IDbTransaction transaction)
        {
            string insertSql = @"insert into dbo.UserRole (UserId, Name) output Inserted.Id values (@UserId, @Name);";

            UserRoleModel userRole = new UserRoleModel()
            {
                Name = roleName,
                UserId = user.Id
            };

            return SqlDataAccess.SaveNewRecord(insertSql, userRole, connection, transaction);
        }

        internal static int RemoveAllUserRoles(UserModel user, IDbConnection connection, IDbTransaction transaction)
        {
            string deleteRolesSql = @"delete from dbo.UserRole where UserId = @Id;";
            return SqlDataAccess.DeleteRecord<UserModel>(deleteRolesSql, user, connection, transaction);
        }

        public static List<UserRoleModel> LoadUserRoles(int userId)
        {
            return LoadAllUserRoles().Where(r => r.UserId == userId).ToList();
        }

        public static IList<string> LoadUserRoleNames(int userId)
        {
            return LoadUserRoles(userId).Select(r => r.Name).ToList();
        }
        public static bool UserIsInRole(int userId, string roleName)
        {
            return LoadUserRoleNames(userId).Contains(roleName);
        }*/
    }
}