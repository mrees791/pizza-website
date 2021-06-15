using Microsoft.Owin;
using Owin;
using PizzaWebsite;

[assembly: OwinStartupAttribute(typeof(Startup))]

namespace PizzaWebsite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}