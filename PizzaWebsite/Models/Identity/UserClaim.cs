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

        public UserClaim(int userId, string claimType, string claimValue)
        {
            this.userId = userId;
            this.claimType = claimType;
            this.claimValue = claimValue;
        }

        public int Id { get => id; set => id = value; }
        public int UserId { get => userId; }
        public string ClaimType { get => claimType; }
        public string ClaimValue { get => claimValue; }
    }
}