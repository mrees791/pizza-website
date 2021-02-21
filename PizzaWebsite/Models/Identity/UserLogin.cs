using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.Identity
{
    /// <summary>
    /// Provides login data for external login APIs (Google, FaceBook)
    /// </summary>
    public class UserLogin
    {
        private int id;
        private int userId;
        private UserLoginInfo userLoginInfo;

        public UserLogin(int userId, UserLoginInfo userLoginInfo)
        {
            this.userId = userId;
            this.userLoginInfo = userLoginInfo;
        }

        public int Id { get => id; set => id = value; }
        public int UserId { get => userId; }
        public UserLoginInfo UserLoginInfo { get => userLoginInfo; }
    }
}