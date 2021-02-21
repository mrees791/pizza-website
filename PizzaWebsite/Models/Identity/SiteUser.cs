using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace PizzaWebsite.Models.Identity
{
    public class SiteUser : IUser<int>
    {
        private int id;
        private string userName;

        public SiteUser(string userName)
        {
            this.userName = userName;
        }

        public int Id { get => id; set => id = value; }

        public string UserName { get => userName; set => userName = value; }
    }
}