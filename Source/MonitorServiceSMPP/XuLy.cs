using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorServiceSMPP
{
    public class XuLy
    {
        public static void ghilog(string nameFile, string msg)
        {
            try
            {
                string path = "";
                DateTime dt = new DateTime();
                dt = DateTime.Now;
                string foldername = dt.ToString();
                path = OneValueConfig.geturlLog() + dt.Year.ToString() + dt.Month.ToString() + dt.Day.ToString() + dt.Hour.ToString() + nameFile + ".txt";
                using (StreamWriter sw = new StreamWriter(path, true))
                {
                    string str = DateTime.Now.ToString() + ": " + msg;
                    sw.WriteLine(str);
                }
            }
            catch
            {

            }
        }
    }
}
