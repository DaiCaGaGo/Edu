using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuyenMangGiuSo
{
    public class OneValueConfig
    {
        public static string JSESSIONID()
        {
            string value = "";
            try
            {
                value = System.Configuration.ConfigurationManager.AppSettings["JSESSIONID"];
            }
            catch { }
            return value;
        }
        public static string TS0165a601()
        {
            string value = "";
            try
            {
                value = System.Configuration.ConfigurationManager.AppSettings["TS0165a601"];
            }
            catch { }
            return value;
        }
        public static string TS017dff08()
        {
            string value = "";
            try
            {
                value = System.Configuration.ConfigurationManager.AppSettings["TS017dff08"];
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
        public static string urlId()
        {
            string value = "";
            try
            {
                value = System.Configuration.ConfigurationManager.AppSettings["urlId"];
            }
            catch { }
            return value;
        }
        public static string urlSave()
        {
            string value = "";
            try
            {
                value = System.Configuration.ConfigurationManager.AppSettings["urlSave"];
            }
            catch { }
            return value;
        }
    }
}
