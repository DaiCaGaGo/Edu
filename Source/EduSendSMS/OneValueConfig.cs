using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduSendSMS
{
    public class OneValueConfig
    {
        public static string username()
        {
            string value = "";
            try
            {
                value = System.Configuration.ConfigurationManager.AppSettings["username"];
            }
            catch { }
            return value;
        }
        public static string password()
        {
            string value = "";
            try
            {
                value = System.Configuration.ConfigurationManager.AppSettings["password"];
            }
            catch { }
            return value;
        }
        public static string headid()
        {
            string value = "";
            try
            {
                value = System.Configuration.ConfigurationManager.AppSettings["headid"];
            }
            catch { }
            return value;
        }
        public static string timeDelay()
        {
            string value = "";
            try
            {
                value = System.Configuration.ConfigurationManager.AppSettings["timeDelay"];
            }
            catch { }
            return value;
        }
        public static string version()
        {
            string value = "";
            try
            {
                value = System.Configuration.ConfigurationManager.AppSettings["version"];
            }
            catch { }
            return value;
        }
        public static string apiTimeout()
        {
            string value = "";
            try
            {
                value = System.Configuration.ConfigurationManager.AppSettings["apiTimeout"];
            }
            catch { }
            return value;
        }
        public static string urlLog()
        {
            string value = "";
            try
            {
                value = System.Configuration.ConfigurationManager.AppSettings["urlLog"];
            }
            catch { }
            return value;
        }
    }
}
