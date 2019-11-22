using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Email
    {
        /// <summary>
        /// Sends the email alert.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="mess">The mess.</param>
        /// <param name="toEmail">To email.</param>
        /// <param name="emailCc">The email cc.</param>
        public void SendEmailAlert(string subject, string mess, string toEmail, string[] emailCc)
        {
            MailMessage mailObj = new MailMessage("admin@onesms.vn", toEmail)
            {
                Subject = subject,
                IsBodyHtml = true,
                Body = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "<br /> - Alert: " + mess
            };

            foreach (var itemEmail in emailCc)
            {
                MailAddress copy = new MailAddress(itemEmail);
                mailObj.CC.Add(copy);
            }

            SmtpClient smtpServer = new SmtpClient("mail.onesms.vn")
            {
                Port = 25,
                EnableSsl = false,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("admin@onesms.vn", "helsfs@&76")
            };
            smtpServer.Send(mailObj);
        }
    }
}
