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

        public UserRole(int userId, int roleId)
        {
            this.userId = userId;
            this.roleId = roleId;
        }

        public int UserId { get => userId; }
        public int RoleId { get => roleId; }
        public int Id { get => id; set => id = value; }
    }
}