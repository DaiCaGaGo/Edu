using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;

namespace CMS
{
    public class SYS_Cookies
    {
        public string getCookie(string cookieName, string setValue = "")
        {
            try
            {
                var bytes = Convert.FromBase64String(HttpContext.Current.Request.Cookies[cookieName].Value);
                var output = MachineKey.Unprotect(bytes, "thanhnv89t");
                string result = Encoding.UTF8.GetString(output);
                return result;
            }
            catch
            {
                return "";
            }
        }

        public void setCookie(string cookieName, string setValue,int day=1)
        {
            var cookieText = Encoding.UTF8.GetBytes(setValue);
            var encryptedValue = Convert.ToBase64String(MachineKey.Protect(cookieText, "thanhnv89t"));
            HttpCookie cookieObject = new HttpCookie(cookieName, encryptedValue);
            if (day > 0)
            {
                cookieObject.Expires.AddDays(day);
                HttpContext.Current.Response.Cookies.Add(cookieObject);
            }
            else
            {
                HttpContext.Current.Response.Cookies.Remove(cookieName);
            }
        }
        public void clearCookie()
        {
            HttpContext.Current.Response.Cookies.Clear();
        }
    }
}