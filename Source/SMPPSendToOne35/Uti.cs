using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace SMPPSendToOne35
{
    public class Uti
    {
        public string GetTelco(string phone)
        {
            string Telco = "";
            string a = "";
            string b = "";
            if (phone.Length == 11 || phone.Length == 12)
            {
                a = phone.Substring(0, 4);
                b = phone.Substring(0, 5);
            }
            if (phone.Length != 11 && phone.Length != 12)
                Telco = "";
            // VMS
            else if (a == "8490" || a == "8493" || b == "84120" || b == "84121" || b == "84122" || b == "84126" || b == "84128")
                Telco = "VMS";
            // GPC 
            else if (a == "8491" || a == "8494" || b == "84123" || b == "84124" || b == "84125" || b == "84127" || b == "84129")
                Telco = "GPC";
            // Viettel 
            else if (a == "8497" || a == "8498" || b == "84161" || b == "84162" || b == "84163" || b == "84164" || b == "84165" || b == "84166" || b == "84167" || b == "84168" || b == "84169")
                Telco = "Viettel";
            // VNM
            else if (a == "8492" || b == "84188" || b == "84186")
                Telco = "VNM";
            // SFone
            else if (a == "8495" || b == "84155")
                Telco = "Sfone";
            // EVN
            else if (a == "8496")
                Telco = "EVN";
            // Gtel 
            else if (a == "8499" || b == "84199")
                Telco = "Gtel";
            else
                Telco = "";
            return Telco;
        }
        public int CountChar(string mes)
        {
            int meslen = 0;
            char[] charExten = { '\\', '\f', '^', '{', '}', '[', ']', '~', '|', '€' };
            foreach (char ch in mes)
            {
                int c = 1;
                for (int i = 0; i < charExten.Length; i++)
                {
                    if (charExten[i].Equals(ch))
                    {
                        c = 2; break;
                    }
                }
                meslen += c;
            }
            return meslen;
        }
        public byte CountMess(string mes, bool isUnicode)
        {
            int meslen = CountChar(mes);
            byte countmes = 1;
            if (isUnicode)
            {
                if (meslen <= 70) { countmes = 1; }
                else if (meslen > 70 && meslen <= 134) { countmes = 2; }
                else if (meslen > 134 && meslen <= 201) { countmes = 3; }
                else countmes = 4;
            }
            else
            {
                if (meslen <= 160) { countmes = 1; }
                else if (meslen > 160 && meslen <= 306) { countmes = 2; }
                else if (meslen > 306 && meslen <= 459) { countmes = 3; }
                else countmes = 4;
            }
            return countmes;
        }
        private bool IsTextValidated(string strTextEntry)
        {
            Regex objNotWholePattern = new Regex("/^(01[2689]|09|08)[0-9]{8}$/");
            return objNotWholePattern.IsMatch(strTextEntry);
        }
        public string FilterPhone(string phone)
        {
            Regex objNotWholePattern = new Regex("^[0-9]{9,12}$");            
            string str = "";
            //filter first 
            if (!objNotWholePattern.IsMatch(phone))
            {
                Regex patern = new Regex("^[0-9]{1}$");
                for (int i = 0; i < phone.Length; i++)
                {
                    if (patern.IsMatch(phone[i].ToString()))
                    {
                        str += phone[i].ToString();
                    }
                }
                phone = str;
            }
            //filter alter
            if (objNotWholePattern.IsMatch(phone))
            {
                if (phone.Length >= 1 && phone.Substring(0, 1) == "0")
                {
                    str = phone.Substring(1); // Loc ky tu 0 o dau
                }
                else if (phone.Length >= 2 && phone.Substring(0, 2) == "84")
                {
                    str = phone.Substring(2); // Loc ki tu 84 o dau
                }
                else
                {
                    str = phone; // giu nguyen
                }

                if (str.Length != 9 && str.Length != 10)
                {
                    str = ""; // loc cac so dai hay ngan qua
                }
            }
            else
            {
                str = "";
            }
            return str;
        }
        public void ghilog(string nameFile, string msg)
        {
            try
            {
                AppConfig appConfig = new AppConfig();
                string path = "";
                DateTime dt = new DateTime();
                dt = DateTime.Now;
                string foldername = dt.ToString();
                path = appConfig.UrlLog + dt.Year.ToString() + dt.Month.ToString() + dt.Day.ToString() + dt.Hour.ToString() + nameFile + ".txt";
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
        public string Add84(string Phone)
        {
            try
            {
                if (Phone.IndexOf("84") != 0)
                    return "84" + Phone.Substring(1, Phone.Length - 1);
                return Phone;
            }
            catch { return Phone; }
        }
    }
}
