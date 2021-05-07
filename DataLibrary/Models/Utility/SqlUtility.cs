using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Utility
{
    public static class SqlUtility
    {
        public static string GetSiteRoleSelectSql()
        {
            return @"select Id, Name from dbo.SiteRole ";
        }

        public static string GetSiteUserSelectSql()
        {
            return @"select Id, CurrentCartId, ConfirmOrderCartId, OrderConfirmationId, IsBanned, ZipCode, Email, EmailConfirmed, 
                     PasswordHash, SecurityStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEndDateUtc, LockoutEnabled, 
                     AccessFailedCount, UserName from dbo.SiteUser ";
        }

        public static string GetUserLoginSelectSql()
        {
            return @"select Id, UserId, LoginProvider, ProviderKey from dbo.UserLogin ";
        }
    }
}