using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PizzaWebsite.Models.Identity;
using PizzaWebsite.Models.Identity.Validators;
using PizzaWebsite.Models.Tests;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace PizzaWebsite.App_Start
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            await ConfigSendAsync(message);
        }

        private async Task ConfigSendAsync(IdentityMessage message)
        {
            MailMessage mail = new MailMessage();
            SmtpClient smtpServer = new SmtpClient(ConfigurationManager.AppSettings["smtpHost"]);

            mail.From = new MailAddress(ConfigurationManager.AppSettings["smtpEmailAddress"]);
            mail.To.Add(message.Destination);
            mail.Subject = message.Subject;
            mail.Body = message.Body;
            mail.IsBodyHtml = true;

            smtpServer.Port = int.Parse(ConfigurationManager.AppSettings["smtpPort"]);
            smtpServer.Credentials = new NetworkCredential(
                ConfigurationManager.AppSettings["smtpEmailAddress"],
                ConfigurationManager.AppSettings["smtpPassword"]);
            // todo: Enable SSL on production server.
            //smtpServer.EnableSsl = true;

            try
            {
                await smtpServer.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Error sending email to {message.Destination}.\n{ex.StackTrace}");
                await Task.FromResult(0);
            }
        }
    }
}