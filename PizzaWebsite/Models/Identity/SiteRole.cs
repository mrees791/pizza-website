using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.Identity
{
    public class SiteRole : IRole<int>
    {
        private int id;

        public SiteRole()
        {
            id = int.Parse(Guid.NewGuid().ToString());
        }

        public SiteRole(string name) : this()
        {
            Name = name;
        }

        public SiteRole(string name, int id)
        {
            Name = name;
            this.id = id;
        }

        public string Name { get; set; }

        int IRole<int>.Id => id;

        public int Id
        {
            get
            {
                return id;
            }
        }
    }
}