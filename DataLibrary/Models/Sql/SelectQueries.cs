using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Sql
{
    internal static class SelectQueries
    {
        internal static readonly string siteRoleSelectQuery = @"select Id, Name from dbo.SiteRole ";
        internal static readonly string userLoginSelectQuery = @"select Id, UserId, LoginProvider, ProviderKey from dbo.UserLogin ";
        internal static readonly string siteUserSelectQuery = @"select Id, CurrentCartId, ConfirmOrderCartId, OrderConfirmationId, IsBanned, ZipCode, Email, EmailConfirmed, 
                     PasswordHash, SecurityStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEndDateUtc, LockoutEnabled, 
                     AccessFailedCount, UserName from dbo.SiteUser ";
        internal static readonly string customerOrderDeliveryInfoJoin = @"select c.Id, c.UserId, c.StoreId, c.CartId, c.IsCancelled, 
                                 c.OrderSubtotal, c.OrderTax, c.OrderTotal, c.OrderPhase,
                                 c.OrderCompleted, c.DateOfOrder, c.IsDelivery, c.DeliveryInfoId,
                                 d.Id, d.DateOfDelivery, d.DeliveryAddressType, d.DeliveryAddressName,
                                 d.DeliveryStreetAddress, d.DeliveryCity, d.DeliveryState, d.DeliveryZipCode,
                                 d.DeliveryPhoneNumber
                                 from CustomerOrder c
                                 left join DeliveryInfo d on c.DeliveryInfoId = d.Id ";
    }
}