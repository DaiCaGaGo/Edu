using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorServiceSMPP
{
    public class OneValueConfig
    {
        public static string getservername()
        {
            string value = "";
            try
            {
                value = System.Configuration.ConfigurationManager.AppSettings["servername"];
            }
            catch { }
            return value;
        }
        public static string geturlLog()
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
