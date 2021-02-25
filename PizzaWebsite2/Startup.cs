using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PizzaWebsite2.Startup))]
namespace PizzaWebsite2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
