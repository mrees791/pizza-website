using DataLibrary.Models.Tables;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.Identity
{
    public class IdentityRole : IRole<string>, IRecordConverter<SiteRole>
    {
        private string name;

        public IdentityRole(string name)
        {
            this.name = name;
        }

        public IdentityRole(SiteRole siteRole)
        {
            FromRecord(siteRole);
        }

        public string Name { get; set; }

        string IRole<string>.Id => name;

        public string Id
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public SiteRole ToRecord()
        {
            return new SiteRole()
            {
                Name = Name
            };
        }

        public void FromRecord(SiteRole siteRole)
        {
            Name = siteRole.Name;
        }
    }
}