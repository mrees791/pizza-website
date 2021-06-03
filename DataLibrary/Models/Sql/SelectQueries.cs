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
            return $"select {SqlUtility.CreateTopClause(onlySelectFirst)} " +
                @"Id, UserId, RoleName from dbo.UserRole";
        }

        internal static string GetUserLoginSelectQuery(bool onlySelectFirst)
        {
            return $"select {SqlUtility.CreateTopClause(onlySelectFirst)} " +
                @"Id, UserId, LoginProvider, ProviderKey from dbo.UserLogin";
        }

        internal static string GetSiteUserSelectQuery(bool onlySelectFirst)
        {
            return $"select {SqlUtility.CreateTopClause(onlySelectFirst)} " +
                @"Id, CurrentCartId, ConfirmOrderCartId, OrderConfirmationId, IsBanned, ZipCode, Email, EmailConfirmed, PasswordHash,
                  SecurityStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEndDateUtc, LockoutEnabled, AccessFailedCount from dbo.SiteUser";
        }

        internal static string GetEmployeeLocationSelectQuery(bool onlySelectFirst)
        {
            return $"select {SqlUtility.CreateTopClause(onlySelectFirst)} " + @"Id, EmployeeId, StoreId from dbo.EmployeeLocation";
        }

        internal static string GetCustomerOrderDeliveryInfoJoin(bool onlySelectFirst)
        {
            return $"select {SqlUtility.CreateTopClause(onlySelectFirst)} " +
                @"c.Id, c.UserId, c.StoreId, c.CartId, c.IsCancelled, c.OrderSubtotal, c.OrderTax, c.OrderTotal, c.OrderPhase,
                  c.OrderCompleted, c.DateOfOrder, c.IsDelivery, c.DeliveryInfoId, d.Id, d.DateOfDelivery, d.DeliveryAddressType, d.DeliveryAddressName,
                  d.DeliveryStreetAddress, d.DeliveryCity, d.DeliveryState, d.DeliveryZipCode, d.DeliveryPhoneNumber
                  from CustomerOrder c
                  left join DeliveryInfo d on c.DeliveryInfoId = d.Id";
        }
    }
}