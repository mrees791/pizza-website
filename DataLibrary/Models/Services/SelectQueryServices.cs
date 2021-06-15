using DataLibrary.Models.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Services
{
    internal class SelectQueryServices
    {
        private SqlServices _sqlServices;

        internal SelectQueryServices()
        {
            _sqlServices = new SqlServices();
        }

        internal string GetUserRoleSelectQuery(bool onlySelectFirst)
        {
            return $@"SELECT {_sqlServices.CreateTopClause(onlySelectFirst)}
                      Id, UserId, RoleName
                      FROM UserRole";
        }

        internal string GetUserLoginSelectQuery(bool onlySelectFirst)
        {
            return $@"SELECT {_sqlServices.CreateTopClause(onlySelectFirst)}
                      Id, UserId, LoginProvider, ProviderKey
                      FROM UserLogin";
        }

        internal string GetSiteUserSelectQuery(bool onlySelectFirst)
        {
            return $@"SELECT {_sqlServices.CreateTopClause(onlySelectFirst)}
                      Id, CurrentCartId, ConfirmOrderCartId, OrderConfirmationId, IsBanned, ZipCode, Email, EmailConfirmed, PasswordHash,
                      SecurityStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEndDateUtc, LockoutEnabled, AccessFailedCount
                      FROM SiteUser";
        }

        internal string GetEmployeeLocationSelectQuery(bool onlySelectFirst)
        {
            return $@"SELECT {_sqlServices.CreateTopClause(onlySelectFirst)}
                      Id, EmployeeId, StoreId
                      FROM EmployeeLocation";
        }

        internal string GetEmployeeSelectQuery(bool onlySelectFirst)
        {
            return $@"SELECT {_sqlServices.CreateTopClause(onlySelectFirst)}
                      Id, UserId
                      FROM Employee";
        }
    }
}
