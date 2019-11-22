using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CMS
{
    public partial class LoginFacebook : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string url = Request.Url.AbsoluteUri;

            if (url.Contains("access_token"))
            {

                string accessToken = Request.QueryString["access_token"];
                string requestUrl = "https://graph.facebook.com/me?access_token=" + accessToken;
                WebClient client = new WebClient();
                string userInformation = client.DownloadString(requestUrl);


                JavaScriptSerializer jss = new JavaScriptSerializer();
                var user = jss.Deserialize<FacebookUserInfo>(userInformation);


                if (user != null)
                {
                    //Luu thong tin dang nhap cua user vao day   
                    Session["user_Email"] = " Facebook: " + user.email;
                }


                //Dong trang dang nhap
                Response.Redirect("Close.aspx");
            }
            else
                if (url.Contains("error")) Response.Write("Có lỗi trong quá trình đăng nhập. <a href=\"javascript:window.close();\">Đóng cửa sổ</a>");
        }
    }
}

public class FacebookUserInfo
{
    public FacebookUserInfo()
    {
    }

    public string id { get; set; }
    public string name { get; set; }
    public string email { get; set; }
    public string first_name { get; set; }
    public string last_name { get; set; }
    public string link { get; set; }
    public string username { get; set; }
    public string gender { get; set; }
    public string timezone { get; set; }
    public string locale { get; set; }
    public string verified { get; set; }
    public string updated_time { get; set; }
    public string birthday { get; set; }

}