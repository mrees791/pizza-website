using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace PizzaWebsite.Models.Identity
{
    public class UserClaim
    {
        private int id;
        private int userId;
        private string claimType;
        private string claimValue;

        public UserClaim(int id, int userId, string claimType, string claimValue)
        {
            this.id = id;
            this.userId = userId;
            this.claimType = claimType;
            this.claimValue = claimValue;
        }
    }
}