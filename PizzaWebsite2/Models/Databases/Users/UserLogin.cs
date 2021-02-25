using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWebsite2.Models.Databases.Users
{
    public class UserLogin
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public UserLoginInfo UserLoginInfo { get; set; }

        public UserLogin(int userId, UserLoginInfo userLoginInfo)
        {
            UserId = userId;
            UserLoginInfo = userLoginInfo;
        }
    }
}