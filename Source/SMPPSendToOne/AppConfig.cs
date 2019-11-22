using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace SMPPSendToOne
{
    public class AppConfig
    {
        private string Config(string arg)
        {
            try
            {
                return ConfigurationManager.AppSettings[arg];
            }
            catch
            {
                return "E:\\OneSMS";
            }
        }
        #region Email
        public int PollingTimeoutSeconds
        {
            get { return Int32.Parse(Config("PollingTimeoutSeconds")); }
        }
        public string MailFrom
        {
            get { return Config("MailFrom"); }
        }
        public string MailPort
        {
            get { return Config("MailPort"); }
        }
        public string MailUser
        {
            get { return Config("MailUser"); }
        }
        public string MailPass
        {
            get { return Config("MailPass"); }
        }
        public string MailTo
        {
            get { return Config("MailTo"); }
        }
        public string MailTo2
        {
            get { return Config("MailTo2"); }
        }
        public string Subject
        {
            get { return Config("Subject"); }
        }
        public string SmtpHost
        {
            get { return Config("SmtpHost"); }
        }
        #endregion
        #region DataBase
        public string DBInstanceName
        {
            get { return Config("DBInstanceName"); }
        }
        public string DbUser
        {
            get { return Config("DbUser"); }
        }
        public string DbPass
        {
            get { return Config("DbPass"); }
        }
        public string DbSource
        {
            get { return Config("DbSource"); }
        }
        #endregion
        #region SMPP
        public string SMPPServer
        {
            get { return Config("SMPPServer"); }
        }
        public string SMPPPort
        {
            get { return Config("SMPPPort"); }
        }
        public string SMPPUser
        {
            get { return Config("SMPPUser"); }
        }
        public string SMPPPass
        {
            get { return Config("SMPPPass"); }
        }
        public string SMPPSendername
        {
            get { return Config("SMPPSendername"); }
        }
        public string SMPPSystemtype
        {
            get { return Config("SMPPSystemtype"); }
        }
        public string SMPPTrantype
        {
            get { return Config("SMPPTrantype"); }
        }
        #endregion
        public string UrlLog
        {
            get { return Config("UrlLog"); }
        }
    }
}
