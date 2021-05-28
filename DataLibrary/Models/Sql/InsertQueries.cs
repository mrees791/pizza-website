using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Sql
{
    internal static class InsertQueries
    {
        internal static readonly string siteUserInsertQuery = @"INSERT INTO SiteUser (Id, CurrentCartId, ConfirmOrderCartId, OrderConfirmationId, IsBanned, ZipCode, Email, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEndDateUtc, LockoutEnabled, AccessFailedCount)
                                                                VALUES (@Id, @CurrentCartId, @ConfirmOrderCartId, @OrderConfirmationId, @IsBanned, @ZipCode, @Email, @EmailConfirmed, @PasswordHash, @SecurityStamp, @PhoneNumber, @PhoneNumberConfirmed, @TwoFactorEnabled, @LockoutEndDateUtc, @LockoutEnabled, @AccessFailedCount)";
        internal static readonly string employeeInsertQuery = "INSERT INTO Employee (Id, UserId) VALUES (@Id, @UserId)";
    }
}