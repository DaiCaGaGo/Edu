using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduSendSMS_VMG
{
    public class OneValueConfig
    {
        public static string ipserver()
        {
            string value = "";
            try
            {
                value = System.Configuration.ConfigurationManager.AppSettings["ipserver"];
            }
            catch { }
            return value;
        }
        public static string port()
        {
            string value = "";
            try
            {
                value = System.Configuration.ConfigurationManager.AppSettings["port"];
            }
            catch { }
            return value;
        }
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
        public static string systemtype()
        {
            string value = "";
            try
            {
                value = System.Configuration.ConfigurationManager.AppSettings["systemtype"];
            }
            catch { }
            return value;
        }
        public static string trantype()
        {
            string value = "";
            try
            {
                value = System.Configuration.ConfigurationManager.AppSettings["trantype"];
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
