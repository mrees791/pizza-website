using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWebsite2.Models.Databases.Users
{
    public class UserRole
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }

        public UserRole(int userId, int roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }
    }
}