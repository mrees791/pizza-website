using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
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

            string toAddress = "acct@littlebrutus.ddns.net";
            mail.From = new MailAddress(ConfigurationManager.AppSettings["smtpEmailAddress"]);
            mail.To.Add(toAddress);
            mail.Subject = "Test mail subject 4.";
            mail.Body = "The test worked.";

            smtpServer.Port = 587;
            smtpServer.Credentials = new NetworkCredential(
                ConfigurationManager.AppSettings["smtpEmailAddress"],
                ConfigurationManager.AppSettings["smtpPassword"]);
            // todo: Enable SSL on production server.
            //smtpServer.EnableSsl = true;

            try
            {
                smtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Error sending email to {toAddress}.");
            }
        }
    }
}