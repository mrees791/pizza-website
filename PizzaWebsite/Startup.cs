using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using PizzaWebsite.Models.Tests;
using System;
using System.Configuration;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(PizzaWebsite.Startup))]

namespace PizzaWebsite
{
    public class Startup
    {
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie
            });

            /*app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            app.UseGoogleAuthentication(
                ConfigurationManager.AppSettings["googleOAuth2ClientId"],
                ConfigurationManager.AppSettings["googleOAuth2ClientSecret"]);*/
        }
    }
}
