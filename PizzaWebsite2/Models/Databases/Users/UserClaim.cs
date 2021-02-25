using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace PizzaWebsite2.Models.Databases.Users
{
    public class UserClaim
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public Claim Claim { get; set; }

        public UserClaim(int userId, Claim claim)
        {
            UserId = userId;
            Claim = claim;
        }
    }
}