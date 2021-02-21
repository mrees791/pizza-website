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
        private Claim claim;

        public UserClaim(int userId, Claim claim)
        {
            this.userId = userId;
            this.claim = claim;
        }

        public int Id { get => id; set => id = value; }
        public int UserId { get => userId; }
        public Claim Claim { get => claim; }
    }
}