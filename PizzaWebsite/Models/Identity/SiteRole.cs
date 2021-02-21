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

        public SiteRole(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        int IRole<int>.Id => id;

        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }
    }
}