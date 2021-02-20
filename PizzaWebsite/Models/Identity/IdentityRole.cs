using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.Identity
{
    public class IdentityRole : IRole<int>
    {
        private int id;

        public IdentityRole()
        {
            id = int.Parse(Guid.NewGuid().ToString());
        }

        public IdentityRole(string name) : this()
        {
            Name = name;
        }

        public IdentityRole(string name, int id)
        {
            Name = name;
            this.id = id;
        }

        public string Name { get; set; }

        int IRole<int>.Id => id;
    }
}