using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.Identity
{
    public class UserRole
    {
        private int id;
        private int userId;
        private int roleId;

        public UserRole(int id, int userId, int roleId)
        {
            this.id = id;
            this.userId = userId;
            this.roleId = roleId;
        }

        public int UserId { get => userId; }
        public int RoleId { get => roleId; }
    }
}