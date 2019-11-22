using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduSendSms_VMG_API
{
    public class XuLy
    {
        public static string GetTelco(string phone)
        {
            var telco = "";
            var a = "";
            var b = "";

            if (phone.Length == 11 || phone.Length == 12)
            {
                a = phone.Substring(0, 4);
                b = phone.Substring(0, 5);
            }
            if (phone.Length != 11 && phone.Length != 12)
                telco = "";
            // VMS
            else if (a == "8490" || a == "8493" || a == "8489" || b == "84120" || b == "84121" || b == "84122" || b == "84126" || b == "84128")
                telco = "VMS";
            // GPC
            else if (a == "8491" || a == "8494" || a == "8488" || b == "84123" || b == "84124" || b == "84125" || b == "84127" || b == "84129")
                telco = "GPC";
            // Viettel
            else if (a == "8497" || a == "8498" || a == "8486" || b == "84161" || b == "84162" || b == "84163" || b == "84164" || b == "84165" || b == "84166" || b == "84167" || b == "84168" || b == "84169")
                telco = "Viettel";
            // VNM
            else if (a == "8492" || b == "84188" || b == "84186")
                telco = "VNM";
            // SFone
            else if (a == "8495" || b == "84155")
                telco = "Sfone";
            // EVN
            else if (a == "8496")
                telco = "Viettel";
            else if (a == "8452" || a == "8458" || a == "8456")
                telco = "VNM";

            else if (a == "8499" || a == "8459" || b == "84199")
                telco = "Gtel";
            else if (a == "8452")
                telco = "VNM";
            else if (a == "8484" || a == "8481" || a == "8482" || a == "8483" || a == "8485")
                telco = "GPC";
            else if (a == "8470" || a == "8479" || a == "8477" || a == "8476" || a == "8478")
                telco = "VMS";
            else if (a == "8439" || a == "8438" || a == "8437" || a == "8436" || a == "8435"
                || a == "8434" || a == "8433" || a == "8432")
                telco = "Viettel";
            else
                telco = "";
            return telco;
        }
        public static string Add84(string Phone)
        {
            try
            {
                if (Phone.IndexOf("84") != 0)
                    return "84" + Phone.Substring(1, Phone.Length - 1);
                return Phone;
            }
            catch { return Phone; }
        }
        public static void ghilog(string nameFile, string msg)
        {
            try
            {
                string path = "";
                DateTime dt = new DateTime();
                dt = DateTime.Now;
                string foldername = dt.ToString();
                path = OneValueConfig.urlLog() + dt.Year.ToString() + dt.Month.ToString() + dt.Day.ToString() + dt.Hour.ToString() + nameFile + ".txt";
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
        public static string RemoveSigns(string sms)
        {
            // Bỏ dấu
            sms = sms.Replace('á', 'a').Replace('à', 'a').Replace('ả', 'a').Replace('ã', 'a').Replace('ạ', 'a');
            sms = sms.Replace('â', 'a').Replace('ấ', 'a').Replace('ầ', 'a').Replace('ẩ', 'a').Replace('ẫ', 'a').Replace('ậ', 'a');
            sms = sms.Replace('ă', 'a').Replace('ắ', 'a').Replace('ằ', 'a').Replace('ẳ', 'a').Replace('ẵ', 'a').Replace('ặ', 'a');
            sms = sms.Replace('Á', 'A').Replace('À', 'A').Replace('Ả', 'A').Replace('Ã', 'A').Replace('Ạ', 'A');
            sms = sms.Replace('Â', 'A').Replace('Ấ', 'A').Replace('Ầ', 'A').Replace('Ẩ', 'A').Replace('Ẫ', 'A').Replace('Ậ', 'A');
            sms = sms.Replace('Ă', 'A').Replace('Ắ', 'A').Replace('Ằ', 'A').Replace('Ẩ', 'A').Replace('Ẫ', 'A').Replace('Ậ', 'A');

            sms = sms.Replace('đ', 'd');
            sms = sms.Replace('Đ', 'D');

            sms = sms.Replace('é', 'e').Replace('è', 'e').Replace('ẻ', 'e').Replace('ẽ', 'e').Replace('ẹ', 'e');
            sms = sms.Replace('ê', 'e').Replace('ế', 'e').Replace('ề', 'e').Replace('ể', 'e').Replace('ễ', 'e').Replace('ệ', 'e');
            sms = sms.Replace('É', 'E').Replace('È', 'E').Replace('Ẻ', 'E').Replace('Ẽ', 'E').Replace('Ẹ', 'E');
            sms = sms.Replace('Ê', 'E').Replace('Ế', 'E').Replace('Ề', 'E').Replace('Ể', 'E').Replace('Ễ', 'E').Replace('Ệ', 'E');

            sms = sms.Replace('í', 'i').Replace('ì', 'i').Replace('ỉ', 'i').Replace('ĩ', 'i').Replace('ị', 'i');
            sms = sms.Replace('Í', 'I').Replace('Ì', 'I').Replace('Ỉ', 'I').Replace('Ĩ', 'I').Replace('Ị', 'I');

            sms = sms.Replace('ó', 'o').Replace('ò', 'o').Replace('ỏ', 'o').Replace('õ', 'o').Replace('ọ', 'o');
            sms = sms.Replace('ô', 'o').Replace('ố', 'o').Replace('ổ', 'o').Replace('ồ', 'o').Replace('ỗ', 'o').Replace('ộ', 'o');
            sms = sms.Replace('ơ', 'o').Replace('ớ', 'o').Replace('ờ', 'o').Replace('ở', 'o').Replace('ỡ', 'o').Replace('ợ', 'o');
            sms = sms.Replace('Ó', 'O').Replace('Ò', 'O').Replace('Ỏ', 'O').Replace('Õ', 'O').Replace('Ọ', 'O');
            sms = sms.Replace('Ô', 'O').Replace('Ố', 'O').Replace('Ồ', 'O').Replace('Ổ', 'O').Replace('Ỗ', 'O').Replace('Ộ', 'O');
            sms = sms.Replace('Ơ', 'O').Replace('Ớ', 'O').Replace('Ờ', 'O').Replace('Ở', 'O').Replace('Ỡ', 'O').Replace('Ợ', 'O');

            sms = sms.Replace('ù', 'u').Replace('ú', 'u').Replace('ũ', 'u').Replace('ủ', 'u').Replace('ụ', 'u');
            sms = sms.Replace('ư', 'u').Replace('ứ', 'u').Replace('ử', 'u').Replace('ừ', 'u').Replace('ữ', 'u').Replace('ự', 'u');
            sms = sms.Replace('Ù', 'U').Replace('Ú', 'U').Replace('Ũ', 'U').Replace('Ủ', 'U').Replace('Ụ', 'U');
            sms = sms.Replace('Ư', 'U').Replace('Ứ', 'U').Replace('Ử', 'U').Replace('Ừ', 'U').Replace('Ữ', 'U').Replace('Ự', 'U');

            sms = sms.Replace('ỳ', 'y').Replace('ý', 'y').Replace('ỹ', 'y').Replace('ỷ', 'y').Replace('ỵ', 'y');
            sms = sms.Replace('Ỳ', 'y').Replace('Ý', 'y').Replace('Ỹ', 'y').Replace('Ỷ', 'y').Replace('Ỵ', 'y');

            // Loai bo ky tu dac biet
            StringBuilder sb = new StringBuilder();
            foreach (char c in sms)
            {
                if (
                    (c >= '0' && c <= '9') ||
                    (c >= 'A' && c <= 'Z') ||
                    (c >= 'a' && c <= 'z') ||
                    c == ' ' ||
                    c.ToString() == Environment.NewLine ||
                    c == '`' ||
                    c == '~' ||
                    c == '!' ||
                    c == '@' ||
                    c == '#' ||
                    c == '$' ||
                    c == '%' ||
                    c == '^' ||
                    c == '&' ||
                    c == '*' ||
                    c == '(' ||
                    c == ')' ||
                    c == '-' ||
                    c == '_' ||
                    c == '=' ||
                    c == '+' ||
                    c == '\\' ||
                    c == '|' ||
                    c == ']' ||
                    c == '}' ||
                    c == '[' ||
                    c == '{' ||
                    c == '\'' ||
                    c == '"' ||
                    c == ';' ||
                    c == ':' ||
                    c == '/' ||
                    c == '?' ||
                    c == '.' ||
                    c == '>' ||
                    c == ',' ||
                    c == '<'
                    )
                {
                    sb.Append(c);
                }
            }
            sms = sb.ToString().Trim();

            return sms;
        }
        public static string ConvertToString(object s)
        {
            if (s != null) return s.ToString();
            else return "";
        }
        public static string ReplaceVietnammese(string msg)
        {
            return RemoveSigns(msg);
        }
    }
}
