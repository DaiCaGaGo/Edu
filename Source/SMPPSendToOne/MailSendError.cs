using System;
using System.Net.Mail;
using System.Net;

namespace SMPPSendToOne
{
    public class MailSendError
    {
        AppConfig _config;
        Uti _uti;

        public MailSendError()
        {
            _config = new AppConfig();
            _uti = new Uti();
        }

        public void OnSendError(string messsage)
        {
            try
            {
                SmtpClient smtpServer = new SmtpClient(_config.SmtpHost, int.Parse(_config.MailPort))
                {
                    EnableSsl = false,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_config.MailUser, _config.MailPass)
                };

                if (!string.IsNullOrEmpty(_config.MailTo))
                {
                    MailMessage mailObj = new MailMessage(_config.MailFrom, _config.MailTo)
                    {
                        Subject = _config.Subject,
                        IsBodyHtml = true,
                        Body = "- Thời gian: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "<br />- Thông tin: " + messsage
                    };

                    smtpServer.Send(mailObj);
                }

                if (!string.IsNullOrEmpty(_config.MailTo2))
                {
                    MailMessage mailObj = new MailMessage(_config.MailFrom, _config.MailTo2)
                    {
                        Subject = _config.Subject,
                        IsBodyHtml = true,
                        Body = "- Thời gian: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "<br />- Thông tin: " + messsage
                    };

                    smtpServer.Send(mailObj);
                }
            }
            catch (Exception ex)
            {
                _uti.ghilog("Mail_Send_Error", string.Format("Error sending e-mail. {0} {1}", ex, messsage));
            }
        }
    }
}
