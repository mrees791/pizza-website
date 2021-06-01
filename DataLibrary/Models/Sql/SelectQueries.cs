using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Sql
{
    internal static class SelectQueries
    {
        // todo: Change these to methods with (bool selectOnlyTopRecord) parameter.
        internal static readonly string siteRoleSelectQuery = @"select Name from dbo.SiteRole ";
        internal static readonly string userRoleSelectQuery = @"select Id, UserId, RoleName from dbo.UserRole ";
        internal static readonly string userLoginSelectQuery = @"select Id, UserId, LoginProvider, ProviderKey from dbo.UserLogin ";
        internal static readonly string siteUserSelectQuery = @"select Id, CurrentCartId, ConfirmOrderCartId, OrderConfirmationId, IsBanned, ZipCode, Email, EmailConfirmed, 
                     PasswordHash, SecurityStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEndDateUtc, LockoutEnabled, 
                     AccessFailedCount from dbo.SiteUser ";

        private static string CreateSelectQuery(bool selectOnlyTopRecord)
        {
            string topClause = "";

            if (selectOnlyTopRecord)
            {
                topClause = "top 1 ";
            }

            return $"select {topClause}";
        }

        internal static string GetEmployeeLocationSelectQuery(bool selectOnlyTopRecord)
        {
            string selectQuery = CreateSelectQuery(selectOnlyTopRecord);

            selectQuery += @"Id, EmployeeId, StoreId from dbo.EmployeeLocation ";

            return selectQuery;
        }

        internal static string GetEmployeeLocationOnStoreLocationJoin(bool selectOnlyTopRecord)
        {
            string joinQuery = CreateSelectQuery(selectOnlyTopRecord);
            joinQuery += @"l.Id, l.EmployeeId, l.StoreId, s.Id, s.Name, s.StreetAddress, s.City, s.State, s.ZipCode, s.PhoneNumber, s.isActiveLocation
                           from EmployeeLocation l inner join StoreLocation s on l.StoreId = s.Id ";

            return joinQuery;
        }

        internal static string GetEmployeeOnEmployeeLocationJoin(bool selectOnlyTopRecord)
        {
            string joinQuery = CreateSelectQuery(selectOnlyTopRecord);

            joinQuery += @"e.Id, e.UserId, l.Id, l.EmployeeId, l.StoreId from Employee e inner join EmployeeLocation l on l.EmployeeId = e.Id ";

            return joinQuery;
        }

        internal static string GetCustomerOrderDeliveryInfoJoin(bool selectOnlyTopRecord)
        {
            string joinQuery = CreateSelectQuery(selectOnlyTopRecord);

            joinQuery += @"c.Id, c.UserId, c.StoreId, c.CartId, c.IsCancelled, 
                                 c.OrderSubtotal, c.OrderTax, c.OrderTotal, c.OrderPhase,
                                 c.OrderCompleted, c.DateOfOrder, c.IsDelivery, c.DeliveryInfoId,
                                 d.Id, d.DateOfDelivery, d.DeliveryAddressType, d.DeliveryAddressName,
                                 d.DeliveryStreetAddress, d.DeliveryCity, d.DeliveryState, d.DeliveryZipCode,
                                 d.DeliveryPhoneNumber
                                 from CustomerOrder c
                                 left join DeliveryInfo d on c.DeliveryInfoId = d.Id ";

            return joinQuery;
        }
    }
}