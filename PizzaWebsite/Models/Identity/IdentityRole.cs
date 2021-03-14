using DataLibrary.Models.Tables;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.Identity
{
    public class IdentityRole : IRole<int>, IRecordViewModel<SiteRole>
    {
        private int id;

        public IdentityRole(string name)
        {
            Name = name;
        }

        public IdentityRole(SiteRole dbModel)
        {
            FromDbModel(dbModel);
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

        public SiteRole ToDbModel()
        {
            return new SiteRole()
            {
                Id = Id,
                Name = Name
            };
        }

        public void FromDbModel(SiteRole dbModel)
        {
            Id = dbModel.Id;
            Name = dbModel.Name;
        }
    }
}