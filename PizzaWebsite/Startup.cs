using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PizzaWebsite.Startup))]
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
