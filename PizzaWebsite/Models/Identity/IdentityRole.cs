using DataLibrary.Models.Tables;
using Microsoft.AspNet.Identity;

namespace PizzaWebsite.Models.Identity
{
    public class IdentityRole : IRole<string>
    {
        public IdentityRole(string name)
        {
            Id = name;
        }

        public IdentityRole(SiteRole siteRole)
        {
            FromRecord(siteRole);
        }

        public string Id { get; set; }

        public string Name { get; set; }

        string IRole<string>.Id => Id;

        public SiteRole ToRecord()
        {
            return new SiteRole
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