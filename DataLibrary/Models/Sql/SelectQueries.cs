using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Sql
{
    internal static class SelectQueries
    {
        internal static string GetUserRoleSelectQuery(bool onlySelectFirst)
        {
            return $@"SELECT {SqlUtility.CreateTopClause(onlySelectFirst)}
                      Id, UserId, RoleName
                      FROM UserRole";
        }

        internal static string GetUserLoginSelectQuery(bool onlySelectFirst)
        {
            return $@"SELECT {SqlUtility.CreateTopClause(onlySelectFirst)}
                      Id, UserId, LoginProvider, ProviderKey
                      FROM UserLogin";
        }

        internal static string GetSiteUserSelectQuery(bool onlySelectFirst)
        {
            return $@"SELECT {SqlUtility.CreateTopClause(onlySelectFirst)}
                      Id, CurrentCartId, ConfirmOrderCartId, OrderConfirmationId, IsBanned, ZipCode, Email, EmailConfirmed, PasswordHash,
                      SecurityStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEndDateUtc, LockoutEnabled, AccessFailedCount
                      FROM SiteUser";
        }

        internal static string GetEmployeeLocationSelectQuery(bool onlySelectFirst)
        {
            return $@"SELECT {SqlUtility.CreateTopClause(onlySelectFirst)}
                      Id, EmployeeId, StoreId
                      FROM EmployeeLocation";
        }
    }
}