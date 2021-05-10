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
    }
}