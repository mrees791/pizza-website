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
        private string loginProvider;
        private string providerKey;

        public UserLogin(int userId, string loginProvider, string providerKey)
        {
            this.userId = userId;
            this.loginProvider = loginProvider;
            this.providerKey = providerKey;
        }

        public int Id { get => id; set => id = value; }
        public int UserId { get => userId; }
        public string LoginProvider { get => loginProvider; }
        public string ProviderKey { get => providerKey; }
    }
}